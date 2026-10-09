using TorneoDeTitanes.Core.Abstracciones;
using TorneoDeTitanes.Core.Combate;
using TorneoDeTitanes.Core.Habilidades;
using TorneoDeTitanes.Core.Modelos;
using TorneoDeTitanes.Core.Tests.Dobles;

namespace TorneoDeTitanes.Core.Tests;

public sealed class HabilidadesTests
{
    [Fact]
    public void HabilidadColosal_PuedeAturdirAlRival()
    {
        var atacante = FabricaDePersonajesDePrueba.Crear();
        var defensor = FabricaDePersonajesDePrueba.Crear(saludMaxima: 500);
        var contexto = CrearContexto(atacante, defensor, new RandomSecuencia(new[] { 0.99, 0.5, 0.5, 0.0 }));

        new HabilidadColosal().Ejecutar(contexto, new ResultadoAccion());

        Assert.True(defensor.TieneEstado(EstadoAlterado.Aturdido));
        Assert.Equal(60, atacante.Energia);
    }

    [Fact]
    public void HabilidadColosal_NoAturdeCuandoFallalaProbabilidad()
    {
        var atacante = FabricaDePersonajesDePrueba.Crear();
        var defensor = FabricaDePersonajesDePrueba.Crear(saludMaxima: 500);
        var contexto = CrearContexto(atacante, defensor, new RandomSecuencia(new[] { 0.99, 0.5, 0.5, 0.9 }));

        new HabilidadColosal().Ejecutar(contexto, new ResultadoAccion());

        Assert.False(defensor.TieneEstado(EstadoAlterado.Aturdido));
    }

    [Fact]
    public void HabilidadBestia_AplicaVenenoYDebilidad()
    {
        var atacante = FabricaDePersonajesDePrueba.Crear();
        var defensor = FabricaDePersonajesDePrueba.Crear(saludMaxima: 500);
        var contexto = CrearContexto(atacante, defensor, new RandomSecuencia(new[] { 0.99, 0.5, 0.5 }));

        new HabilidadBestia().Ejecutar(contexto, new ResultadoAccion());

        Assert.True(defensor.TieneEstado(EstadoAlterado.Veneno));
        Assert.True(defensor.TieneEstado(EstadoAlterado.Debilitado));
    }

    [Fact]
    public void HabilidadAtaque_ProvocaSangradoYGolpesMultiples()
    {
        var atacante = FabricaDePersonajesDePrueba.Crear();
        var defensor = FabricaDePersonajesDePrueba.Crear(saludMaxima: 1000);
        var resultado = new ResultadoAccion();
        var random = new RandomSecuencia(Enumerable.Repeat(0.99d, 9));

        new HabilidadAtaque().Ejecutar(CrearContexto(atacante, defensor, random), resultado);

        Assert.True(defensor.TieneEstado(EstadoAlterado.Sangrado));
        Assert.True(resultado.DanoInfligido > 0);
    }

    [Fact]
    public void UltimateColosal_InfligeDanoYCausaQuemadura()
    {
        var atacante = FabricaDePersonajesDePrueba.Crear();
        var defensor = FabricaDePersonajesDePrueba.Crear(saludMaxima: 500);
        var resultado = new ResultadoAccion();
        var contexto = CrearContexto(atacante, defensor, new RandomSecuencia(new[] { 0.99, 0.5, 0.5 }));

        new HabilidadUltimateColosal().Ejecutar(contexto, resultado);

        Assert.True(defensor.TieneEstado(EstadoAlterado.Quemadura));
        Assert.True(defensor.Salud < defensor.SaludMaxima);
    }

    [Fact]
    public void UltimateAcorazado_OtorgaEscudoYFortificacion()
    {
        var atacante = FabricaDePersonajesDePrueba.Crear();
        var defensor = FabricaDePersonajesDePrueba.Crear(saludMaxima: 500);
        var contexto = CrearContexto(atacante, defensor, new RandomSecuencia(new[] { 0.99, 0.5, 0.5 }));

        new HabilidadUltimateAcorazado().Ejecutar(contexto, new ResultadoAccion());

        Assert.True(atacante.Escudo > 0);
        Assert.True(atacante.TieneEstado(EstadoAlterado.Fortalecido));
    }

    [Fact]
    public void UltimateBestia_RegeneraSaludDelLanzador()
    {
        var atacante = FabricaDePersonajesDePrueba.Crear(saludMaxima: 100);
        atacante.RecibirDano(50);
        var defensor = FabricaDePersonajesDePrueba.Crear(saludMaxima: 500);
        var contexto = CrearContexto(atacante, defensor, new RandomSecuencia(new[] { 0.99, 0.5, 0.5 }));

        new HabilidadUltimateBestia().Ejecutar(contexto, new ResultadoAccion());

        Assert.True(atacante.Salud > 50);
        Assert.True(defensor.TieneEstado(EstadoAlterado.Veneno));
    }

    [Fact]
    public void UltimateAtaque_EjecutaVariosCortes()
    {
        var atacante = FabricaDePersonajesDePrueba.Crear();
        var defensor = FabricaDePersonajesDePrueba.Crear(saludMaxima: 2000);
        var resultado = new ResultadoAccion();
        var contexto = CrearContexto(atacante, defensor, new RandomSecuencia(Enumerable.Repeat(0.99d, 15)));

        new HabilidadUltimateAtaque().Ejecutar(contexto, resultado);

        Assert.Equal(5, resultado.Eventos.Count(e => e.Tipo == TipoEvento.Ataque));
    }

    private static ContextoAccion CrearContexto(Personaje atacante, Personaje defensor, IRandomProvider random) => new()
    {
        Atacante = atacante,
        Defensor = defensor,
        Random = random,
        CalculadoraDano = new CalculadoraDano(),
    };
}