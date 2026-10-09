using Microsoft.Extensions.Options;
using TorneoDeTitanes.Console.Infraestructura;
using TorneoDeTitanes.Core.Abstracciones;
using TorneoDeTitanes.Core.Combate;
using TorneoDeTitanes.Core.Modelos;
using TorneoDeTitanes.Core.Personajes;

namespace TorneoDeTitanes.Console.Ui;

public sealed class TorneoConsola
{
    private readonly IPersonajeRepositorio _personajes;
    private readonly IHistorialRepositorio _historial;
    private readonly IPersonajeApiClient _api;
    private readonly FabricaDePersonajes _fabrica;
    private readonly MotorDeCombate _motor;
    private readonly ISelectorAccion _selectorIa;
    private readonly SelectorAccionConsola _selectorJugador;
    private readonly ObservadorCombateConsola _observador;
    private readonly IRandomProvider _random;
    private readonly OpcionesJuego _opciones;

    public TorneoConsola(
        IPersonajeRepositorio personajes,
        IHistorialRepositorio historial,
        IPersonajeApiClient api,
        FabricaDePersonajes fabrica,
        MotorDeCombate motor,
        ISelectorAccion selectorIa,
        SelectorAccionConsola selectorJugador,
        ObservadorCombateConsola observador,
        IRandomProvider random,
        IOptions<OpcionesJuego> opciones)
    {
        _personajes = personajes;
        _historial = historial;
        _api = api;
        _fabrica = fabrica;
        _motor = motor;
        _selectorIa = selectorIa;
        _selectorJugador = selectorJugador;
        _observador = observador;
        _random = random;
        _opciones = opciones.Value;
    }

    public async Task EjecutarAsync()
    {
        Pantallas.MostrarIntroduccion();

        while (true)
        {
            string opcion = Pantallas.MostrarMenuPrincipal();
            switch (opcion)
            {
                case "Jugar Torneo":
                    await JugarTorneoAsync();
                    break;

                case "Ver Historial de Campeones":
                    MostrarHistorial();
                    Pantallas.Pausar("Presiona una tecla para volver al menú principal...");
                    break;

                default:
                    return;
            }
        }
    }

    private async Task JugarTorneoAsync()
    {
        List<Personaje> roster = await ObtenerRosterAsync();
        if (roster.Count == 0)
        {
            Pantallas.MensajeSinPersonajes();
            return;
        }

        Personaje? seleccionado = Pantallas.SeleccionarPersonaje(roster);
        if (seleccionado is null)
        {
            return;
        }

        Personaje jugador = seleccionado;
        var rivales = ConstruirRivales(roster, jugador);

        for (int ronda = 1; ronda <= _opciones.RondasTorneo; ronda++)
        {
            if (rivales.Count == 0)
            {
                rivales = ConstruirRivales(roster, jugador);
            }

            int indice = _random.Siguiente(0, rivales.Count);
            Personaje rival = rivales[indice];
            rivales.RemoveAt(indice);

            Pantallas.MostrarEncabezadoRonda(ronda, _opciones.RondasTorneo, jugador, rival);

            ResultadoCombate resultado = _motor.Combatir(
                jugador,
                rival,
                _selectorJugador,
                _selectorIa,
                _observador);

            Pantallas.MostrarVictoriaRonda(resultado.Ganador, resultado.Perdedor);

            bool elJugadorSigueEnPie = ReferenceEquals(resultado.Ganador, jugador) && jugador.EstaVivo;

            if (resultado.PorRendicion)
            {
                ProgresionPersonaje.Penalizar(resultado.Perdedor);
                Pantallas.MostrarPenalizacion(resultado.Perdedor);
            }

            ProgresionPersonaje.Mejorar(resultado.Ganador, _random);
            if (elJugadorSigueEnPie)
            {
                Pantallas.MostrarMejora(jugador);
            }

            jugador = resultado.Ganador;

            if (!elJugadorSigueEnPie)
            {
                Pantallas.MensajeEliminado();
                break;
            }

            string espera = ronda == _opciones.RondasTorneo
                ? "Presiona una tecla para conocer al campeón del torneo..."
                : "Presiona una tecla para la próxima ronda...";
            Pantallas.Pausar(espera);
        }

        Pantallas.MostrarCampeon(jugador);
        _historial.RegistrarGanador(jugador);

        PersistirRoster(roster);
        Pantallas.Pausar("Presiona una tecla para volver al menú principal...");
    }

    private void PersistirRoster(List<Personaje> roster)
    {
        foreach (var personaje in roster)
        {
            personaje.Restaurar();
        }

        _personajes.Guardar(roster);
    }

    private void MostrarHistorial()
    {
        var historial = _historial.Leer();
        Pantallas.MostrarHistorial(historial);
    }

    private async Task<List<Personaje>> ObtenerRosterAsync()
    {
        if (_personajes.Existe())
        {
            var existentes = _personajes.Leer();
            if (existentes.Count > 0)
            {
                return existentes;
            }
        }

        var datos = await _api.ObtenerPersonajesAsync();
        if (datos.Count == 0)
        {
            datos = PersonajesDeRespaldo.Obtener();
        }

        var roster = _fabrica.CrearSeleccion(datos, _opciones.CantidadPersonajes);
        _personajes.Guardar(roster);
        return roster;
    }

    private static List<Personaje> ConstruirRivales(IReadOnlyList<Personaje> roster, Personaje jugador)
        => roster.Where(p => !ReferenceEquals(p, jugador)).ToList();
}