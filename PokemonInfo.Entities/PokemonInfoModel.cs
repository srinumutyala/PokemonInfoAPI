using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonInfo.Entities
{
    public class PokemonInfoModel
    {
        [JsonProperty("flavor_text_entries")]
        public Flavor_Text_Entries[] TextEntries { get; set; }
        [JsonProperty("habitat")]
        public Habitat Habitat { get; set; }
        [JsonProperty("is_legendary")]
        public bool IsLegendary { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }

        //[JsonProperty("description")]
        //public string Description { get; set; }
    }

    public class Habitat
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Flavor_Text_Entries
    {
        [JsonProperty("flavor_text")]
        public string FlavorText { get; set; }

        [JsonProperty("language")]
        public Language Language { get; set; }
    }

    public class Language
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
