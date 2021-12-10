using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PokemonInfo.Services
{
	public interface ITranslator
	{
		Task<Result<string>> Translate(string input);

		string TranslatorType { get; }
	}
}
