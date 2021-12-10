using Newtonsoft.Json;

namespace PokemonInfo.Entities
{
    public class Language
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
