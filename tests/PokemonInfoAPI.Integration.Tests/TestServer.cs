using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace PokemonInfoAPI.Integration.Tests
{
    public class TestServer: WebApplicationFactory<Startup>,IDisposable
    {
        public PokemonInfoMockApiServer pokemonInfoMockApiServer;

        public PokemonTranslationAPIFixure pokemonTranslationAPIFixure;

        public TestServer()
        {
            pokemonInfoMockApiServer = new PokemonInfoMockApiServer();
            pokemonTranslationAPIFixure = new PokemonTranslationAPIFixure();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            var configuration = new Dictionary<string, string>();
            configuration.Add("PokemonUrlConfig:PokemonApiUrl", $"http://localhost:{pokemonInfoMockApiServer.Port}");
            configuration.Add("PokemonUrlConfig:TranslationApiUrl", $"http://localhost:{pokemonTranslationAPIFixure.Port}");

            builder.ConfigureAppConfiguration(webBuilder =>
            {
                webBuilder.AddInMemoryCollection(configuration);
            });
        }

        public new void Dispose()
        {
            base.Dispose();
            pokemonInfoMockApiServer.Dispose();
            pokemonTranslationAPIFixure.Dispose();
        }
    }
}
