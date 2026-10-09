using System.Text.Json;
using System.Globalization;
using TorneoDeTitanes.Core.Integracion;

namespace TorneoDeTitanes.Console.Infraestructura;

public static class PersonajesDeRespaldo
{
    public static IReadOnlyList<PersonajeApiDto> Obtener()
    {
        return new List<PersonajeApiDto>
        {
            Crear("Eren Yeager", "Male", 19),
            Crear("Mikasa Ackerman", "Female", 19),
            Crear("Armin Arlelt", "Male", 19),
            Crear("Levi Ackerman", "Male", 34),
            Crear("Erwin Smith", "Male", 39),
            Crear("Hange Zoe", "Female", 33),
            Crear("Reiner Braun", "Male", 21),
            Crear("Annie Leonhart", "Female", 20),
            Crear("Bertholdt Hoover", "Male", 20),
            Crear("Zeke Yeager", "Male", 29),
            Crear("Pieck Finger", "Female", 24),
            Crear("Porco Galliard", "Male", 20),
        };
    }

    private static PersonajeApiDto Crear(string nombre, string genero, int edad)
    {
        using var documento = JsonDocument.Parse(edad.ToString(CultureInfo.InvariantCulture));
        return new PersonajeApiDto
        {
            Name = nombre,
            Gender = genero,
            Age = documento.RootElement.Clone(),
        };
    }
}
