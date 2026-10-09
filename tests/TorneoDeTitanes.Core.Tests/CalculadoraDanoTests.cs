using TorneoDeTitanes.Core.Combate;
using TorneoDeTitanes.Core.Modelos;
using TorneoDeTitanes.Core.Tests.Dobles;

namespace TorneoDeTitanes.Core.Tests;

public sealed class CalculadoraDanoTests
{
    private readonly CalculadoraDano _calculadora = new();

    [Fact]
    public void Calcular_CuandoElDefensorEsMuchoMasRapido_PuedeEsquivar()
    {
        var atacante = FabricaDePersonajesDePrueba.Crear(velocidad: 5);
        var defensor = FabricaDePersonajesDePrueba.Crear(velocidad: 50);
        var random = new RandomSecuencia(new[] { 0.0 });

        var resultado = _calculadora.Calcular(atacante, defensor, defensorEstaDefendiendo: false, random);

        Assert.True(resultado.Esquivado);
        Assert.Equal(0, resultado.Dano);
    }

    [Fact]
    public void Calcular_SinEsquivaNiCritico_DevuelveDanoPredecible()
    {
        var atacante = FabricaDePersonajesDePrueba.Crear(fuerza: 10, destreza: 5, nivel: 5);
        var defensor = FabricaDePersonajesDePrueba.Crear(armadura: 5, velocidad: 5);
        var random = new RandomSecuencia(new[] { 0.99, 0.99, 0.99 });

        var resultado = _calculadora.Calcular(atacante, defensor, defensorEstaDefendiendo: false, random);

        Assert.False(resultado.Esquivado);
        Assert.False(resultado.Critico);
        Assert.Equal(67, resultado.Dano);
    }

    [Fact]
    public void Calcular_GolpeCritico_InfligeMasDanoQueUnoNormal()
    {
        var atacante = FabricaDePersonajesDePrueba.Crear(fuerza: 10, destreza: 5, nivel: 5);
        var defensor = FabricaDePersonajesDePrueba.Crear(armadura: 5, velocidad: 5);

        var conCritico = _calculadora.Calcular(
            atacante, defensor, false, new RandomSecuencia(new[] { 0.99, 0.5, 0.0 }));
        var sinCritico = _calculadora.Calcular(
            atacante, defensor, false, new RandomSecuencia(new[] { 0.99, 0.5, 0.5 }));

        Assert.True(conCritico.Critico);
        Assert.False(sinCritico.Critico);
        Assert.True(conCritico.Dano > sinCritico.Dano);
        Assert.Equal(102, conCritico.Dano);
        Assert.Equal(58, sinCritico.Dano);
    }

    [Fact]
    public void Calcular_DefensorFortalecido_ReduceElDano()
    {
        var atacante = FabricaDePersonajesDePrueba.Crear(fuerza: 10, destreza: 5, nivel: 5);
        var defensorDebil = FabricaDePersonajesDePrueba.Crear(armadura: 5, velocidad: 5);
        var defensorFuerte = FabricaDePersonajesDePrueba.Crear(armadura: 5, velocidad: 5);
        defensorFuerte.AgregarEstado(new EfectoEstado
        {
            Tipo = EstadoAlterado.Fortalecido,
            Duracion = 1,
            Potencia = 1,
            Fuente = "Test",
        });

        var sinFortalecer = _calculadora.Calcular(
            atacante, defensorDebil, false, new RandomSecuencia(new[] { 0.99, 0.5, 0.5 }));
        var fortalecido = _calculadora.Calcular(
            atacante, defensorFuerte, false, new RandomSecuencia(new[] { 0.99, 0.5, 0.5 }));

        Assert.True(fortalecido.Dano < sinFortalecer.Dano);
    }

    [Fact]
    public void Calcular_AtacanteDebilitado_CausaMenosDano()
    {
        var atacanteDebilitado = FabricaDePersonajesDePrueba.Crear(fuerza: 10, destreza: 5, nivel: 5);
        atacanteDebilitado.AgregarEstado(new EfectoEstado
        {
            Tipo = EstadoAlterado.Debilitado,
            Duracion = 2,
            Potencia = 1,
            Fuente = "Test",
        });
        var atacanteNormal = FabricaDePersonajesDePrueba.Crear(fuerza: 10, destreza: 5, nivel: 5);
        var defensor = FabricaDePersonajesDePrueba.Crear(armadura: 5, velocidad: 5);

        var normal = _calculadora.Calcular(
            atacanteNormal, defensor, false, new RandomSecuencia(new[] { 0.99, 0.5, 0.5 }));
        var debilitado = _calculadora.Calcular(
            atacanteDebilitado, defensor, false, new RandomSecuencia(new[] { 0.99, 0.5, 0.5 }));

        Assert.True(debilitado.Dano < normal.Dano);
    }

    [Fact]
    public void Calcular_DefendiendoActivamente_ReduceElDano()
    {
        var atacante = FabricaDePersonajesDePrueba.Crear(fuerza: 10, destreza: 5, nivel: 5);
        var defensor = FabricaDePersonajesDePrueba.Crear(armadura: 5, velocidad: 5);

        var atacando = _calculadora.Calcular(
            atacante, defensor, defensorEstaDefendiendo: false, new RandomSecuencia(new[] { 0.99, 0.5, 0.5 }));
        var defendiendo = _calculadora.Calcular(
            atacante, defensor, defensorEstaDefendiendo: true, new RandomSecuencia(new[] { 0.99, 0.5, 0.5 }));

        Assert.True(defendiendo.Dano < atacando.Dano);
        Assert.True(defendiendo.Bloqueado);
    }

    [Fact]
    public void Calcular_ElDanoNuncaEsCero()
    {
        var atacante = FabricaDePersonajesDePrueba.Crear(fuerza: 1, destreza: 1, nivel: 1);
        var defensor = FabricaDePersonajesDePrueba.Crear(armadura: 10, velocidad: 15);
        var random = new RandomSecuencia(Enumerable.Repeat(0.99d, 3));

        var resultado = _calculadora.Calcular(atacante, defensor, false, random);

        Assert.False(resultado.Esquivado);
        Assert.True(resultado.Dano >= 1);
    }
}