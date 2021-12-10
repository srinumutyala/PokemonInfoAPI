using PokemonInfo.Entities;
using System.Threading.Tasks;

namespace PokemonInfo.Services
{
	public interface IPokemonApiClient
	{
		Task<Result<Pokemon>> GetPokemonAsync(string name);
	}
}
