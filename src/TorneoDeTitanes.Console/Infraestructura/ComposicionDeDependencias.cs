using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TorneoDeTitanes.Console.Ui;
using TorneoDeTitanes.Core.Abstracciones;
using TorneoDeTitanes.Core.Combate;
using TorneoDeTitanes.Core.Habilidades;
using TorneoDeTitanes.Core.Personajes;

namespace TorneoDeTitanes.Console.Infraestructura;

public static class ComposicionDeDependencias
{
    public static IServiceCollection AgregarJuego(this IServiceCollection servicios, IConfiguration configuracion)
    {
        servicios.Configure<OpcionesJuego>(configuracion.GetSection(OpcionesJuego.Seccion));

        servicios.AddSingleton<IRandomProvider>(_ => new RandomProvider());
        servicios.AddSingleton<ICalculadoraDano, CalculadoraDano>();

        servicios.AddSingleton<IHabilidad, HabilidadColosal>();
        servicios.AddSingleton<IHabilidad, HabilidadAcorazado>();
        servicios.AddSingleton<IHabilidad, HabilidadBestia>();
        servicios.AddSingleton<IHabilidad, HabilidadAtaque>();
        servicios.AddSingleton<IHabilidad, HabilidadUltimateColosal>();
        servicios.AddSingleton<IHabilidad, HabilidadUltimateAcorazado>();
        servicios.AddSingleton<IHabilidad, HabilidadUltimateBestia>();
        servicios.AddSingleton<IHabilidad, HabilidadUltimateAtaque>();
        servicios.AddSingleton<IProveedorHabilidades, ProveedorHabilidades>();

        servicios.AddSingleton<FabricaDePersonajes>();
        servicios.AddSingleton<MotorDeCombate>();

        servicios.AddSingleton<IPersonajeRepositorio, JsonPersonajeRepositorio>();
        servicios.AddSingleton<IHistorialRepositorio, JsonHistorialRepositorio>();
        servicios.AddHttpClient<IPersonajeApiClient, AttackOnTitanApiClient>();

        servicios.AddSingleton<ISelectorAccion, SelectorAccionIa>();
        servicios.AddSingleton<ObservadorCombateConsola>();
        servicios.AddSingleton<SelectorAccionConsola>();
        servicios.AddSingleton<TorneoConsola>();

        return servicios;
    }
}