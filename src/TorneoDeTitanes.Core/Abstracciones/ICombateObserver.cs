using TorneoDeTitanes.Core.Combate;
using TorneoDeTitanes.Core.Modelos;

namespace TorneoDeTitanes.Core.Abstracciones;

public interface ICombateObserver
{
    void AlIniciarCombate(Personaje jugador, Personaje rival);

    void AlActualizarEstado(Personaje jugador, Personaje rival, int numeroTurno, bool esTurnoDelJugador);

    void AlMostrarEvento(EventoCombate evento);

    void AlFinalizarCombate(Personaje ganador);
}
