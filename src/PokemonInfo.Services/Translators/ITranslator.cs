using System.Threading.Tasks;

namespace PokemonInfo.Services
{
	public interface ITranslator
	{
		Task<Result<string>> Translate(string input);

		string TranslatorType { get; }
	}
}
