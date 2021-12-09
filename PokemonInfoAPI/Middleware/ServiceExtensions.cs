using Microsoft.Extensions.DependencyInjection;
using PokemonInfo.Services;
using PokemonInfo.Services.Cache;
using PokemonInfo.Services.Clients;
using PokemonInfo.Services.Factory;

namespace PokemonInfoAPI.Middleware
{
	public static class ServiceExtensions
	{
		//TODO: These methods can go into their own libraries (assuming there are more libraries),
		//that way when there a any change
		//we done need to buid the actual application. Building that separate library and updating 
		//the services method on the same library is enough.
		public static void AddPokemonServices(this IServiceCollection services)
		{
			services.AddTransient<IPokemonApiClient, PokemonApiClient>();
			services.AddScoped<ICacheManager, CacheManager>();
		}

		public static void AddTranslatorServices(this IServiceCollection services)
		{
			services.AddTransient<ITranslatorFactory, TranslatorFactory>();
			services.AddTransient<ITranslator, ShakespeareTranslator>();
			services.AddTransient<ITranslator, YodaTranslator>();
		}
		public static void AddHttpClients(this IServiceCollection services, PokemonUrlConfig config)
		{
			services.AddHttpClient("Pokemon", c =>
			{
				c.BaseAddress = config.PokemonApiUrl;
			});
			services.AddHttpClient("Translation", c =>
			{
				c.BaseAddress = config.TranslationApiUrl;
			});
		}
	}
}
