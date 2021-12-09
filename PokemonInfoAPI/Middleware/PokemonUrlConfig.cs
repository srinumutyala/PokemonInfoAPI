using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonInfoAPI.Middleware
{
	public class PokemonUrlConfig
	{
        public Uri PokemonApiUrl { get; set; }
        public Uri TranslationApiUrl { get; set; }
    }
}
