namespace TorneoDeTitanes.Console.Infraestructura;

public sealed class OpcionesJuego
{
    public const string Seccion = "Juego";

    public string RutaPersonajes { get; set; } = "Data/Personajes.json";

    public string RutaHistorial { get; set; } = "Data/Historial.json";

    public string ApiUrl { get; set; } =
        "https://api.attackontitanapi.com/characters/1,2,3,4,5,8,10,12,57,66,67,74,86,87,88,89,90,91,95,101,184,193,188";

    public int CantidadPersonajes { get; set; } = 10;

    public int RondasTorneo { get; set; } = 9;

    public int MaximoHistorial { get; set; } = 15;

    public int VelocidadAnimacionMs { get; set; } = 700;
}
