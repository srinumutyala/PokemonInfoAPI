﻿using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PokemonInfo.Entities;
using PokemonInfo.Services.Cache;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static PokemonInfo.Services.Result;

namespace PokemonInfo.Services
{
	public class ShakespeareTranslator : ITranslator
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ILogger<ShakespeareTranslator> _logger;
		private readonly ICacheManager _cacheManager;

		public ShakespeareTranslator(IHttpClientFactory httpClientFactor, ICacheManager cacheManager, 
			ILogger<ShakespeareTranslator> logger)
		{
			_httpClientFactory = httpClientFactor;
			_cacheManager = cacheManager;
			_logger = logger;
		}

		public string TranslatorType => TranslatorConstants.Shakespeare;

		public async Task<Result<string>> Translate(string descriptionText)
		{
			if (_cacheManager.TryGetValue(descriptionText, out string cachedTranslation))
				return cachedTranslation;

			var httpClient = _httpClientFactory.CreateClient("Translation");
			var descriptionToTranslate = new { text = descriptionText };
			var data = new StringContent(JsonConvert.SerializeObject(descriptionToTranslate), UnicodeEncoding.UTF8, "application/json");
			
			using (var response = await httpClient.PostAsync($"shakespeare.json", data))
			{
				var contentStream = await response.Content.ReadAsStreamAsync();

				if (response.IsSuccessStatusCode)
				{
					var translationModel = PokemonDeserializer.DeserializeStream<TranslationModel>(contentStream);
					var translatedDescription = translationModel.Content.Translated;
					_cacheManager.Set(descriptionText, translatedDescription);
					return translatedDescription;
				}

				_logger.LogWarning($"There is an issue translating using shakespeare '{descriptionText}'; reason: {response.ReasonPhrase}");
				var content = await PokemonDeserializer.StreamToStringAsync(contentStream);
				return new ErrorResultContent(response.StatusCode, content);
			}
		}
	}
}
