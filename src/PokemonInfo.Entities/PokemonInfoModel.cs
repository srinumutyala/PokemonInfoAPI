using Newtonsoft.Json;

namespace PokemonInfo.Entities
{
    public class PokemonInfoModel
    {
        [JsonProperty("flavor_text_entries")]
        public FlavorTextEntries[] TextEntries { get; set; }
        
        [JsonProperty("habitat")]
        public Habitat Habitat { get; set; }
        
        [JsonProperty("is_legendary")]
        public bool IsLegendary { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
