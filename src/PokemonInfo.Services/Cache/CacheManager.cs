using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;

namespace PokemonInfo.Services.Cache
{
	public class CacheManager : ICacheManager
	{
        private readonly IMemoryCache _memoryCache;
        private readonly MemoryCacheEntryOptions _cacheExpirationOptions;

		public CacheManager(IMemoryCache memoryCache, IOptions<Entities.MemoryCacheOptions> cacheExpirationOptions)
		{
			_memoryCache = memoryCache;
            _cacheExpirationOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(cacheExpirationOptions.Value.SlidingExpiration))
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(cacheExpirationOptions.Value.AbsoluteExpiration));
		}

		public void Set<T>(string key, T cache)
        {
            _memoryCache.Set(key, cache, _cacheExpirationOptions);
        }

        public bool TryGetValue<T>(string Key, out T cache)
        {
            if (_memoryCache.TryGetValue(Key, out T cachedItem))
            {
                cache = cachedItem;
                return true;
            }
            cache = default(T);
            return false;
        }
        
    }
}
