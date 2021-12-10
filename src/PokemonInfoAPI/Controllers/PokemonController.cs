using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokemonInfo.Entities;
using PokemonInfo.Services;
using PokemonInfo.Services.Factory;
using PokemonInfoAPI.Helpers;
using System.Threading.Tasks;

namespace PokemonInfoAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PokemonController : ControllerBase
	{
		private readonly ILogger<PokemonController> _logger;
        private readonly IPokemonApiClient _pokemonApiClient;
        private readonly ITranslatorFactory _translatorFactory;

        public PokemonController(IPokemonApiClient pokemonInnfoService, ITranslatorFactory translatorFactory,
             ILogger<PokemonController> logger)
		{
			_logger = logger;
            _pokemonApiClient = pokemonInnfoService;
            _translatorFactory = translatorFactory; 
        }

        // GET: pokemon/wormadam
        [HttpGet("{name}", Name = "pokemon")]
        public async Task<ActionResult<Pokemon>> GetPokemonInfo(string name)
        {
            //TODO: In production, this empty check can be handles using attributes
            if(string.IsNullOrWhiteSpace(name))
                return BadRequest($"The pokemon name {name} is invalid");

			var pokemonResult = await _pokemonApiClient.GetPokemonAsync(name);

            if (pokemonResult.Succeeded)
            {
                return pokemonResult.Value;
            }

            return StaticHelper.HandleFailure(pokemonResult, name);
        }


        // GET: pokemon/translated/wormadam
        [HttpGet("translated/{name}")]
        public async Task<ActionResult<Pokemon>> GetPokemonInfoWithTranslation(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest($"The pokemon name {name} is invalid");

			var pokemonResult = await _pokemonApiClient.GetPokemonAsync(name);

            if (!pokemonResult.Succeeded)
                return StaticHelper.HandleFailure(pokemonResult, name);

            //Translate is the description is not empty
            if (!string.IsNullOrEmpty(pokemonResult.Value.Description))
            {
                var translator = _translatorFactory.GetFunTranslatorService(TranslatorTypeHelper.DeriveTRanslatorType(pokemonResult.Value));
                var translatedDescrption = await translator.Translate(pokemonResult.Value.Description);
                pokemonResult.Value.Description = translatedDescrption.Value?? pokemonResult.Value.Description;

                return pokemonResult.Value;
            }

            _logger.LogWarning($"No description found for the given pokemon: {name}");

            return StaticHelper.HandleFailure(pokemonResult, name);
        }
	}
}
