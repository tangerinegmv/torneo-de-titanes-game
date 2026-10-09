namespace EspacioApi
{
    using System.Text.Json;
    using System.Text.Json.Serialization;
   
    


// Root myDeserializedClass = JsonSerializer.Deserialize<List<Root>>(myJsonResponse);
    public class Group
    {
        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("sub_groups")]
        public List<string> sub_groups { get; set; }
    }

    public class Relative
    {
        [JsonPropertyName("family")]
        public string family { get; set; }

        [JsonPropertyName("members")]
        public List<string> members { get; set; }
    }

    public class PersonajeDatos
    {
        [JsonPropertyName("id")]
        public int id { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("img")]
        public string img { get; set; }

        [JsonPropertyName("alias")]
        public List<string> alias { get; set; }

        [JsonPropertyName("species")]
        public List<string> species { get; set; }

        [JsonPropertyName("gender")]
        public string gender { get; set; }

        [JsonPropertyName("age")]
        public object age { get; set; }

        [JsonPropertyName("height")]
        public string height { get; set; }

        [JsonPropertyName("relatives")]
        public List<Relative> relatives { get; set; }

        [JsonPropertyName("birthplace")]
        public string birthplace { get; set; }

        [JsonPropertyName("residence")]
        public string residence { get; set; }

        [JsonPropertyName("status")]
        public string status { get; set; }

        [JsonPropertyName("occupation")]
        public string occupation { get; set; }

        [JsonPropertyName("groups")]
        public List<Group> groups { get; set; }

        [JsonPropertyName("roles")]
        public List<string> roles { get; set; }

        [JsonPropertyName("episodes")]
        public List<string> episodes { get; set; }
    }

}