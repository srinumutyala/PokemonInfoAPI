using System;
using System.Linq;
using WireMock.RequestBuilders;
using WireMock.Server;
using WireMock.Util;

namespace PokemonInfoAPI.Integration.Tests
{
    public class PokemonTranslationAPIFixure
    {
        public WireMockServer PokemonTranslationApi;

        public PokemonTranslationAPIFixure()
        {
            PokemonTranslationApi = WireMockServer.Start(PortUtils.FindFreeTcpPort());
        }

        public int Port => PokemonTranslationApi.Ports.First();

        public IRespondWithAProvider SetupPokemonInfo(string name)
            => PokemonTranslationApi.Given(Request.Create().WithPath($"/{name}").UsingGet());

        public void Reset()
        {
            PokemonTranslationApi.Reset();
        }

        public void Dispose()
        {
            PokemonTranslationApi.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
