using System.Text.Json.Serialization;

namespace TorneoDeTitanes.Core.Modelos;

public enum TipoPersonaje
{
    Colosal = 0,
    Acorazado = 1,
    Bestia = 2,
    Ataque = 3,
}

public class Personaje
{
    public const int SaludInicial = 100;
    public const int EnergiaInicial = 100;
    public const int FuriaMaxima = 100;

    public int Velocidad { get; set; }
    public int Destreza { get; set; }
    public int Fuerza { get; set; }
    public int Nivel { get; set; }
    public int Armadura { get; set; }
    public int Salud { get; set; } = SaludInicial;
    public string Nombre { get; set; } = string.Empty;
    public string Genero { get; set; } = string.Empty;
    public DateTime FecNac { get; set; }
    public int Edad { get; set; }
    public TipoPersonaje Tipo { get; set; }

    public int SaludMaxima { get; set; } = SaludInicial;
    public int EnergiaMaxima { get; set; } = EnergiaInicial;
    public int Energia { get; set; } = EnergiaInicial;

    [JsonIgnore]
    public List<EfectoEstado> Estados { get; } = new();

    [JsonIgnore]
    public int Furia { get; set; }

    [JsonIgnore]
    public int Escudo { get; set; }

    [JsonIgnore]
    public int TurnosRecargaHabilidad { get; set; }

    [JsonIgnore]
    public bool EstaVivo => Salud > 0;

    [JsonIgnore]
    public bool PuedeDesatarUltimate => Furia >= FuriaMaxima;

    [JsonIgnore]
    public double PorcentajeSalud => SaludMaxima <= 0 ? 0d : Math.Clamp((double)Salud / SaludMaxima, 0d, 1d);

    [JsonIgnore]
    public double PorcentajeEnergia => EnergiaMaxima <= 0 ? 0d : Math.Clamp((double)Energia / EnergiaMaxima, 0d, 1d);

    [JsonIgnore]
    public double PorcentajeFuria => Math.Clamp((double)Furia / FuriaMaxima, 0d, 1d);

    public bool TieneEstado(EstadoAlterado tipo) => Estados.Any(e => e.Tipo == tipo);

    public void AgregarEstado(EfectoEstado efecto)
    {
        var existente = Estados.FirstOrDefault(e => e.Tipo == efecto.Tipo);
        if (existente is not null)
        {
            existente.Duracion = Math.Max(existente.Duracion, efecto.Duracion);
            return;
        }

        Estados.Add(efecto);
    }

    public void LimpiarEstados() => Estados.Clear();

    public void Restaurar()
    {
        Salud = SaludMaxima;
        Energia = EnergiaMaxima;
        Furia = 0;
        Escudo = 0;
        TurnosRecargaHabilidad = 0;
        LimpiarEstados();
    }

    public void OtorgarEscudo(int cantidad)
    {
        if (cantidad > 0)
        {
            Escudo += cantidad;
        }
    }

    public void GanarFuria(int cantidad)
    {
        if (cantidad != 0)
        {
            Furia = Math.Clamp(Furia + cantidad, 0, FuriaMaxima);
        }
    }

    public void RecibirDano(int cantidad)
    {
        if (cantidad <= 0)
        {
            return;
        }

        if (Escudo > 0)
        {
            int absorbido = Math.Min(Escudo, cantidad);
            Escudo -= absorbido;
            cantidad -= absorbido;
        }

        Salud = Math.Max(0, Salud - cantidad);
    }

    public void Curar(int cantidad)
    {
        if (cantidad <= 0)
        {
            return;
        }

        Salud = Math.Min(SaludMaxima, Salud + cantidad);
    }
}
