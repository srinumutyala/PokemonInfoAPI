using Microsoft.Extensions.Logging;
using Moq;
using PokemonInfo.Entities;
using PokemonInfo.Services.Cache;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PokemonInfo.Services.Tests
{
	public class YodaTranslatorTests
	{
		private Mock<IHttpClientFactory> mockFactory;
		private Mock<ILogger<YodaTranslator>> yodaLogger;
		private readonly string translatedDescription = "yoda translated sample description";
		private readonly Mock<ICacheManager> cacheManager;
		public YodaTranslatorTests()
		{
			//Arrange
			yodaLogger = new Mock<ILogger<YodaTranslator>>();
			cacheManager = new Mock<ICacheManager>();
		}

		[Fact]
		public async Task YodaTranslator_Translate_returns_translated_description()
		{
			//Arrange
			mockFactory = TestHelper.GetHttpFactoryMock(System.Net.HttpStatusCode.OK, new TranslationModel(){Content = new ContentModel() { Translated = translatedDescription } });
			var yodaTranslator = new YodaTranslator(mockFactory.Object, cacheManager.Object, yodaLogger.Object);

			cacheManager.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<string>()))
			.Callback<string, string>((key, cache) =>
			{
				cacheManager.Setup(s => s.TryGetValue(key, out cache))
					.Returns(true);
			});

			//Act
			var result = await yodaTranslator.Translate("sample description");

			//Assert
			Assert.NotNull(result.Value);
			Assert.Equal(translatedDescription, result.Value);

			//Now check to see if the description available from Cache
			//if it is not hitting the cache, we should get null reference exception 
			//These tests can be written more efficiently by introducing the ordered tests
			yodaTranslator = new YodaTranslator(null, cacheManager.Object, yodaLogger.Object);
			//Act
			var cachedResult = await yodaTranslator.Translate("sample description");

			//Assert
			Assert.Equal(translatedDescription, cachedResult.Value);

		}
		[Theory]
		[InlineData("Too Many Requests", HttpStatusCode.TooManyRequests)]
		[InlineData("Not Found", HttpStatusCode.NotFound)]
		public async Task YodaTranslator_Translate_returns_errors(string translatedDescription, HttpStatusCode httpStatusCode)
		{
			//Arrange
			mockFactory = TestHelper.GetHttpFactoryMock(httpStatusCode, translatedDescription);
			var yodaTranslator = new YodaTranslator(mockFactory.Object, cacheManager.Object, yodaLogger.Object);

			//Act
			var result = await yodaTranslator.Translate("sample description");

			//Assert
			Assert.True(result.Failed);
			Assert.NotNull(result.ErrorResult);
			Assert.Equal(httpStatusCode, result.ErrorResult.StatusCode);

		}
	}
}
