namespace PokemonInfo.Services.Factory
{
	public interface ITranslatorFactory
	{
		ITranslator GetFunTranslatorService(string translationType);
	}
}
