using Moq;
using PokemonInfo.Entities;
using PokemonInfo.Unit.Tests;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PokemonInfo.Services.Tests
{
	public class TestHelper
	{
		public static PokemonInfoModel GetPokemon(string name, string description, string language, string habitat = "", bool islegendary = false)
		{

			var pokemonInfoObj = new PokemonInfoModel()
			{
				Name = name,
				TextEntries = new List<Flavor_Text_Entries>()
				{
					new Flavor_Text_Entries(){
					FlavorText = description,
					Language = new Language() { Name = language }
				}
				}.ToArray(),
				IsLegendary = islegendary,
				Habitat = new Habitat() { Name = habitat }
			};
			return pokemonInfoObj;
		}

		public static Mock<IHttpClientFactory> GetHttpFactoryMock(HttpStatusCode httpStatusCode, object expectedPokemon)
		{
			var mockFactory = new Mock<IHttpClientFactory>();
			var configuration = new HttpConfiguration();

			var clientHandlerStub = new DelegatingHandlerStub((request, cancellationToken) =>
			{
				request.SetConfiguration(configuration);
				var response = request.CreateResponse(httpStatusCode, expectedPokemon);
				return Task.FromResult(response);
			});

			var client = new HttpClient(clientHandlerStub);
			client.BaseAddress = new Uri("http://example.com");

			mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

			IHttpClientFactory factory = mockFactory.Object;
			return mockFactory;
		}
	}
}
