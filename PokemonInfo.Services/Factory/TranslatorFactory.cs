using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PokemonInfo.Services.Factory
{
	public class TranslatorFactory : ITranslatorFactory
	{
		private readonly IEnumerable<ITranslator> _translators;

		//We need to register the ITranslator with multiple implementations into IOC container
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
