using TorneoDeTitanes.Core.Abstracciones;
using TorneoDeTitanes.Core.Habilidades;
using TorneoDeTitanes.Core.Modelos;

namespace TorneoDeTitanes.Core.Combate;

public sealed class MotorDeCombate
{
    private const int MaximoTurnos = 100;
    private const int RegeneracionEnergiaPorTurno = 12;
    private const int EnergiaRecuperadaAlDefender = 18;
    private const int EnergiaGanadaAlAtacar = 6;
    private const int FuriaGanadaAlAtacar = 12;
    private const int FuriaGanadaAlRecibirDano = 8;
    private const int EscudoBaseAlDefender = 8;
    private const int EscudoPorArmadura = 2;
    private const double PorcentajeContraataque = 0.30;

    private readonly ICalculadoraDano _calculadoraDano;
    private readonly IRandomProvider _random;
    private readonly IProveedorHabilidades _habilidades;

    public MotorDeCombate(
        ICalculadoraDano calculadoraDano,
        IRandomProvider random,
        IProveedorHabilidades habilidades)
    {
        _calculadoraDano = calculadoraDano;
        _random = random;
        _habilidades = habilidades;
    }

    public ResultadoCombate Combatir(
        Personaje jugador,
        Personaje rival,
        ISelectorAccion selectorJugador,
        ISelectorAccion selectorRival,
        ICombateObserver observer)
    {
        jugador.Restaurar();
        rival.Restaurar();

        observer.AlIniciarCombate(jugador, rival);

        int numeroTurno = 1;
        bool turnoDelJugador = jugador.Velocidad >= rival.Velocidad;
        bool huboRendicion = false;

        while (jugador.EstaVivo && rival.EstaVivo && numeroTurno <= MaximoTurnos)
        {
            Personaje actor = turnoDelJugador ? jugador : rival;
            Personaje oponente = turnoDelJugador ? rival : jugador;
            ISelectorAccion selector = turnoDelJugador ? selectorJugador : selectorRival;

            observer.AlActualizarEstado(jugador, rival, numeroTurno, turnoDelJugador);

            bool aturdido = ProcesarEfectosAlInicio(actor, observer);
            ReducirRecarga(actor);

            if (!actor.EstaVivo || !oponente.EstaVivo)
            {
                break;
            }

            if (aturdido)
            {
                observer.AlMostrarEvento(
                    new EventoCombate($"{actor.Nombre} está aturdido y pierde su turno.", TipoEvento.Estado));
            }
            else
            {
                RegenerarEnergia(actor);

                var habilidad = _habilidades.Obtener(actor.Tipo, NivelHabilidad.Especial);
                var ultimate = _habilidades.Obtener(actor.Tipo, NivelHabilidad.Ultimate);
                var acciones = DeterminarAccionesDisponibles(actor, habilidad);
                var contexto = new ContextoTurno
                {
                    Actor = actor,
                    Oponente = oponente,
                    NumeroTurno = numeroTurno,
                    AccionesDisponibles = acciones,
                    HabilidadEspecial = habilidad,
                    HabilidadUltimate = ultimate,
                };

                AccionDisponible accion = selector.ElegirAccion(contexto);
                var resultado = new ResultadoAccion();

                EjecutarAccion(accion, actor, oponente, habilidad, ultimate, resultado);

                foreach (var evento in resultado.Eventos)
                {
                    observer.AlMostrarEvento(evento);
                }

                if (resultado.EsRendicion)
                {
                    huboRendicion = true;
                }

                if (resultado.DanoInfligido > 0)
                {
                    actor.GanarFuria(FuriaGanadaAlAtacar);
                    oponente.GanarFuria(FuriaGanadaAlRecibirDano);
                }
            }

            turnoDelJugador = !turnoDelJugador;
            numeroTurno++;
        }

        Personaje ganador = DeterminarGanador(jugador, rival);
        Personaje perdedor = ReferenceEquals(ganador, jugador) ? rival : jugador;

        observer.AlMostrarEvento(
            new EventoCombate(
                huboRendicion
                    ? $"{perdedor.Nombre} abandona el campo de batalla entre silbidos."
                    : $"{perdedor.Nombre} cae derrotado.",
                huboRendicion ? TipoEvento.Rendicion : TipoEvento.Muerte));
        observer.AlFinalizarCombate(ganador);

        return new ResultadoCombate(ganador, perdedor, numeroTurno - 1, huboRendicion);
    }

    private static bool ProcesarEfectosAlInicio(Personaje personaje, ICombateObserver observer)
    {
        int danoDeEstados = personaje.Estados
            .Where(e => e.Tipo is EstadoAlterado.Sangrado or EstadoAlterado.Veneno or EstadoAlterado.Quemadura)
            .Sum(e => e.Potencia);

        if (danoDeEstados > 0)
        {
            personaje.RecibirDano(danoDeEstados);
            observer.AlMostrarEvento(
                new EventoCombate($"{personaje.Nombre} sufre {danoDeEstados} de daño por sus heridas.", TipoEvento.Estado));
        }

        int curacionEstados = personaje.Estados
            .Where(e => e.Tipo == EstadoAlterado.Regeneracion)
            .Sum(e => e.Potencia);

        if (curacionEstados > 0 && personaje.EstaVivo)
        {
            int antes = personaje.Salud;
            personaje.Curar(curacionEstados);
            int curado = personaje.Salud - antes;
            if (curado > 0)
            {
                observer.AlMostrarEvento(
                    new EventoCombate($"{personaje.Nombre} regenera {curado} de salud.", TipoEvento.Curacion));
            }
        }

        bool aturdido = personaje.TieneEstado(EstadoAlterado.Aturdido);

        foreach (var efecto in personaje.Estados)
        {
            efecto.Duracion--;
        }

        personaje.Estados.RemoveAll(e => e.Duracion <= 0);

        return aturdido;
    }

