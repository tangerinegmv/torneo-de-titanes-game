using TorneoDeTitanes.Core.Modelos;

namespace TorneoDeTitanes.Core.Abstracciones;

public interface IHistorialRepositorio
{
    IReadOnlyList<HistorialRegistro> Leer();

    void RegistrarGanador(Personaje ganador, DateTime? fecha = null);
}
