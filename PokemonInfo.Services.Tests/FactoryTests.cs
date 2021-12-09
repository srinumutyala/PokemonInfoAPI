using Microsoft.Extensions.Logging;
using Moq;
using PokemonInfo.Services.Cache;
using PokemonInfo.Services.Factory;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace PokemonInfo.Services.Tests
{
	public class FactoryTests
	{

		private List<ITranslator> lstTranslators;
		public FactoryTests()
		{
			//Arrange
			Mock<IHttpClientFactory> mockFactory = TestHelper.GetHttpFactoryMock(System.Net.HttpStatusCode.OK, string.Empty);
			var yodaLogger = new Mock<ILogger<YodaTranslator>>();
			var shakeSpearLogger = new Mock<ILogger<ShakespeareTranslator>>();
			var cacheManager = new Mock<ICacheManager>();

			lstTranslators = new List<ITranslator>();
			lstTranslators.Add(new YodaTranslator(mockFactory.Object, cacheManager.Object, yodaLogger.Object));
			lstTranslators.Add(new ShakespeareTranslator(mockFactory.Object, cacheManager.Object, shakeSpearLogger.Object));
		}


		[Theory]
		[InlineData("Shakespeare")]
		[InlineData("Yoda")]
		public void GetFunTranslatorService_returns_correct_translator(string translatorType)
		{
			
			//Arrange
			var translatorFactory = new TranslatorFactory(lstTranslators.AsEnumerable());

			//Act
			var result = translatorFactory.GetFunTranslatorService(translatorType);

			//Assert
			Assert.NotNull(result);
			Assert.Equal(result.TranslatorType, translatorType);

		}

		[Fact]
		public void GetFunTranslatorService_returns_null()
		{

			//Arrange
			var translatorFactory = new TranslatorFactory(lstTranslators.AsEnumerable());

			//Act
			var result = translatorFactory.GetFunTranslatorService("unknown");

			//Assert
			Assert.Null(result);

		}


	}
}
