using PokemonInfo.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PokemonInfo.Services
{
	public interface IPokemonApiClient
	{
		Task<Result<Pokemon>> GetPokemonAsync(string name);
	}
}
