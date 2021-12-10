using PokemonInfoAPI.Integration.Tests.Utils;
using System.Net;
using System.Threading.Tasks;
using WireMock.ResponseBuilders;
using Xunit;

namespace PokemonInfoAPI.Integration.Tests
{
    public class PokemonControllerTests: IClassFixture<TestServer>
    {
        private readonly TestServer _testServer;

        public PokemonControllerTests(TestServer testFixure)
        {
            _testServer= testFixure;
        }

        [Fact]
        public async Task GetPokemonInfo_WithValidName_ReturnSuccess()
        {
            // Arrange
            var name = "wormadam";
            var infoResponse= TestDataSetup.GetTestData("InfoResponse.json");
             _testServer.pokemonInfoMockApiServer.SetupPokemonInfo(name).RespondWith(Response.Create().WithSuccess().WithBody(infoResponse));
            var httpClient=_testServer.Server.CreateClient();

            // Act
            var response = await httpClient.GetAsync($"/Pokemon/{name}");
            
            //Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task GetPokemonInfo_WithInValidName_ReturnNotFound()
        {
            // Arrange
            var name = "wormadam1";
            var infoResponse = TestDataSetup.GetTestData("InfoResponse.json");
            _testServer.pokemonInfoMockApiServer.SetupPokemonInfo("wormadam").RespondWith(Response.Create().WithSuccess().WithBody(infoResponse));
            var httpClient = _testServer.Server.CreateClient();
            
            // Act
            var response = await httpClient.GetAsync($"/Pokemon/{name}");
            
            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound,response.StatusCode);
        }

        [Fact]
        public async Task GetPokemonYodaTranslatedDescription_WithGivenValidName_ReturnSuccess()
        {
            // Arrange
            var name = "wormadam";
            var infoResponse = TestDataSetup.GetTestData("InfoResponse.json");
            var translationResponse = TestDataSetup.GetTestData("YodaTranslationResponse.json");
            _testServer.pokemonInfoMockApiServer.SetupPokemonInfo(name).RespondWith(Response.Create().WithSuccess().WithBody(infoResponse));
            _testServer.pokemonTranslationAPIFixure.SetupPokemonInfo(name).RespondWith(Response.Create().WithSuccess().WithBody(translationResponse));
            var httpClient = _testServer.Server.CreateClient();
            
            // Act
            var response = await httpClient.GetAsync($"/Pokemon/translated/{name}");
            
            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task GetPokemonShakespeareTranslatedDescription_WithGivenValidName_ReturnSuccess()
        {
            // Arrange
            var name = "metwo";
            var infoResponse = TestDataSetup.GetTestData("InfoResponse1.json");
            var translationResponse = TestDataSetup.GetTestData("ShakespheareTranslation.json");
            _testServer.pokemonInfoMockApiServer.SetupPokemonInfo(name).RespondWith(Response.Create().WithSuccess().WithBody(infoResponse));
            _testServer.pokemonTranslationAPIFixure.SetupPokemonInfo(name).RespondWith(Response.Create().WithSuccess().WithBody(translationResponse));
            var httpClient = _testServer.Server.CreateClient();
            
            // Act
            var response = await httpClient.GetAsync($"/Pokemon/translated/{name}");
            
            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task NoTranslationFound_WithGivenName()
        {
            // Arrange
            var name = "ddd";
            var infoResponse = TestDataSetup.GetTestData("InfoResponse.json");
            _testServer.pokemonInfoMockApiServer.SetupPokemonInfo("wormadam").RespondWith(Response.Create().WithSuccess().WithBody(infoResponse));
            var httpClient = _testServer.Server.CreateClient();
            
            // Act
            var response = await httpClient.GetAsync($"/Pokemon/translated/{name}");
            
            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        public void Dispose()
        {
            _testServer.Dispose();
        }
    }
}