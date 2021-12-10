//using PokemonInfo.Entities;
//using PokemonInfoAPI.Services;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace PokemonInfo.Services
//{
	
//        public class PokemonInfoService : IPokemonInfoService
//    {
//            private readonly IPokemonApiClient _pokemonClient;
//            //private readonly ITranslatorClient _translatorClient;

//            public PokemonInfoService(IPokemonApiClient pokemonClient)
//            {
//                _pokemonClient = pokemonClient;
//                //_translatorClient = translatorClient;
//            }

//            public async Task<ShakespearePokemon> GetPokemon(string name)
//            {
//                var pokemonResult = await _pokemonClient.GetPokemonAsync(name);
//                if (pokemonResult.Failed)
//                {
//                    return pokemonResult.ErrorResult;
//                }

//                var pokemon = pokemonResult.Value;
//                var descriptionResult = await _translatorClient.Translate(pokemon.Description);
//                if (descriptionResult.Failed)
//                {
//                    return descriptionResult.ErrorResult;
//                }

//                return new ShakespearePokemon(pokemon, descriptionResult.Value);
//            }
//        }
//    }
