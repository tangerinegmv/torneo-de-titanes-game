using TorneoDeTitanes.Core.Abstracciones;
using TorneoDeTitanes.Core.Combate;
using TorneoDeTitanes.Core.Modelos;

namespace TorneoDeTitanes.Core.Tests.Dobles;

public sealed class ObservadorNulo : ICombateObserver
{
    public List<EventoCombate> Eventos { get; } = new();

    public Personaje? Ganador { get; private set; }

    public void AlIniciarCombate(Personaje jugador, Personaje rival)
    {
    }

    public void AlActualizarEstado(Personaje jugador, Personaje rival, int numeroTurno, bool esTurnoDelJugador)
    {
    }

    public void AlMostrarEvento(EventoCombate evento) => Eventos.Add(evento);

    public void AlFinalizarCombate(Personaje ganador) => Ganador = ganador;
}