using System;
using System.Linq;
using WireMock.RequestBuilders;
using WireMock.Server;
using WireMock.Util;

namespace PokemonInfoAPI.Integration.Tests
{
    public class PokemonInfoMockApiServer
    {
        public WireMockServer PokemonInfoApi;

        public PokemonInfoMockApiServer()
        {
            PokemonInfoApi=WireMockServer.Start(PortUtils.FindFreeTcpPort());
        }

        public int Port => PokemonInfoApi.Ports.First();

        public IRespondWithAProvider SetupPokemonInfo(string name)
            => PokemonInfoApi.Given(Request.Create().WithPath($"/{name}").UsingGet());

        public void Reset()
        {
            PokemonInfoApi.Reset();
        }

        public void Dispose()
        {
            PokemonInfoApi.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
