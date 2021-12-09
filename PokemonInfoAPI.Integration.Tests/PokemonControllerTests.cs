using PokemonInfoAPI.Integration.Tests.Utils;
using System.Net;
using System.Threading.Tasks;
using WireMock.ResponseBuilders;
using Xunit;

namespace PokemonInfoAPI.Integration.Tests
{
    public class PokemonControllerTests: IClassFixture<TestFixure>
    {
        private readonly TestFixure _testFixure;

        public PokemonControllerTests(TestFixure testFixure)
        {
            _testFixure= testFixure;
        }

        [Fact]
        public async Task GetPokemonInfo_WithValidName_ReturnSuccess()
        {
            var name = "wormadam";
            var infoResponse= TestDataSetup.GetTestData("InfoResponse.json");
             _testFixure.pokemonInfoMockApiServer.SetupPokemonInfo(name).RespondWith(Response.Create().WithSuccess().WithBody(infoResponse));
            var httpClient=_testFixure.Server.CreateClient();
            var response = await httpClient.GetAsync($"/Pokemon/{name}");
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task GetPokemonInfo_WithInValidName_ReturnNotFound()
        {
            var name = "wormadam1";
            var infoResponse = TestDataSetup.GetTestData("InfoResponse.json");
            _testFixure.pokemonInfoMockApiServer.SetupPokemonInfo("wormadam").RespondWith(Response.Create().WithSuccess().WithBody(infoResponse));
            var httpClient = _testFixure.Server.CreateClient();
            var response = await httpClient.GetAsync($"/Pokemon/{name}");
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound,response.StatusCode);
        }

        [Fact]
        public async Task GetPokemonYodaTranslatedDescription_WithGivenValidName_ReturnSuccess()
        {
            var name = "wormadam";
            var infoResponse = TestDataSetup.GetTestData("InfoResponse.json");
            var translationResponse = TestDataSetup.GetTestData("YodaTranslationResponse.json");
            _testFixure.pokemonInfoMockApiServer.SetupPokemonInfo(name).RespondWith(Response.Create().WithSuccess().WithBody(infoResponse));
            _testFixure.pokemonTranslationAPIFixure.SetupPokemonInfo(name).RespondWith(Response.Create().WithSuccess().WithBody(translationResponse));
            var httpClient = _testFixure.Server.CreateClient();
            var response = await httpClient.GetAsync($"/Pokemon/translated/{name}");
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task GetPokemonShakespeareTranslatedDescription_WithGivenValidName_ReturnSuccess()
        {
            var name = "metwo";
            var infoResponse = TestDataSetup.GetTestData("InfoResponse1.json");
            var translationResponse = TestDataSetup.GetTestData("ShakespheareTranslation.json");
            _testFixure.pokemonInfoMockApiServer.SetupPokemonInfo(name).RespondWith(Response.Create().WithSuccess().WithBody(infoResponse));
            _testFixure.pokemonTranslationAPIFixure.SetupPokemonInfo(name).RespondWith(Response.Create().WithSuccess().WithBody(translationResponse));
            var httpClient = _testFixure.Server.CreateClient();
            var response = await httpClient.GetAsync($"/Pokemon/translated/{name}");
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task NoTranslationFound_WithGivenName()
        {
            var name = "ddd";
            var infoResponse = TestDataSetup.GetTestData("InfoResponse.json");
            _testFixure.pokemonInfoMockApiServer.SetupPokemonInfo("wormadam").RespondWith(Response.Create().WithSuccess().WithBody(infoResponse));
            var httpClient = _testFixure.Server.CreateClient();
            var response = await httpClient.GetAsync($"/Pokemon/translated/{name}");
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        public void Dispose()
        {
            _testFixure.Dispose();
        }
    }
}