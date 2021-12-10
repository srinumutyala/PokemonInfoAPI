using System.Collections.Generic;
using System.Linq;

namespace PokemonInfo.Services.Factory
{
	public class TranslatorFactory : ITranslatorFactory
	{
		private readonly IEnumerable<ITranslator> _translators;

		public TranslatorFactory(IEnumerable<ITranslator> funtranslators)
		{
			_translators = funtranslators;
		}

		public ITranslator GetFunTranslatorService(string translationType)
		{
			return _translators.ToList().Find(t => t.TranslatorType.ToLower() == translationType.ToLower());
		}
	}
}
