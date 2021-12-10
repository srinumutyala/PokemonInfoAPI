using Microsoft.AspNetCore.Mvc;
using PokemonInfo.Entities;
using PokemonInfo.Services;
using System.Net;

namespace PokemonInfoAPI.Helpers
{
	public class StaticHelper
	{
		public static ContentResult HandleFailure(Result<Pokemon> result, string name )
		{
			if (result.StatusCode == HttpStatusCode.NotFound || result.StatusCode == HttpStatusCode.TooManyRequests)
			{
				return new ContentResult
				{
					StatusCode = (int?)result.StatusCode,
					Content = result.ErrorMessage
				};
			}

			return new ContentResult
			{
				StatusCode = (int?)HttpStatusCode.InternalServerError,
				Content = "There is an underlying service problem."
			};
		}
	}
}
