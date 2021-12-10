using Newtonsoft.Json;

namespace PokemonInfo.Entities
{
    public class FlavorTextEntries
    {
        [JsonProperty("flavor_text")]
        public string FlavorText { get; set; }

        [JsonProperty("language")]
        public Language Language { get; set; }
    }
}
