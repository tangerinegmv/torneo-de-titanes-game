using Spectre.Console;
using System.Globalization;
using TorneoDeTitanes.Core.Combate;
using TorneoDeTitanes.Core.Modelos;

namespace TorneoDeTitanes.Console.Ui;

public static class Pantallas
{
    public static void MostrarIntroduccion()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new FigletText("Torneo de Titanes").Color(Color.DarkRed));
        AnsiConsole.Write(new Rule("[grey]Attack on Titan AU[/]").RuleStyle("red"));

        AnsiConsole.MarkupLine(
            "\nEn las profundidades de la Isla Paradis, un antiguo secreto se esconde entre las sombras.");
        AnsiConsole.MarkupLine(
            "Cada ser nacido en esta tierra posee un poder aterrador: [red]el poder de un Titán[/].");
        AnsiConsole.MarkupLine(
            "Estos seres se enfrentan en un torneo sagrado para determinar al [bold]Titán Primordial[/].\n");
        AnsiConsole.MarkupLine(
            "[italic]Solo el más fuerte, el más astuto y el más valiente se alzará como gobernante supremo.[/]\n");

        Pausar("Presiona cualquier tecla para continuar...");
    }

    public static string MostrarMenuPrincipal()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new FigletText("Menu").Color(Color.Red));

        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold]Selecciona una opción[/]")
                .AddChoices("Jugar Torneo", "Ver Historial de Campeones", "Salir"));
    }

    public static bool Confirmar(string mensaje)
        => AnsiConsole.Confirm(mensaje, defaultValue: true);

    public static Personaje? SeleccionarPersonaje(IReadOnlyList<Personaje> personajes)
    {
        AnsiConsole.Clear();
        MostrarTablaPersonajes(personajes);

        var opciones = personajes
            .Select(p => new OpcionSeleccion(
                $"{p.Nombre} — Titán {p.Tipo} (Fue {p.Fuerza} / Vel {p.Velocidad} / Des {p.Destreza} / Arm {p.Armadura})",
                p))
            .ToList();

        opciones.Add(new OpcionSeleccion("[grey]← Volver al menú principal[/]", Personaje: null));

        var elegida = AnsiConsole.Prompt(
            new SelectionPrompt<OpcionSeleccion>()
                .Title("[bold yellow]Selecciona tu Titán para el torneo:[/]")
                .PageSize(10)
                .UseConverter(o => o.Texto)
                .AddChoices(opciones));

        return elegida.Personaje;
    }

    private sealed record OpcionSeleccion(string Texto, Personaje? Personaje)
    {
        public override string ToString() => Texto;
    }

    public static void MostrarTablaPersonajes(IReadOnlyList<Personaje> personajes)
    {
        var tabla = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Red)
            .Title("[bold]Combatientes registrados[/]");

        tabla.AddColumn("[grey]Nombre[/]");
        tabla.AddColumn(new TableColumn("[grey]Género[/]").Centered());
        tabla.AddColumn(new TableColumn("[grey]Edad[/]").Centered());
        tabla.AddColumn(new TableColumn("[grey]Titán[/]").Centered());
        tabla.AddColumn(new TableColumn("[grey]Fue[/]").Centered());
        tabla.AddColumn(new TableColumn("[grey]Vel[/]").Centered());
        tabla.AddColumn(new TableColumn("[grey]Des[/]").Centered());
        tabla.AddColumn(new TableColumn("[grey]Arm[/]").Centered());
        tabla.AddColumn(new TableColumn("[grey]Nivel[/]").Centered());

        foreach (var p in personajes)
        {
            tabla.AddRow(
                Markup.Escape(p.Nombre),
                Markup.Escape(p.Genero),
                p.Edad.ToString(CultureInfo.InvariantCulture),
                $"[red]{p.Tipo}[/]",
                p.Fuerza.ToString(CultureInfo.InvariantCulture),
                p.Velocidad.ToString(CultureInfo.InvariantCulture),
                p.Destreza.ToString(CultureInfo.InvariantCulture),
                p.Armadura.ToString(CultureInfo.InvariantCulture),
                p.Nivel.ToString(CultureInfo.InvariantCulture));
        }

        AnsiConsole.Write(tabla);
    }

    public static void MostrarEncabezadoRonda(int ronda, int total, Personaje jugador, Personaje rival)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule($"[bold red]RONDA {ronda} de {total}[/]").RuleStyle("red"));
        AnsiConsole.MarkupLine(
            $"\n[bold yellow]{Markup.Escape(jugador.Nombre)}[/] (Titán {jugador.Tipo})  vs  [bold blue]{Markup.Escape(rival.Nombre)}[/] (Titán {rival.Tipo})\n");
    }

    public static void MostrarTablaEstado(Personaje jugador, Personaje rival, int turno)
    {
        var tabla = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .Title($"[grey]Turno {turno}[/]");

        tabla.AddColumn("[grey]Titán[/]");
        tabla.AddColumn("[grey]Salud[/]");
        tabla.AddColumn("[grey]Energía[/]");
        tabla.AddColumn("[grey]Furia[/]");
        tabla.AddColumn("[grey]Escudo[/]");
        tabla.AddColumn("[grey]Estados[/]");

        AgregarFilaEstado(tabla, jugador, Color.Yellow);
        AgregarFilaEstado(tabla, rival, Color.Blue);

        AnsiConsole.Write(tabla);
    }

    private static void AgregarFilaEstado(Table tabla, Personaje personaje, Color color)
    {
        string estados = personaje.Estados.Count == 0
            ? "[grey]-[/]"
            : string.Join(", ", personaje.Estados.Select(e => $"{e.Tipo}({e.Duracion})"));

        string escudo = personaje.Escudo > 0
            ? $"[cyan]{personaje.Escudo}[/]"
            : "[grey]-[/]";

        tabla.AddRow(
            $"[{color}]{Markup.Escape(personaje.Nombre)}[/]",
            Barra(personaje.PorcentajeSalud, Color.Lime),
            Barra(personaje.PorcentajeEnergia, Color.Aqua),
            Barra(personaje.PorcentajeFuria, Color.Red),
            escudo,
            estados);
    }

    public static void MostrarEvento(EventoCombate evento)
    {
        string color = evento.Tipo switch
        {
            TipoEvento.Ataque => "orange1",
            TipoEvento.Critico => "red bold",
            TipoEvento.Esquiva => "cyan",
            TipoEvento.Bloqueo => "blue",
            TipoEvento.Habilidad => "magenta1 bold",
            TipoEvento.Estado => "yellow3_1",
            TipoEvento.Curacion => "green",
            TipoEvento.Mejora => "green bold",
            TipoEvento.Muerte => "red bold italic",
            TipoEvento.Rendicion => "grey italic",
            _ => "grey",
        };

        AnsiConsole.MarkupLine($"[{color}]{Markup.Escape(evento.Texto)}[/]");
    }

    public static void MostrarVictoriaRonda(Personaje ganador, Personaje perdedor)
    {
        AnsiConsole.MarkupLine(
            $"\n[gold1 bold]{Markup.Escape(ganador.Nombre.ToUpperInvariant())}[/] [green]ha derrotado a[/] [red]{Markup.Escape(perdedor.Nombre)}[/] en esta ronda!\n");
    }

    public static void MostrarPenalizacion(Personaje personaje)
    {
        AnsiConsole.MarkupLine(
            $"\n[red]{Markup.Escape(personaje.Nombre)} se rindió: sus estadísticas se debilitan.[/] " +
            $"[grey](Fue {personaje.Fuerza} / Vel {personaje.Velocidad} / Des {personaje.Destreza} / Arm {personaje.Armadura} / Nivel {personaje.Nivel})[/]");
    }

    public static void MensajeEliminado()
    {
        AnsiConsole.MarkupLine(
            "\n[bold red]Tu Titán cae: los tambores de guerra señalan su salida del torneo.[/]");
        Pausar("Presiona una tecla para conocer al campeón del torneo...");
    }

    public static string Barra(double porcentaje, Color color, int ancho = 12)
    {
        int llenos = (int)Math.Round(Math.Clamp(porcentaje, 0, 1) * ancho);
        string bloque = new string('█', llenos).PadRight(ancho, '░');
        return $"[{color}]{bloque}[/] {porcentaje * 100:0}%";
    }

    public static void MostrarMejora(Personaje campeon)
    {
        AnsiConsole.MarkupLine(
            $"\n[green]{Markup.Escape(campeon.Nombre)} ha mejorado sus estadísticas tras la victoria![/] " +
            $"[grey](Fue {campeon.Fuerza} / Vel {campeon.Velocidad} / Des {campeon.Destreza} / Arm {campeon.Armadura} / Nivel {campeon.Nivel})[/]");
    }

    public static void MostrarCampeon(Personaje campeon)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new FigletText("Campeon").Color(Color.Gold1));

        var panel = new Panel(
                $"[bold]{Markup.Escape(campeon.Nombre.ToUpperInvariant())}[/]\n" +
                $"Titán [red]{campeon.Tipo}[/] — Nivel {campeon.Nivel}\n" +
                $"Fuerza {campeon.Fuerza} | Velocidad {campeon.Velocidad} | Destreza {campeon.Destreza} | Armadura {campeon.Armadura}")
            .Header("[gold1]TITÁN PRIMORDIAL[/]")
            .Border(BoxBorder.Double)
            .BorderColor(Color.Gold1);

        AnsiConsole.Write(panel);
        AnsiConsole.MarkupLine(
            $"\n[italic]El nombre de {Markup.Escape(campeon.Nombre)} resonará por la eternidad.[/]\n");
    }

    public static void MensajeSinPersonajes()
    {
        AnsiConsole.MarkupLine("[red]No hay titanes registrados y no se pudieron crear nuevos. Reintenta más tarde.[/]");
        Pantallas.Pausar("Presiona una tecla para volver al menú principal...");
    }

    public static void MostrarHistorial(IReadOnlyList<HistorialRegistro> historial)
    {
        if (historial.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]Todavía no hay campeones registrados.[/]");
            return;
        }

        var tabla = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Gold1)
            .Title("[bold gold1]HISTORIAL DE CAMPEONES[/]");

        tabla.AddColumn("[grey]Campeón[/]");
        tabla.AddColumn(new TableColumn("[grey]Titán[/]").Centered());
        tabla.AddColumn("[grey]Fecha[/]");

        foreach (var registro in historial)
        {
            tabla.AddRow(
                Markup.Escape(registro.Ganador.Nombre),
                $"[red]{registro.Ganador.Tipo}[/]",
                registro.Hora.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
        }

        AnsiConsole.Write(tabla);
    }

    public static void Pausar(string mensaje)
    {
        AnsiConsole.MarkupLine($"[grey]{Markup.Escape(mensaje)}[/]");
        try
        {
            System.Console.ReadKey(intercept: true);
        }
        catch (InvalidOperationException)
        {
            System.Console.ReadLine();
        }
    }
}
