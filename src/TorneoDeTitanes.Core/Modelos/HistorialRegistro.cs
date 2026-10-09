namespace TorneoDeTitanes.Core.Modelos;

public sealed class HistorialRegistro
{
    public Personaje Ganador { get; set; } = new();
    public DateTime Hora { get; set; }
}
