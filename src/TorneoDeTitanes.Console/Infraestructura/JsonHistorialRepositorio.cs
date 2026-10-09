using System.Text.Json;
using Microsoft.Extensions.Options;
using TorneoDeTitanes.Core.Abstracciones;
using TorneoDeTitanes.Core.Modelos;

namespace TorneoDeTitanes.Console.Infraestructura;

public sealed class JsonHistorialRepositorio : IHistorialRepositorio
{
    private static readonly JsonSerializerOptions OpcionesJson = new()
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true,
    };

    private readonly string _ruta;
    private readonly int _maximoRegistros;

    public JsonHistorialRepositorio(IOptions<OpcionesJuego> opciones)
    {
        _ruta = RutasDeArchivo.Resolver(opciones.Value.RutaHistorial);
        _maximoRegistros = opciones.Value.MaximoHistorial;
    }

    public IReadOnlyList<HistorialRegistro> Leer()
    {
        if (!File.Exists(_ruta))
        {
            return Array.Empty<HistorialRegistro>();
        }

        string json = File.ReadAllText(_ruta);
        return JsonSerializer.Deserialize<List<HistorialRegistro>>(json, OpcionesJson)
               ?? new List<HistorialRegistro>();
    }

    public void RegistrarGanador(Personaje ganador, DateTime? fecha = null)
    {
        var historial = Leer().ToList();

        DateTime momento = fecha ?? DateTime.Now;
        var fechaAjustada = new DateTime(724, momento.Month, momento.Day, momento.Hour, momento.Minute, momento.Second);

        historial.Add(new HistorialRegistro { Ganador = ganador, Hora = fechaAjustada });

        while (historial.Count > _maximoRegistros)
        {
            historial.RemoveAt(0);
        }

        AsegurarDirectorio();
        string json = JsonSerializer.Serialize(historial, OpcionesJson);
        File.WriteAllText(_ruta, json);
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
