namespace TorneoDeTitanes.Core.Combate;

public enum TipoEvento
{
    Informacion,
    Ataque,
    Critico,
    Esquiva,
    Bloqueo,
    Habilidad,
    Estado,
    Curacion,
    Mejora,
    Muerte,
    Rendicion,
}

public readonly record struct EventoCombate(string Texto, TipoEvento Tipo = TipoEvento.Informacion);
