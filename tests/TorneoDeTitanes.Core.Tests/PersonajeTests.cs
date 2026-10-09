using TorneoDeTitanes.Core.Modelos;
using TorneoDeTitanes.Core.Tests.Dobles;

namespace TorneoDeTitanes.Core.Tests;

public sealed class PersonajeTests
{
    [Fact]
    public void RecibirDano_ConsumeElEscudoAntesQueLaSalud()
    {
        var personaje = FabricaDePersonajesDePrueba.Crear(saludMaxima: 100);
        personaje.OtorgarEscudo(30);

        personaje.RecibirDano(50);

        Assert.Equal(0, personaje.Escudo);
        Assert.Equal(80, personaje.Salud);
        Assert.True(personaje.EstaVivo);
    }

    [Fact]
    public void RecibirDano_NuncaDejaLaSaludNegativa()
    {
        var personaje = FabricaDePersonajesDePrueba.Crear(saludMaxima: 10);

        personaje.RecibirDano(500);

        Assert.Equal(0, personaje.Salud);
        Assert.False(personaje.EstaVivo);
    }

    [Fact]
    public void GanarFuria_LimitaAlMaximoYDesbloqueaUltimate()
    {
        var personaje = FabricaDePersonajesDePrueba.Crear();

        personaje.GanarFuria(Personaje.FuriaMaxima + 80);

        Assert.Equal(Personaje.FuriaMaxima, personaje.Furia);
        Assert.True(personaje.PuedeDesatarUltimate);
    }

    [Fact]
    public void Curar_NuncaSuperaLaSaludMaxima()
    {
        var personaje = FabricaDePersonajesDePrueba.Crear(saludMaxima: 100);
        personaje.RecibirDano(60);

        personaje.Curar(500);

        Assert.Equal(100, personaje.Salud);
    }

    [Fact]
    public void Restaurar_ReiniciaEstadoDeCombate()
    {
        var personaje = FabricaDePersonajesDePrueba.Crear(saludMaxima: 80);
        personaje.RecibirDano(30);
        personaje.OtorgarEscudo(15);
        personaje.GanarFuria(70);
        personaje.AgregarEstado(new EfectoEstado { Tipo = EstadoAlterado.Veneno, Duracion = 3, Potencia = 5, Fuente = "Test" });

        personaje.Restaurar();

        Assert.Equal(80, personaje.Salud);
        Assert.Equal(0, personaje.Escudo);
        Assert.Equal(0, personaje.Furia);
        Assert.Empty(personaje.Estados);
    }

    [Fact]
    public void AgregarEstado_RefrescaLaDuracionDelEstadoExistente()
    {
        var personaje = FabricaDePersonajesDePrueba.Crear();
        personaje.AgregarEstado(new EfectoEstado { Tipo = EstadoAlterado.Sangrado, Duracion = 2, Potencia = 5, Fuente = "A" });
        personaje.AgregarEstado(new EfectoEstado { Tipo = EstadoAlterado.Sangrado, Duracion = 4, Potencia = 5, Fuente = "B" });

        Assert.Single(personaje.Estados);
        Assert.Equal(4, personaje.Estados[0].Duracion);
    }
}