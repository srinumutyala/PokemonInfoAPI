using Moq;
using PokemonInfo.Entities;
using PokemonInfo.Services.Cache;
using Xunit;

namespace PokemonInfoAPI.Unit.Tests
{
	public class CacheManagerTests
	{
		private Mock<ICacheManager> _cacheManagerMock;
		public CacheManagerTests()
		{
			_cacheManagerMock = new Mock<ICacheManager>();
		}

		[Fact]
		public void Cache_returns_value()
		{
			//Arrange
			_cacheManagerMock.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<PokemonInfoModel>()))
				.Callback<string, PokemonInfoModel>((key, cache) =>
				{
					_cacheManagerMock.Setup(s => s.TryGetValue(key, out cache))
						.Returns(true);
				});
			var expectedPokemonInfo = new PokemonInfoModel() { Name = "wordamam", IsLegendary = true };
			//Act
			_cacheManagerMock.Object.Set("testkey", expectedPokemonInfo);
			var cachedResult = _cacheManagerMock.Object.TryGetValue("testkey", out PokemonInfoModel actualPokemonInfo);

			//Assert
			Assert.Equal(expectedPokemonInfo, actualPokemonInfo);
		}

	}
}
