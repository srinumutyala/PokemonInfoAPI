
using PokemonInfo.Entities;
using PokemonInfo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonInfoAPI.Helpers
{
	public class TranslatorTypeHelper
	{
		public static string DeriveTRanslatorType(Pokemon pokemon)
		{
			return (pokemon.IsLegendary || pokemon.Habitat == Constants.Cave) ? TranslatorConstants.Yoda : TranslatorConstants.Shakespeare;
		}
	}
}
