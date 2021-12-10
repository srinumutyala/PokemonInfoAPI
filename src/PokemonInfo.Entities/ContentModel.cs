using Newtonsoft.Json;

namespace PokemonInfo.Entities
{
	public class ContentModel
	{
		[JsonProperty("translated")]
		public string Translated { get; set; }
	}
}
