using PokemonInfo.Entities;
using PokemonInfo.Services;

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
