using System.Text.Json;
using TorneoDeTitanes.Core.Abstracciones;
using TorneoDeTitanes.Core.Integracion;
using TorneoDeTitanes.Core.Modelos;

namespace TorneoDeTitanes.Core.Personajes;

public sealed class FabricaDePersonajes
{
    private readonly IRandomProvider _random;

    public FabricaDePersonajes(IRandomProvider random)
    {
        _random = random;
    }

    public Personaje Crear(PersonajeApiDto datos)
    {
        int edad = ObtenerEdad(datos.Age);
        var personaje = new Personaje
        {
            Nombre = datos.Name,
            Tipo = ElegirTipo(),
            Edad = edad,
            FecNac = GenerarFechaNacimiento(),
            Genero = MapearGenero(datos.Gender),
            SaludMaxima = Personaje.SaludInicial,
            Salud = Personaje.SaludInicial,
        };

        AsignarEstadisticas(personaje);
        return personaje;
    }

    public List<Personaje> CrearSeleccion(IEnumerable<PersonajeApiDto> personajes, int cantidad)
    {
        return personajes
            .OrderBy(_ => _random.Siguiente(int.MinValue, int.MaxValue))
            .Take(cantidad)
            .Select(Crear)
            .ToList();
    }

    private TipoPersonaje ElegirTipo()
    {
        var valores = Enum.GetValues<TipoPersonaje>();
        return valores[_random.Siguiente(0, valores.Length)];
    }

    private DateTime GenerarFechaNacimiento()
    {
        var inicio = new DateTime(700, 1, 1);
        var fin = new DateTime(705, 12, 31);
        int rangoDias = (fin - inicio).Days;
        return inicio.AddDays(_random.Siguiente(0, rangoDias + 1));
    }

    private static string MapearGenero(string genero)
    {
        if (string.IsNullOrWhiteSpace(genero))
        {
            return "Desconocido";
        }

        return genero.Trim().ToLowerInvariant() switch
        {
            "male" => "Hombre",
            "female" => "Mujer",
            _ => "Desconocido",
        };
    }

    private int ObtenerEdad(JsonElement edad)
    {
        switch (edad.ValueKind)
        {
            case JsonValueKind.Number when edad.TryGetInt32(out int numero):
                return numero;

            case JsonValueKind.String:
                string? texto = edad.GetString();
                if (!string.IsNullOrWhiteSpace(texto)
                    && !texto.Equals("unknown", StringComparison.OrdinalIgnoreCase)
                    && int.TryParse(texto, out int parseada))
                {
                    return parseada;
                }

                break;
        }

        return _random.Siguiente(19, 36);
    }

    private void AsignarEstadisticas(Personaje personaje)
    {
        switch (personaje.Tipo)
        {
            case TipoPersonaje.Colosal:
                personaje.Fuerza = _random.Siguiente(5, 8);
                personaje.Velocidad = _random.Siguiente(3, 7);
                personaje.Destreza = _random.Siguiente(2, 5);
                personaje.Nivel = _random.Siguiente(1, 6);
                personaje.Armadura = _random.Siguiente(2, 4);
                break;

            case TipoPersonaje.Acorazado:
                personaje.Fuerza = _random.Siguiente(6, 9);
                personaje.Velocidad = _random.Siguiente(4, 7);
                personaje.Destreza = _random.Siguiente(2, 5);
                personaje.Nivel = _random.Siguiente(1, 6);
                personaje.Armadura = _random.Siguiente(4, 6);
                break;

            case TipoPersonaje.Ataque:
                personaje.Fuerza = _random.Siguiente(6, 10);
                personaje.Velocidad = _random.Siguiente(5, 8);
                personaje.Destreza = _random.Siguiente(3, 6);
                personaje.Nivel = _random.Siguiente(1, 6);
                personaje.Armadura = _random.Siguiente(2, 4);
                break;

            case TipoPersonaje.Bestia:
                personaje.Fuerza = _random.Siguiente(5, 8);
                personaje.Velocidad = _random.Siguiente(4, 8);
                personaje.Destreza = _random.Siguiente(2, 5);
                personaje.Nivel = _random.Siguiente(1, 6);
                personaje.Armadura = _random.Siguiente(5, 8);
                break;
        }
    }
}
