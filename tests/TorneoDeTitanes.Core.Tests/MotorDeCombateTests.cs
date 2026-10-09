using TorneoDeTitanes.Core.Abstracciones;
using TorneoDeTitanes.Core.Combate;
using TorneoDeTitanes.Core.Habilidades;
using TorneoDeTitanes.Core.Modelos;
using TorneoDeTitanes.Core.Tests.Dobles;

namespace TorneoDeTitanes.Core.Tests;

public sealed class MotorDeCombateTests
{
    [Fact]
    public void Combatir_UnTitanFuerteDerrotaAOtroDebil()
    {
        var jugador = FabricaDePersonajesDePrueba.Crear(
            tipo: TipoPersonaje.Colosal, nombre: "Fuerte", fuerza: 30, nivel: 5);
        var rival = FabricaDePersonajesDePrueba.Crear(
            tipo: TipoPersonaje.Ataque, nombre: "Débil", fuerza: 1, destreza: 1, velocidad: 1, armadura: 0, nivel: 1);
        var motor = CrearMotor(new RandomSecuencia(Enumerable.Repeat(0.99d, 100)));
        var observador = new ObservadorNulo();

        var resultado = motor.Combatir(
            jugador, rival, new SelectorFijo(AccionDisponible.Atacar), new SelectorFijo(AccionDisponible.Atacar), observador);

        Assert.Same(jugador, resultado.Ganador);
        Assert.Same(jugador, observador.Ganador);
        Assert.Same(rival, resultado.Perdedor);
        Assert.NotEmpty(observador.Eventos);
    }

    [Fact]
    public void Combatir_AlCompletarseLaFuria_SeDesataLaTecnicaDefinitiva()
    {
        var jugador = FabricaDePersonajesDePrueba.Crear(
            tipo: TipoPersonaje.Colosal, nombre: "Coloso", fuerza: 10, destreza: 5, velocidad: 10, armadura: 2, nivel: 5, saludMaxima: 2000);
        var rival = FabricaDePersonajesDePrueba.Crear(
            tipo: TipoPersonaje.Ataque, nombre: "Mazmorra", fuerza: 3, destreza: 1, velocidad: 1, armadura: 0, nivel: 1, saludMaxima: 4000);
        var motor = CrearMotor(new RandomSecuencia(Enumerable.Repeat(0.99d, 1000)));
        var observador = new ObservadorNulo();
        var selectorJugador = new SelectorFijo(ctx =>
            ctx.AccionesDisponibles.Contains(AccionDisponible.Ultimate) ? AccionDisponible.Ultimate : AccionDisponible.Atacar);

        var resultado = motor.Combatir(jugador, rival, selectorJugador, new SelectorFijo(AccionDisponible.Atacar), observador);

        Assert.Same(jugador, resultado.Ganador);
        Assert.Contains(observador.Eventos, e => e.Texto.Contains("TÉCNICA DEFINITIVA"));
        Assert.Contains(observador.Eventos, e => e.Tipo == TipoEvento.Estado && e.Texto.Contains("vapor"));
    }

    [Fact]
    public void Combatir_AccionDefender_GeneraContraataque()
    {
        var jugador = FabricaDePersonajesDePrueba.Crear(
            tipo: TipoPersonaje.Ataque, nombre: "Atacante", fuerza: 30, destreza: 5, velocidad: 10, armadura: 2, nivel: 5, saludMaxima: 500);
        var rival = FabricaDePersonajesDePrueba.Crear(
            tipo: TipoPersonaje.Acorazado, nombre: "Muro", fuerza: 5, destreza: 1, velocidad: 1, armadura: 10, nivel: 1, saludMaxima: 500);
        var motor = CrearMotor(new RandomSecuencia(Enumerable.Repeat(0.99d, 100)));
        var observador = new ObservadorNulo();

        motor.Combatir(
            jugador, rival, new SelectorFijo(AccionDisponible.Atacar), new SelectorFijo(AccionDisponible.Defender), observador);

        Assert.Contains(observador.Eventos, e => e.Texto.Contains("contraataca"));
    }

