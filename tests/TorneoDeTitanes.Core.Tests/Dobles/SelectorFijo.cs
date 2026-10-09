using TorneoDeTitanes.Core.Abstracciones;
using TorneoDeTitanes.Core.Combate;

namespace TorneoDeTitanes.Core.Tests.Dobles;

public sealed class SelectorFijo : ISelectorAccion
{
    private readonly Func<ContextoTurno, AccionDisponible> _selector;

    public SelectorFijo(AccionDisponible accion)
        : this(_ => accion)
    {
    }

    public SelectorFijo(Func<ContextoTurno, AccionDisponible> selector)
    {
        _selector = selector;
    }

    public AccionDisponible ElegirAccion(ContextoTurno contexto) => _selector(contexto);
}