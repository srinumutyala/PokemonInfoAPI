using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PokemonInfo.Entities;
using PokemonInfo.Services;
using PokemonInfo.Services.Factory;
using PokemonInfo.Services.Tests;
using PokemonInfoAPI.Controllers;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using static PokemonInfo.Services.Result;

namespace PokemonInfoAPI.Unit.Tests
{
   
    public class PokemonControllerTests : IDisposable
    {
        private readonly Mock<ILogger<PokemonController>> _logger;
        private readonly Mock<IPokemonApiClient> _pokemonApiClient;
        private readonly Mock<ITranslatorFactory> _translatorFactory;
        PokemonController _controller;

        public void Dispose()
		{
			//Write any cleanup code here
		}

		public PokemonControllerTests()
        {
            _logger = new Mock<ILogger<PokemonController>>();
            _pokemonApiClient = new Mock<IPokemonApiClient>();
            _translatorFactory = new Mock<ITranslatorFactory>();
			_controller = new PokemonController(_pokemonApiClient.Object, _translatorFactory.Object,  _logger.Object);
        }

		

		[Fact]
        public async void GetPokemonInfo_returns_valid_result()
        {
			//Arrange
			var name = "Foliat";
			var description = "Although Foliat is unable to fly, it still has an extremely high metabolism. It's often found feeding on sugar-rich nectar and sap found in plants";
			var expectedPokemon = new Pokemon(TestHelper.GetPokemon(name, description, "en"));

			_pokemonApiClient.Setup(c => c.GetPokemonAsync(It.IsAny<string>()))
				.Returns(Task.FromResult(
					new Result<Pokemon>(
						expectedPokemon)));

			//Act
			var result = await _controller.GetPokemonInfo(name);

			//Assert
			Assert.Equal(expectedPokemon, result.Value);
		}

		//[Skip]
		[Theory]
		[InlineData("NotFound", 404, "NotFound", HttpStatusCode.NotFound)]
		[InlineData("Too many requests", 429, "Too many requests", HttpStatusCode.TooManyRequests)]
		public async  void GetPokemonInfo_returns_error_results(string expectedResultMessage, int expectedStatusCode, string errorMessage, HttpStatusCode httpStatusCode)
		{
			//Arrange
			var name = "cat";
			var errorResult = new ErrorResultContent(httpStatusCode, errorMessage);
			_pokemonApiClient.Setup(c => c.GetPokemonAsync(It.IsAny<string>()))
					.Returns(Task.FromResult(new Result<Pokemon>(errorResult)));

			//Act
			var result = await _controller.GetPokemonInfo(name);
			var contentResult = result.Result as ContentResult;

			//Assert
			Assert.Equal(expectedStatusCode, contentResult.StatusCode);
			Assert.Equal(expectedResultMessage, contentResult.Content);
		}

		[Fact]
		public async void GetPokemonInfoWithTranslation_returns_valid_result_yodatranslation()
		{
			//Arrange
			var name = "Foliat";
			var description = "Although Foliat is unable to fly, it still has an extremely high metabolism. It's often found feeding on sugar-rich nectar and sap found in plants";
			var translatedDescription = "Translated with yoda: Although Foliat is unable to fly, it still has an extremely high metabolism. It's often found feeding on sugar-rich nectar and sap found in plants";
			var expectedPokemon = new Pokemon(TestHelper.GetPokemon(name, description, "en",string.Empty, true));

			_pokemonApiClient.Setup(c => c.GetPokemonAsync(It.IsAny<string>()))
				.Returns(Task.FromResult(
					new Result<Pokemon>(
						expectedPokemon)));

			var mockedYodaTranslator = new Mock<ITranslator>();
			_translatorFactory.Setup(c => c.GetFunTranslatorService(TranslatorConstants.Yoda)).Returns(mockedYodaTranslator.Object);
			mockedYodaTranslator.Setup(c => c.Translate(It.IsAny<string>())).Returns(Task.FromResult(
					new Result<string>(translatedDescription)));
			//Act
			var result = await _controller.GetPokemonInfoWithTranslation(name);

			//Assert
			Assert.Equal(expectedPokemon, result.Value);
		}

		[Fact]
		public async void GetPokemonInfoWithTranslation_returns_valid_result_shakeapearetranslation()
		{
			//Arrange
			var name = "Foliat";
			var description = "Although Foliat is unable to fly, it still has an extremely high metabolism. It's often found feeding on sugar-rich nectar and sap found in plants";
			var translatedDescription = "Translated with Shakespeare: Although Foliat is unable to fly, it still has an extremely high metabolism. It's often found feeding on sugar-rich nectar and sap found in plants";
			var expectedPokemon = new Pokemon(TestHelper.GetPokemon(name, description, "en", string.Empty, false));

			_pokemonApiClient.Setup(c => c.GetPokemonAsync(It.IsAny<string>()))
				.Returns(Task.FromResult(
					new Result<Pokemon>(
						expectedPokemon)));

			var mockedShakespeareTranslator = new Mock<ITranslator>();
			_translatorFactory.Setup(c => c.GetFunTranslatorService(TranslatorConstants.Shakespeare)).Returns(mockedShakespeareTranslator.Object);
			mockedShakespeareTranslator.Setup(c => c.Translate(It.IsAny<string>())).Returns(Task.FromResult(
					new Result<string>(translatedDescription)));
			//Act
			var result = await _controller.GetPokemonInfoWithTranslation(name);

			//Assert
			Assert.Equal(expectedPokemon, result.Value);
		}


		[Theory]
		[InlineData("NotFound", 404, "NotFound", HttpStatusCode.NotFound)]
		[InlineData("Too many requests", 429, "Too many requests", HttpStatusCode.TooManyRequests)]
		public async void GetPokemonInfoWithTranslation_returns_error_results(string expectedResultMessage, int expectedStatusCode, string errorMessage, HttpStatusCode httpStatusCode)
		{
			//Arrange
			var name = "cat";
			var errorResult = new ErrorResultContent(httpStatusCode, errorMessage);
			_pokemonApiClient.Setup(c => c.GetPokemonAsync(It.IsAny<string>()))
					.Returns(Task.FromResult(new Result<Pokemon>(errorResult)));

			//Act
			var result = await _controller.GetPokemonInfoWithTranslation(name);
			var contentResult = result.Result as ContentResult;

			//Assert
			Assert.Equal(expectedStatusCode, contentResult.StatusCode);
			Assert.Equal(expectedResultMessage, contentResult.Content);
		}

	}
}
