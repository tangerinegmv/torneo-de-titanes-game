using System.Text.Json;
using Microsoft.Extensions.Options;
using TorneoDeTitanes.Core.Abstracciones;
using TorneoDeTitanes.Core.Modelos;

namespace TorneoDeTitanes.Console.Infraestructura;

public sealed class JsonPersonajeRepositorio : IPersonajeRepositorio
{
    private static readonly JsonSerializerOptions OpcionesJson = new()
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true,
    };

    private readonly string _ruta;

    public JsonPersonajeRepositorio(IOptions<OpcionesJuego> opciones)
    {
        _ruta = RutasDeArchivo.Resolver(opciones.Value.RutaPersonajes);
    }

    public bool Existe() => File.Exists(_ruta);

    public List<Personaje> Leer()
    {
        if (!Existe())
        {
            return new List<Personaje>();
        }

        string json = File.ReadAllText(_ruta);
        return JsonSerializer.Deserialize<List<Personaje>>(json, OpcionesJson) ?? new List<Personaje>();
    }

    public void Guardar(IEnumerable<Personaje> personajes)
    {
        AsegurarDirectorio();
        string json = JsonSerializer.Serialize(personajes.ToList(), OpcionesJson);
        File.WriteAllText(_ruta, json);
    }

    public void Eliminar()
    {
        if (Existe())
        {
            File.Delete(_ruta);
        }
    }

    private void AsegurarDirectorio()
    {
        string? directorio = Path.GetDirectoryName(_ruta);
        if (!string.IsNullOrEmpty(directorio))
        {
            Directory.CreateDirectory(directorio);
        }
    }
}
