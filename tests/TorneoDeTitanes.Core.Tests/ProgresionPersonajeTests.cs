using TorneoDeTitanes.Core.Combate;
using TorneoDeTitanes.Core.Modelos;
using TorneoDeTitanes.Core.Tests.Dobles;

namespace TorneoDeTitanes.Core.Tests;

public sealed class ProgresionPersonajeTests
{
    [Fact]
    public void Mejorar_SubeElNivelHastaElMaximo()
    {
        var personaje = FabricaDePersonajesDePrueba.Crear(tipo: TipoPersonaje.Colosal, nivel: 4);
        var random = new RandomSecuencia(Array.Empty<double>(), new[] { 1 });

        ProgresionPersonaje.Mejorar(personaje, random);

        Assert.Equal(5, personaje.Nivel);
    }

    [Fact]
    public void Mejorar_NoSuperaElNivelMaximo()
    {
        var personaje = FabricaDePersonajesDePrueba.Crear(tipo: TipoPersonaje.Colosal, nivel: 5);
        var random = new RandomSecuencia(Array.Empty<double>(), new[] { 1 });

        ProgresionPersonaje.Mejorar(personaje, random);

        Assert.Equal(5, personaje.Nivel);
    }

    [Fact]
    public void Mejorar_ColosalIncrementaFuerzaYArmadura()
    {
        var personaje = FabricaDePersonajesDePrueba.Crear(tipo: TipoPersonaje.Colosal, fuerza: 5, armadura: 2, nivel: 1);
        var random = new RandomSecuencia(Array.Empty<double>(), new[] { 2, 3 });

        ProgresionPersonaje.Mejorar(personaje, random);

        Assert.True(personaje.Fuerza >= 6);
        Assert.True(personaje.Armadura >= 4);
    }

    [Fact]
    public void Mejorar_AcorazadoIncrementaSuArmaduraSinSuperarElTope()
    {
        var personaje = FabricaDePersonajesDePrueba.Crear(tipo: TipoPersonaje.Acorazado, armadura: 9, nivel: 1);
        var random = new RandomSecuencia(Array.Empty<double>(), new[] { 4 });

        ProgresionPersonaje.Mejorar(personaje, random);

        Assert.Equal(10, personaje.Armadura);
    }

    [Fact]
    public void Penalizar_ReduceEstadisticasYElNivel()
    {
        var personaje = FabricaDePersonajesDePrueba.Crear(
            tipo: TipoPersonaje.Colosal, fuerza: 5, destreza: 5, velocidad: 5, armadura: 5, nivel: 3);

        ProgresionPersonaje.Penalizar(personaje);

        Assert.Equal(4, personaje.Fuerza);
        Assert.Equal(4, personaje.Destreza);
        Assert.Equal(4, personaje.Velocidad);
        Assert.Equal(4, personaje.Armadura);
        Assert.Equal(2, personaje.Nivel);
    }

    [Fact]
    public void Penalizar_NuncaBajaLasEstadisticasPorDebajoDeUno()
    {
        var personaje = FabricaDePersonajesDePrueba.Crear(
            tipo: TipoPersonaje.Colosal, fuerza: 1, destreza: 1, velocidad: 1, armadura: 1, nivel: 1);

        ProgresionPersonaje.Penalizar(personaje);

        Assert.Equal(1, personaje.Fuerza);
        Assert.Equal(1, personaje.Destreza);
        Assert.Equal(1, personaje.Velocidad);
        Assert.Equal(1, personaje.Armadura);
        Assert.Equal(1, personaje.Nivel);
    }
}