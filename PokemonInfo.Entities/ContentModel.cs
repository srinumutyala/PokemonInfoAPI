using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonInfo.Entities
{
	public class ContentModel
	{
		[JsonProperty("translated")]
		public string Translated { get; set; }
	}
}
