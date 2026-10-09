using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using TorneoDeTitanes.Core.Abstracciones;
using TorneoDeTitanes.Core.Integracion;

namespace TorneoDeTitanes.Console.Infraestructura;

public sealed class AttackOnTitanApiClient : IPersonajeApiClient
{
    private readonly HttpClient _http;
    private readonly string _url;

    public AttackOnTitanApiClient(HttpClient http, IOptions<OpcionesJuego> opciones)
    {
        _url = opciones.Value.ApiUrl;
        _http = http;
        _http.Timeout = TimeSpan.FromSeconds(20);
    }

    public async Task<IReadOnlyList<PersonajeApiDto>> ObtenerPersonajesAsync(CancellationToken cancelacion = default)
    {
        try
        {
            var personajes = await _http.GetFromJsonAsync<List<PersonajeApiDto>>(_url, cancelacion);
            return personajes ?? new List<PersonajeApiDto>();
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or NotSupportedException)
        {
            return new List<PersonajeApiDto>();
        }
    }
}