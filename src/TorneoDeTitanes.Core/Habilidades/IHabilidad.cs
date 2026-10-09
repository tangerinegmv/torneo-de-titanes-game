using TorneoDeTitanes.Core.Combate;
using TorneoDeTitanes.Core.Modelos;

namespace TorneoDeTitanes.Core.Habilidades;

public enum NivelHabilidad
{
    Especial,
    Ultimate,
}

public interface IHabilidad
{
    string Nombre { get; }

    string Descripcion { get; }

    int CostoEnergia { get; }

    int TurnosRecarga { get; }

    TipoPersonaje Tipo { get; }

    NivelHabilidad Nivel { get; }

    void Ejecutar(ContextoAccion contexto, ResultadoAccion resultado);
}
