using TorneoDeTitanes.Core.Combate;

namespace TorneoDeTitanes.Core.Abstracciones;

public interface ISelectorAccion
{
    AccionDisponible ElegirAccion(ContextoTurno contexto);
}
