namespace TorneoDeTitanes.Core.Modelos;

public enum EstadoAlterado
{
    Sangrado,
    Veneno,
    Quemadura,
    Regeneracion,
    Aturdido,
    Fortalecido,
    Debilitado,
}

public sealed class EfectoEstado
{
    public EstadoAlterado Tipo { get; init; }
    public int Duracion { get; set; }
    public int Potencia { get; init; }
    public string Fuente { get; init; } = string.Empty;

    public EfectoEstado Clonar() => new()
    {
        Tipo = Tipo,
        Duracion = Duracion,
        Potencia = Potencia,
        Fuente = Fuente,
    };
}
