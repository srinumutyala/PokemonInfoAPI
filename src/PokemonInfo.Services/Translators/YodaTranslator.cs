using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PokemonInfo.Entities;
using PokemonInfo.Services.Cache;
using PokemonInfo.Services.Clients;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static PokemonInfo.Services.Result;

namespace PokemonInfo.Services
{
	public class YodaTranslator : ITranslator
	{

		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ILogger<YodaTranslator> _logger;
		private readonly ICacheManager _cacheManager;
		public YodaTranslator(IHttpClientFactory httpClientFactor, ICacheManager cacheManager, 
			ILogger<YodaTranslator> logger)
		{
			this._httpClientFactory = httpClientFactor;
			_cacheManager = cacheManager;
			this._logger = logger;
		}

		public string TranslatorType => TranslatorConstants.Yoda;

		public async Task<Result<string>> Translate(string descriptionText)
		{
			if (_cacheManager.TryGetValue(descriptionText, out string cachedTranslation))
				return cachedTranslation;

			var httpClient = _httpClientFactory.CreateClient("Translation");
			var descriptionToTranslate = new { text = descriptionText };
			var data = new StringContent(JsonConvert.SerializeObject(descriptionToTranslate), UnicodeEncoding.UTF8, "application/json");
			using (var response = await httpClient.PostAsync($"yoda.json", data))
			{
				var contentStream = await response.Content.ReadAsStreamAsync();

				if (response.IsSuccessStatusCode)
				{
					var  translationModel = PokemonDeserializer.DeserializeStream<TranslationModel>(contentStream);

					var translatedDescription = translationModel.Content.Translated;
					_cacheManager.Set(descriptionText, translatedDescription);
					return translatedDescription;
				}

				_logger.LogWarning($"There is an issue translating using yoda '{descriptionText}'; reason: {response.ReasonPhrase}");
				var content = await PokemonDeserializer.StreamToStringAsync(contentStream);
				return new ErrorResultContent(response.StatusCode, content);
			}
		}
	}
}
