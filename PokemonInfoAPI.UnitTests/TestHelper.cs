using PokemonInfo.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonInfoAPI.Tests
{
	public class TestHelper
	{
		public static PokemonInfoModel GetPokemon(string name, string description, string language, string habitat = "", bool islegendary = false)
		{

			var pokemonInfoObj = new PokemonInfoModel()
			{
				Name = "Foliat",
				TextEntries = new List<Flavor_Text_Entries>()
				{
					new Flavor_Text_Entries(){
					FlavorText = "Although Foliat is unable to fly, it still has an extremely high metabolism. It's often found feeding on sugar-rich nectar and sap found in plants",
					Language = new Language() { Name = "en" }
				}
				}.ToArray(),
				IsLegendary = islegendary,
				Habitat = new Habitat() { Name = habitat }
			};
			return pokemonInfoObj;
		}
	}
}
