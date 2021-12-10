using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PokemonInfo.Services.Cache
{
	

	public interface ICacheManager
	{
		bool TryGetValue<T>(string key, out T cache);

		void Set<T>(string key, T data);
	}
}
