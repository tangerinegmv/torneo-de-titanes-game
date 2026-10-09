using TorneoDeTitanes.Core.Habilidades;
using TorneoDeTitanes.Core.Modelos;

namespace TorneoDeTitanes.Core.Tests;

public sealed class ProveedorHabilidadesTests
{
    private readonly ProveedorHabilidades _proveedor = CrearProveedor();

    [Theory]
    [InlineData(TipoPersonaje.Colosal, "Pisotón Sísmico")]
    [InlineData(TipoPersonaje.Acorazado, "Muro de Acero")]
    [InlineData(TipoPersonaje.Bestia, "Rugido Bestial")]
    [InlineData(TipoPersonaje.Ataque, "Asalto Relámpago")]
    public void Obtener_DevuelveLaHabilidadEspecialDeCadaTitan(TipoPersonaje tipo, string nombreEsperado)
    {
        var habilidad = _proveedor.Obtener(tipo);

        Assert.Equal(NivelHabilidad.Especial, habilidad.Nivel);
        Assert.Equal(nombreEsperado, habilidad.Nombre);
    }

    [Theory]
    [InlineData(TipoPersonaje.Colosal, "Detonación Apocalíptica")]
    [InlineData(TipoPersonaje.Acorazado, "Fortaleza Impenetrable")]
    [InlineData(TipoPersonaje.Bestia, "Juicio de las Bestias")]
    [InlineData(TipoPersonaje.Ataque, "Filo del Titán de Ataque")]
    public void Obtener_DevuelveElUltimateDeCadaTitan(TipoPersonaje tipo, string nombreEsperado)
    {
        var habilidad = _proveedor.Obtener(tipo, NivelHabilidad.Ultimate);

        Assert.Equal(NivelHabilidad.Ultimate, habilidad.Nivel);
        Assert.Equal(nombreEsperado, habilidad.Nombre);
    }

    [Fact]
    public void Obtener_ConNivelNoRegistrado_LanzaExcepcion()
    {
        Assert.Throws<InvalidOperationException>(() => _proveedor.Obtener(TipoPersonaje.Colosal, (NivelHabilidad)99));
    }

    private static ProveedorHabilidades CrearProveedor() => new(
    [
        new HabilidadColosal(),
        new HabilidadAcorazado(),
        new HabilidadBestia(),
        new HabilidadAtaque(),
        new HabilidadUltimateColosal(),
        new HabilidadUltimateAcorazado(),
        new HabilidadUltimateBestia(),
        new HabilidadUltimateAtaque(),
    ]);
}