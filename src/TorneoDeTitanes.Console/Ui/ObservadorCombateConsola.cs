using Microsoft.Extensions.Options;
using Spectre.Console;
using TorneoDeTitanes.Console.Infraestructura;
using TorneoDeTitanes.Core.Abstracciones;
using TorneoDeTitanes.Core.Combate;
using TorneoDeTitanes.Core.Modelos;

namespace TorneoDeTitanes.Console.Ui;

public sealed class ObservadorCombateConsola : ICombateObserver
{
    private const double FactorRespuestaContrincante = 2.0;

    private readonly int _velocidadMs;
    private bool _esTurnoDelJugador;

    public ObservadorCombateConsola(IOptions<OpcionesJuego> opciones)
    {
        _velocidadMs = opciones.Value.VelocidadAnimacionMs;
    }

    public void AlIniciarCombate(Personaje jugador, Personaje rival)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold red]¡EL COMBATE COMIENZA![/]").RuleStyle("red"));
        AnsiConsole.MarkupLine(
            $"\n[bold yellow]{Markup.Escape(jugador.Nombre)}[/] (Titán {jugador.Tipo})  vs  " +
            $"[bold blue]{Markup.Escape(rival.Nombre)}[/] (Titán {rival.Tipo})\n");
    }

    public void AlActualizarEstado(Personaje jugador, Personaje rival, int numeroTurno, bool esTurnoDelJugador)
    {
        _esTurnoDelJugador = esTurnoDelJugador;
        AnsiConsole.Clear();
        Pantallas.MostrarTablaEstado(jugador, rival, numeroTurno);
        AnsiConsole.WriteLine();
    }

    public void AlMostrarEvento(EventoCombate evento)
    {
        Pantallas.MostrarEvento(evento);

        double demora = _esTurnoDelJugador ? 1 : FactorRespuestaContrincante;
        Thread.Sleep((int)(_velocidadMs * demora));
    }

    public void AlFinalizarCombate(Personaje ganador)
    {
        AnsiConsole.MarkupLine(
            $"\n[bold gold1]{Markup.Escape(ganador.Nombre.ToUpperInvariant())}[/] [grey]domina el campo de batalla.[/]\n");
    }
}