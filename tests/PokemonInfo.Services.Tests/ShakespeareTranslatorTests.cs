using Microsoft.Extensions.Logging;
using Moq;
using PokemonInfo.Entities;
using PokemonInfo.Services;
using PokemonInfo.Services.Cache;
using PokemonInfo.Services.Tests;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace PokemonInfoAPI.Unit.Tests
{
	public class ShakespeareTranslatorTests
	{

		private Mock<IHttpClientFactory> mockFactory;
		private Mock<ILogger<ShakespeareTranslator>> shakespeareLogger;
		private readonly string translatedDescription = "shakespear translated sample description";
		private readonly Mock<ICacheManager> cacheManager;
		public ShakespeareTranslatorTests()
		{
			//Arrange
			shakespeareLogger = new Mock<ILogger<ShakespeareTranslator>>();
			cacheManager = new Mock<ICacheManager>();
		}

		[Fact]
		public async Task ShakespeareTranslator_Translate_returns_translated_description()
		{
			//Arrange
			mockFactory = TestHelper.GetHttpFactoryMock(System.Net.HttpStatusCode.OK, new TranslationModel() { Content = new ContentModel() { Translated = translatedDescription } });
			var shakespeareTranslator = new ShakespeareTranslator(mockFactory.Object, cacheManager.Object, shakespeareLogger.Object);

			cacheManager.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<string>()))
			.Callback<string, string>((key, cache) =>
			{
				cacheManager.Setup(s => s.TryGetValue(key, out cache))
					.Returns(true);
			});

			//Act
			var result = await shakespeareTranslator.Translate("sample description");

			//Assert
			Assert.NotNull(result.Value);
			Assert.Equal(translatedDescription, result.Value);

			//Now check to see if the description available from Cache
			//if it is not hitting the cache, we should get null reference exception 
			//These tests can be written more efficiently by introducing the ordered tests
			shakespeareTranslator = new ShakespeareTranslator(null, cacheManager.Object, shakespeareLogger.Object);
			//Act
			var cachedResult = await shakespeareTranslator.Translate("sample description");

			//Assert
			Assert.Equal(translatedDescription, cachedResult.Value);

		}
		[Theory]
		[InlineData("Too Many Requests", HttpStatusCode.TooManyRequests)]
		[InlineData("Not Found", HttpStatusCode.NotFound)]
		public async Task ShakespeareTranslator_Translate_returns_errors(string translatedDescription, HttpStatusCode httpStatusCode)
		{
			//Arrange
			mockFactory = TestHelper.GetHttpFactoryMock(httpStatusCode, new TranslationModel() { Content = new ContentModel() { Translated = translatedDescription } });
			var yodaTranslator = new ShakespeareTranslator(mockFactory.Object, cacheManager.Object, shakespeareLogger.Object);

			//Act
			var result = await yodaTranslator.Translate("sample description");

			//Assert
			Assert.True(result.Failed);
			Assert.NotNull(result.ErrorResult);
			Assert.Equal(httpStatusCode, result.ErrorResult.StatusCode);

		}
	}
}
