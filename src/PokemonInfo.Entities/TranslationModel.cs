using Newtonsoft.Json;

namespace PokemonInfo.Entities
{
	public class TranslationModel
	{
		[JsonProperty("contents")]
		public ContentModel Content { get; set; }
	}
}