    private static void ReducirRecarga(Personaje personaje)
    {
        if (personaje.TurnosRecargaHabilidad > 0)
        {
            personaje.TurnosRecargaHabilidad--;
        }
    }

    public static IReadOnlyList<AccionDisponible> DeterminarAccionesDisponibles(Personaje actor, IHabilidad habilidad)
    {
        var acciones = new List<AccionDisponible> { AccionDisponible.Atacar };

        bool habilidadDisponible =
            actor.Energia >= habilidad.CostoEnergia && actor.TurnosRecargaHabilidad <= 0;
        if (habilidadDisponible)
        {
            acciones.Add(AccionDisponible.Habilidad);
        }

        if (actor.PuedeDesatarUltimate)
        {
            acciones.Add(AccionDisponible.Ultimate);
        }

        acciones.Add(AccionDisponible.Defender);
        return acciones;
    }

    private static void RegenerarEnergia(Personaje personaje)
        => personaje.Energia = Math.Min(personaje.EnergiaMaxima, personaje.Energia + RegeneracionEnergiaPorTurno);

    private void EjecutarAccion(
        AccionDisponible accion,
        Personaje actor,
        Personaje oponente,
        IHabilidad habilidad,
        IHabilidad ultimate,
        ResultadoAccion resultado)
    {
        switch (accion)
        {
            case AccionDisponible.Ultimate:
                actor.Furia = 0;
                resultado.Agregar(
                    $"¡¡{actor.Nombre} desata su TÉCNICA DEFINITIVA: {ultimate.Nombre}!!",
                    TipoEvento.Habilidad);
                ultimate.Ejecutar(
                    new ContextoAccion
                    {
                        Atacante = actor,
                        Defensor = oponente,
                        Random = _random,
                        CalculadoraDano = _calculadoraDano,
                    },
                    resultado);
                break;

            case AccionDisponible.Habilidad:
                resultado.Agregar($"{actor.Nombre} desata su habilidad: {habilidad.Nombre}.", TipoEvento.Habilidad);
                habilidad.Ejecutar(
                    new ContextoAccion
                    {
                        Atacante = actor,
                        Defensor = oponente,
                        Random = _random,
                        CalculadoraDano = _calculadoraDano,
                    },
                    resultado);
                actor.TurnosRecargaHabilidad = habilidad.TurnosRecarga;
                break;

            case AccionDisponible.Defender:
                int escudo = EscudoBaseAlDefender + actor.Armadura * EscudoPorArmadura;
                actor.OtorgarEscudo(escudo);
                actor.AgregarEstado(new EfectoEstado
                {
                    Tipo = EstadoAlterado.Fortalecido,
                    Duracion = 1,
                    Potencia = 1,
                    Fuente = "Postura defensiva",
                });
                actor.Energia = Math.Min(actor.EnergiaMaxima, actor.Energia + EnergiaRecuperadaAlDefender);
                resultado.Agregar(
                    $"{actor.Nombre} adopta una postura defensiva, gana {escudo} de escudo y recupera energía.",
                    TipoEvento.Bloqueo);
                break;

            case AccionDisponible.Rendirse:
                actor.RecibirDano(actor.Salud + actor.Escudo + 1);
                resultado.EsRendicion = true;
                resultado.Agregar(
                    $"{actor.Nombre} baja las armas y se rinde ante {oponente.Nombre}.",
                    TipoEvento.Rendicion);
                break;

            default:
                AtaqueBasico(actor, oponente, resultado);
                break;
        }
    }

    private void AtaqueBasico(Personaje actor, Personaje oponente, ResultadoAccion resultado)
    {
        var calculo = _calculadoraDano.Calcular(actor, oponente, defensorEstaDefendiendo: false, _random);

        if (calculo.Esquivado)
        {
            resultado.Agregar($"{oponente.Nombre} esquiva el golpe de {actor.Nombre}.", TipoEvento.Esquiva);
            return;
        }

        oponente.RecibirDano(calculo.Dano);
        resultado.DanoInfligido += calculo.Dano;

        string prefijo = calculo.Critico ? "¡GOLPE CRÍTICO! " : string.Empty;
        resultado.Agregar(
            $"{prefijo}{actor.Nombre} golpea a {oponente.Nombre} y causa {calculo.Dano} de daño.",
            calculo.Critico ? TipoEvento.Critico : TipoEvento.Ataque);

        actor.Energia = Math.Min(actor.EnergiaMaxima, actor.Energia + EnergiaGanadaAlAtacar);
        EjecutarContraataque(actor, oponente, calculo.Dano, resultado);
    }

    private static void EjecutarContraataque(Personaje actor, Personaje oponente, int danoRecibido, ResultadoAccion resultado)
    {
        if (!oponente.EstaVivo || !oponente.TieneEstado(EstadoAlterado.Fortalecido))
        {
            return;
        }

        int contraataque = Math.Max(1, (int)Math.Round(danoRecibido * PorcentajeContraataque));
        actor.RecibirDano(contraataque);
        resultado.Agregar(
            $"{oponente.Nombre} contraataca desde su postura defensiva y causa {contraataque} de daño.",
            TipoEvento.Ataque);
    }

    private static Personaje DeterminarGanador(Personaje jugador, Personaje rival)
    {
        if (jugador.EstaVivo && !rival.EstaVivo)
        {
            return jugador;
        }

        if (rival.EstaVivo && !jugador.EstaVivo)
        {
            return rival;
        }

        return jugador.PorcentajeSalud >= rival.PorcentajeSalud ? jugador : rival;
    }
}
