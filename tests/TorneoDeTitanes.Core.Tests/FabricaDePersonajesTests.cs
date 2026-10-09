using System.Text.Json;
using TorneoDeTitanes.Core.Integracion;
using TorneoDeTitanes.Core.Modelos;
using TorneoDeTitanes.Core.Personajes;
using TorneoDeTitanes.Core.Tests.Dobles;

namespace TorneoDeTitanes.Core.Tests;

public sealed class FabricaDePersonajesTests
{
    private readonly FabricaDePersonajes _fabrica = new(new RandomSecuencia(Array.Empty<double>(), Array.Empty<int>()));

    [Fact]
    public void Crear_AsignaEstadisticasMinimasParaElTipo()
    {
        var personaje = _fabrica.Crear(CrearDto("Eren", "Male", 19));

        Assert.Equal(TipoPersonaje.Colosal, personaje.Tipo);
        Assert.Equal(5, personaje.Fuerza);
        Assert.Equal(3, personaje.Velocidad);
        Assert.Equal(2, personaje.Destreza);
        Assert.Equal(2, personaje.Armadura);
        Assert.Equal(1, personaje.Nivel);
        Assert.Equal("Hombre", personaje.Genero);
        Assert.Equal(Personaje.SaludInicial, personaje.Salud);
    }

    [Fact]
    public void Crear_MapeaGeneroDesconocido_CuandoElValorNoSeReconoce()
    {
        var personaje = _fabrica.Crear(CrearDto("Mystery", "non-binary", 25));

        Assert.Equal("Desconocido", personaje.Genero);
    }

    [Fact]
    public void CrearSeleccion_DevuelveSoloLaCantidadPedida()
    {
        var dto = Enumerable.Range(1, 8).Select(i => CrearDto($"Titan {i}", "Female", 20)).ToList();

        var seleccion = _fabrica.CrearSeleccion(dto, cantidad: 3);

        Assert.Equal(3, seleccion.Count);
    }

    private static PersonajeApiDto CrearDto(string nombre, string genero, int edad)
    {
        using var documento = JsonDocument.Parse(edad.ToString());
        return new PersonajeApiDto
        {
            Name = nombre,
            Gender = genero,
            Age = documento.RootElement.Clone(),
        };
    }
}