using System.Text.Json;
using System.Text.Json.Serialization;

namespace TorneoDeTitanes.Core.Integracion;

public sealed class GrupoDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("sub_groups")]
    public List<string> SubGroups { get; set; } = new();
}

public sealed class ParienteDto
{
    [JsonPropertyName("family")]
    public string Family { get; set; } = string.Empty;

    [JsonPropertyName("members")]
    public List<string> Members { get; set; } = new();
}

public sealed class PersonajeApiDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("img")]
    public string Img { get; set; } = string.Empty;

    [JsonPropertyName("alias")]
    public List<string> Alias { get; set; } = new();

    [JsonPropertyName("species")]
    public List<string> Species { get; set; } = new();

    [JsonPropertyName("gender")]
    public string Gender { get; set; } = string.Empty;

    [JsonPropertyName("age")]
    public JsonElement Age { get; set; }

    [JsonPropertyName("height")]
    public string Height { get; set; } = string.Empty;

    [JsonPropertyName("relatives")]
    public List<ParienteDto> Relatives { get; set; } = new();

    [JsonPropertyName("birthplace")]
    public string Birthplace { get; set; } = string.Empty;

    [JsonPropertyName("residence")]
    public string Residence { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("occupation")]
    public string Occupation { get; set; } = string.Empty;

    [JsonPropertyName("groups")]
    public List<GrupoDto> Groups { get; set; } = new();

    [JsonPropertyName("roles")]
    public List<string> Roles { get; set; } = new();

    [JsonPropertyName("episodes")]
    public List<string> Episodes { get; set; } = new();
}
