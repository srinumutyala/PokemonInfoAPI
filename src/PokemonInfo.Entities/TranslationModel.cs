using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonInfo.Entities
{
	public class TranslationModel
	{

		[JsonProperty("contents")]
		public ContentModel Content { get; set; }

	}
}
