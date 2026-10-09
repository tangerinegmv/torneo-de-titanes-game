using TorneoDeTitanes.Core.Abstracciones;
using TorneoDeTitanes.Core.Habilidades;
using TorneoDeTitanes.Core.Modelos;

namespace TorneoDeTitanes.Core.Combate;

public enum AccionDisponible
{
    Atacar,
    Habilidad,
    Ultimate,
    Defender,
    Rendirse,
}

public sealed class ContextoTurno
{
    public required Personaje Actor { get; init; }
    public required Personaje Oponente { get; init; }
    public required int NumeroTurno { get; init; }
    public required IReadOnlyList<AccionDisponible> AccionesDisponibles { get; init; }
    public required IHabilidad HabilidadEspecial { get; init; }
    public required IHabilidad HabilidadUltimate { get; init; }
}

public sealed class ContextoAccion
{
    public required Personaje Atacante { get; init; }
    public required Personaje Defensor { get; init; }
    public required IRandomProvider Random { get; init; }
    public required ICalculadoraDano CalculadoraDano { get; init; }
}

public sealed class ResultadoAccion
{
    public List<EventoCombate> Eventos { get; } = new();

    public int DanoInfligido { get; set; }

    public bool EsRendicion { get; set; }

    public void Agregar(string texto, TipoEvento tipo = TipoEvento.Informacion)
        => Eventos.Add(new EventoCombate(texto, tipo));
}

public sealed record ResultadoCombate(Personaje Ganador, Personaje Perdedor, int Turnos, bool PorRendicion = false);
