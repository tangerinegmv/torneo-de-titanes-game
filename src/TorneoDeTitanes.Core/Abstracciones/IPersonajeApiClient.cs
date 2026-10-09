using TorneoDeTitanes.Core.Integracion;

namespace TorneoDeTitanes.Core.Abstracciones;

public interface IPersonajeApiClient
{
    Task<IReadOnlyList<PersonajeApiDto>> ObtenerPersonajesAsync(CancellationToken cancelacion = default);
}
