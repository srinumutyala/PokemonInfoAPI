namespace PokemonInfo.Services.Cache
{
	public interface ICacheManager
	{
		bool TryGetValue<T>(string key, out T cache);

		void Set<T>(string key, T data);
	}
}
