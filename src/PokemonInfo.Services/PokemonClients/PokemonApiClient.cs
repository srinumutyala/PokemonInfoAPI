using Microsoft.Extensions.Logging;
using PokemonInfo.Entities;
using PokemonInfo.Services.Cache;
using System.Net.Http;
using System.Threading.Tasks;
using static PokemonInfo.Services.Result;

namespace PokemonInfo.Services.Clients
{
	public class PokemonApiClient : IPokemonApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger _logger;
		private readonly ICacheManager _cacheManager;

		public PokemonApiClient(IHttpClientFactory httpClientFactory, ICacheManager cacheManager, ILogger<PokemonApiClient> logger)
        {
            _httpClientFactory = httpClientFactory;
			_cacheManager = cacheManager;
			_logger = logger;
        }

        public async Task<Result<Pokemon>> GetPokemonAsync(string name)
        {
			if (_cacheManager.TryGetValue(name, out Pokemon cachedPokemon))
				return cachedPokemon;

			var httpClient = _httpClientFactory.CreateClient("Pokemon");

			using (var response = await httpClient.GetAsync(name))
			{
				var contentStream = await response.Content.ReadAsStreamAsync();

				if (response.IsSuccessStatusCode)
				{
					PokemonInfoModel pokemonObj = PokemonDeserializer.DeserializeStream<PokemonInfoModel>(contentStream);

					var result =  new Pokemon(pokemonObj);

					_cacheManager.Set(name, result);

					return result;
				}

				_logger.LogWarning("The pokemon named '{name}' could not be retrieved; reason: {reason}", name, response.ReasonPhrase);
				var content = await PokemonDeserializer.StreamToStringAsync(contentStream);
				return new ErrorResultContent(response.StatusCode, content);
			}
        }
	}
}
