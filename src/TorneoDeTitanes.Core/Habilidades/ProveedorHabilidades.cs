using TorneoDeTitanes.Core.Abstracciones;
using TorneoDeTitanes.Core.Modelos;

namespace TorneoDeTitanes.Core.Habilidades;

public sealed class ProveedorHabilidades : IProveedorHabilidades
{
    private readonly Dictionary<(TipoPersonaje Tipo, NivelHabilidad Nivel), IHabilidad> _habilidades;

    public ProveedorHabilidades(IEnumerable<IHabilidad> habilidades)
    {
        _habilidades = habilidades.ToDictionary(h => (h.Tipo, h.Nivel));
    }

    public IHabilidad Obtener(TipoPersonaje tipo, NivelHabilidad nivel = NivelHabilidad.Especial)
    {
        if (_habilidades.TryGetValue((tipo, nivel), out var habilidad))
        {
            return habilidad;
        }

        throw new InvalidOperationException(
            $"No existe una habilidad de nivel {nivel} registrada para el tipo {tipo}.");
    }
}
