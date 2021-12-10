using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PokemonInfoAPI.Middleware;

namespace PokemonInfoAPI
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddPokemonServices();
			services.AddTranslatorServices();

			services.Configure<PokemonInfo.Entities.MemoryCacheOptions>(Configuration.GetSection("MemoryCacheConfiguration"));
			services.AddMemoryCache();

			var pokemonUrlConfig = Configuration.GetSection("PokemonUrlConfig").Get<PokemonUrlConfig>();
			services.AddHttpClients(pokemonUrlConfig);

			services.AddControllers(
				options =>
					options.Filters.Add<PokemonInfoExceptionFilterAttribute>()
			);

			services.AddSwaggerGen(config => {
				config.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "Pokemontranslation" });
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			app.UseSwagger();
			app.UseSwaggerUI(config =>
			{
				config.SwaggerEndpoint("/swagger/v1/swagger.json", "Pokemontranslation");
			});
			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
