using Microsoft.Extensions.Logging;
using Moq;
using PokemonInfo.Entities;
using PokemonInfo.Services.Cache;
using PokemonInfo.Services.Clients;
using PokemonInfo.Services.Tests;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using static PokemonInfo.Services.Result;

namespace PokemonInfoAPI.Tests
{
	public class PokemonApiClientTests
	{
		[Fact]
		public async Task GetPokemonAsync_returns_valid_result()
		{
			// Arrange
			var name = "Foliat";
			var description = "Although Foliat is unable to fly, it still has an extremely high metabolism. It's often found feeding on sugar-rich nectar and sap found in plants";
			var expectedPokemon = TestHelper.GetPokemon(name, description, "en", string.Empty, true);
			var expectedResult = new Pokemon(expectedPokemon);

			IHttpClientFactory factory = TestHelper.GetHttpFactoryMock(HttpStatusCode.OK, expectedPokemon).Object;
			var logger = new Mock<ILogger<PokemonApiClient>>();
			var cacheManager = new Mock<ICacheManager>();
			var pokeMonClient = new PokemonApiClient(factory, cacheManager.Object, logger.Object);

			// Act
			var result = await pokeMonClient.GetPokemonAsync(name);

			// Assert
			Assert.NotNull(result.Value);
			Assert.Equal(expectedResult.Name, result.Value.Name);
			Assert.Equal(expectedResult.Description, result.Value.Description);
			Assert.Equal(expectedResult.IsLegendary, result.Value.IsLegendary);

		}

		[Theory]
		[InlineData( 404, "NotFound", HttpStatusCode.NotFound)]
		[InlineData( 429, "Too many requests", HttpStatusCode.TooManyRequests)]
		public async void GetPokemonInfo_returns_error_results(int expectedStatusCode, string errorMessage, HttpStatusCode httpStatusCode)
		{
			// Arrange
			var name = "cat";
			IHttpClientFactory factory = TestHelper.GetHttpFactoryMock(httpStatusCode, errorMessage).Object;
			var logger = new Mock<ILogger<PokemonApiClient>>();
			var cacheManager = new Mock<ICacheManager>();
			var pokeMonClient = new PokemonApiClient(factory, cacheManager.Object, logger.Object);

			// Act
			var result = await pokeMonClient.GetPokemonAsync(name);
			
			// Assert
			Assert.Equal(httpStatusCode, result.ErrorResult.StatusCode);
			Assert.NotNull(result.ErrorResult.ErrorMessage);
			Assert.True(result.Failed);
			Assert.Equal(expectedStatusCode, (int)result.ErrorResult.StatusCode);
		}

	}
}
