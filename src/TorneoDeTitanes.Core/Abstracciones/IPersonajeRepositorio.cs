using TorneoDeTitanes.Core.Modelos;

namespace TorneoDeTitanes.Core.Abstracciones;

public interface IPersonajeRepositorio
{
    bool Existe();

    List<Personaje> Leer();

    void Guardar(IEnumerable<Personaje> personajes);

    void Eliminar();
}