    [Fact]
    public void AccionesDisponibles_ConEnergiaYSinRecarga_IncluyeAtaqueHabilidadYDefensa()
    {
        var actor = FabricaDePersonajesDePrueba.Crear(tipo: TipoPersonaje.Colosal, fuerza: 10);
        var habilidad = new HabilidadColosal();

        var acciones = MotorDeCombate.DeterminarAccionesDisponibles(actor, habilidad);

        Assert.Contains(AccionDisponible.Atacar, acciones);
        Assert.Contains(AccionDisponible.Habilidad, acciones);
        Assert.Contains(AccionDisponible.Defender, acciones);
        Assert.DoesNotContain(AccionDisponible.Ultimate, acciones);
    }

    [Fact]
    public void AccionesDisponibles_SinEnergiaSuficiente_NoOfreceHabilidad()
    {
        var actor = FabricaDePersonajesDePrueba.Crear(tipo: TipoPersonaje.Colosal, fuerza: 10);
        actor.Energia = 10;
        var habilidad = new HabilidadColosal(); // costo 40

        var acciones = MotorDeCombate.DeterminarAccionesDisponibles(actor, habilidad);

        Assert.DoesNotContain(AccionDisponible.Habilidad, acciones);
    }

    [Fact]
    public void AccionesDisponibles_ConHabilidadEnRecarga_NoOfreceHabilidad()
    {
        var actor = FabricaDePersonajesDePrueba.Crear(tipo: TipoPersonaje.Colosal, fuerza: 10);
        actor.TurnosRecargaHabilidad = 2;
        var habilidad = new HabilidadColosal();

        var acciones = MotorDeCombate.DeterminarAccionesDisponibles(actor, habilidad);

        Assert.DoesNotContain(AccionDisponible.Habilidad, acciones);
    }

    [Fact]
    public void AccionesDisponibles_ConFuriaCompleta_OfreceUltimate()
    {
        var actor = FabricaDePersonajesDePrueba.Crear(tipo: TipoPersonaje.Colosal, fuerza: 10);
        actor.GanarFuria(Personaje.FuriaMaxima);
        var habilidad = new HabilidadColosal();

        var acciones = MotorDeCombate.DeterminarAccionesDisponibles(actor, habilidad);

        Assert.Contains(AccionDisponible.Ultimate, acciones);
    }

    [Fact]
    public void Combatir_CuandoElJugadorSeRinde_ElRivalGanaYSeMarcaPorRendicion()
    {
        var jugador = FabricaDePersonajesDePrueba.Crear(
            tipo: TipoPersonaje.Colosal, nombre: "Cobarde", fuerza: 30, nivel: 5);
        var rival = FabricaDePersonajesDePrueba.Crear(
            tipo: TipoPersonaje.Ataque, nombre: "Verdugo", fuerza: 5, nivel: 1);
        var motor = CrearMotor(new RandomSecuencia(Enumerable.Repeat(0.99d, 100)));
        var observador = new ObservadorNulo();

        var resultado = motor.Combatir(
            jugador, rival, new SelectorFijo(AccionDisponible.Rendirse), new SelectorFijo(AccionDisponible.Atacar), observador);

        Assert.True(resultado.PorRendicion);
        Assert.Same(rival, resultado.Ganador);
        Assert.Same(jugador, resultado.Perdedor);
        Assert.Contains(observador.Eventos, e => e.Tipo == TipoEvento.Rendicion);
    }

    [Fact]
    public void AccionesDisponibles_NuncaIncluyeRendirse()
    {
        var actor = FabricaDePersonajesDePrueba.Crear(tipo: TipoPersonaje.Colosal, fuerza: 10);
        var habilidad = new HabilidadColosal();

        var acciones = MotorDeCombate.DeterminarAccionesDisponibles(actor, habilidad);

        Assert.DoesNotContain(AccionDisponible.Rendirse, acciones);
    }

    private static MotorDeCombate CrearMotor(IRandomProvider random) => new(
        new CalculadoraDano(),
        random,
        new ProveedorHabilidades(
        [
            new HabilidadColosal(),
            new HabilidadAcorazado(),
            new HabilidadBestia(),
            new HabilidadAtaque(),
            new HabilidadUltimateColosal(),
            new HabilidadUltimateAcorazado(),
            new HabilidadUltimateBestia(),
            new HabilidadUltimateAtaque(),
        ]));
}