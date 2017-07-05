namespace Alpha.BetLists.Tests.Services
{
    using BetLists.Criteria;
    using BetLists.Extensions;
    using BetLists.Models;
    using BetLists.Services;
    using Fixtures;
    using NSubstitute;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    [Trait("Category", "BaseFunction")]
    [Collection("FanexLibsCollection")]
    public class ForecastServiceImplTests
    {
        private const string ScoreMapBetTypesCacheKey = "Alpha.BetLists.Services.ForeCastServiceImpl.ScoreMapBetTypes";

        private readonly FanexCacheFixture fanexCacheFixture;
        private readonly FanexDataFixture fanexDataFixture;
        private readonly ForecastService foreCastService;

        private readonly IEnumerable<ScoreMap> scoreMaps = new List<ScoreMap>
        {
            new ScoreMap(1, 0, "1,7,301,303", "1,7,301,303"),
            new ScoreMap(1, 1, "1,7", "1,7"),
            new ScoreMap(3, 1, "3,8", "3,8"),
            new ScoreMap(5, 0, "5,15", "5,15")
        };

        public ForecastServiceImplTests(
            FanexCacheFixture fanexCacheFixture,
            FanexDataFixture fanexDataFixture)
        {
            this.fanexCacheFixture = fanexCacheFixture;
            this.fanexDataFixture = fanexDataFixture;

            foreCastService = new ForecastServiceImpl(fanexDataFixture.DynamicRepository);
        }

        [Fact]
        public void GetBetTypeIdsSupportScoreMap_Always_ReturnsAllBetTypeIdsSupportScoreMap()
        {
            // Arrange
            var expectedBettypeIds = scoreMaps.Select(s => s.FirstId.ToListInt()).SelectMany(x => x).Distinct().ToList();

            IEnumerable<ScoreMap> cachedScoreMaps;
            fanexCacheFixture.CacheStorage.TryGet($"{ScoreMapBetTypesCacheKey}", out cachedScoreMaps).Returns(false);

            fanexDataFixture.DynamicRepository.Fetch<ScoreMap>(Arg.Any<GetScoreMapBetTypesCriteria>()).Returns(scoreMaps);

            // Act
            var actualBettypeIds = foreCastService.GetBetTypeIdsSupportScoreMap();

            // Assert
            Assert.Equal(expectedBettypeIds, actualBettypeIds);
        }
    }
}
