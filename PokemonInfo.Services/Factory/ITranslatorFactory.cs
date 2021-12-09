using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonInfo.Services.Factory
{
	public interface ITranslatorFactory
	{
		ITranslator GetFunTranslatorService(string translationType);
	}
}
