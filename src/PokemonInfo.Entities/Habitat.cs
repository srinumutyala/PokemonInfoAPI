using Newtonsoft.Json;

namespace PokemonInfo.Entities
{
    public class Habitat
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
