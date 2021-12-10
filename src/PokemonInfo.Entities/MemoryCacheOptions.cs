namespace PokemonInfo.Entities
{
	public class MemoryCacheOptions
	{
        public int SlidingExpiration { get; set; }

        public int AbsoluteExpiration { get; set; }
    }
}
