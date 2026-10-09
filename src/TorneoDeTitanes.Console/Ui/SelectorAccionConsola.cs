using Spectre.Console;
using TorneoDeTitanes.Core.Abstracciones;
using TorneoDeTitanes.Core.Combate;

namespace TorneoDeTitanes.Console.Ui;

public sealed class SelectorAccionConsola : ISelectorAccion
{
    public AccionDisponible ElegirAccion(ContextoTurno contexto)
    {
        var opciones = contexto.AccionesDisponibles
            .Select(accion => new OpcionAccion(accion, Describir(accion, contexto)))
            .ToList();

        opciones.Add(new OpcionAccion(AccionDisponible.Rendirse, Describir(AccionDisponible.Rendirse, contexto)));

        while (true)
        {
            var elegida = AnsiConsole.Prompt(
                new SelectionPrompt<OpcionAccion>()
                    .Title($"[bold]Turno del Titán [yellow]{Markup.Escape(contexto.Actor.Nombre)}[/] — ¿qué harás?[/]")
                    .PageSize(6)
                    .HighlightStyle(new Style(foreground: Color.Gold1))
                    .AddChoices(opciones));

            if (elegida.Accion != AccionDisponible.Rendirse
                || AnsiConsole.Confirm("[red]¿Seguro que deseas rendirte? Tu Titán sufrirá una penalización en sus estadísticas.[/]", defaultValue: false))
            {
                return elegida.Accion;
            }

            AnsiConsole.MarkupLine("[grey]Continúas en pie de lucha.[/]\n");
        }
    }

    private static string Describir(AccionDisponible accion, ContextoTurno contexto) => accion switch
    {
        AccionDisponible.Atacar => "⚔ Atacar — golpe básico, ganas energía",
        AccionDisponible.Habilidad =>
            $"🔥 {contexto.HabilidadEspecial.Nombre} (costo {contexto.HabilidadEspecial.CostoEnergia} energía)",
        AccionDisponible.Ultimate =>
            $"💥 TÉCNICA DEFINITIVA: {contexto.HabilidadUltimate.Nombre} (100 de furia)",
        AccionDisponible.Defender => "🛡 Defender — escudo, energía y contraataque",
        AccionDisponible.Rendirse => "🏳 Rendirse — abandonar el combate (penaliza estadísticas)",
        _ => accion.ToString(),
    };

    private sealed record OpcionAccion(AccionDisponible Accion, string Etiqueta)
    {
        public override string ToString() => Etiqueta;
    }
}