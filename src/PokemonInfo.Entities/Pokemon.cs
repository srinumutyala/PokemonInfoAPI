using System.Linq;

namespace PokemonInfo.Entities
{
    public class Pokemon
    {
        private const string _language = "en";

        public Pokemon(PokemonInfoModel pokemonModel)
        {
            Name = pokemonModel.Name;
			Description = pokemonModel
                .TextEntries?
				.FirstOrDefault(t => t.Language.Name == _language)?.FlavorText ?? string.Empty;
            IsLegendary = pokemonModel.IsLegendary;
            Habitat = pokemonModel.Habitat?.Name;
		}

        public string Name { get; private set; }

        public string Description { get; set; }

		public bool IsLegendary { get; private set; }

		public string Habitat { get; private set; }
	}
}
