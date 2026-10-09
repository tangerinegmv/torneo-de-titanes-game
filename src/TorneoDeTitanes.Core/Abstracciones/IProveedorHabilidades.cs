using TorneoDeTitanes.Core.Habilidades;
using TorneoDeTitanes.Core.Modelos;

namespace TorneoDeTitanes.Core.Abstracciones;

public interface IProveedorHabilidades
{
    IHabilidad Obtener(TipoPersonaje tipo, NivelHabilidad nivel = NivelHabilidad.Especial);
}
