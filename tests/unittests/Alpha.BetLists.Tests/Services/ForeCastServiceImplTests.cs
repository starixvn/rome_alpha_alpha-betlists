namespace Alpha.BetLists.Tests.Services
{
    using BetLists.Criteria;
    using BetLists.Extensions;
    using BetLists.Models;
    using BetLists.Services;
    using Fanex.Caching;
    using Fixtures;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    [Trait("Category", "BaseFunction")]
    [Collection("FanexLibsCollection")]
    public class ForecastServiceImplTests
    {
        private const string ScoreMapBetTypesCacheKey = "Alpha.BetLists.Services.ForecastServiceImpl.ScoreMapBetTypes";

        private readonly FanexDataFixture fanexDataFixture;
        private readonly ForecastService forecastService;

        private readonly IEnumerable<ScoreMap> scoreMaps = new List<ScoreMap>
        {
            new ScoreMap(1, 0, "1,7,301,303", "1,7,301,303"),
            new ScoreMap(1, 1, "1,7", "1,7"),
            new ScoreMap(3, 1, "3,8", "3,8"),
            new ScoreMap(5, 0, "5,15", "5,15")
        };

        public ForecastServiceImplTests(
            FanexDataFixture fanexDataFixture)
        {
            this.fanexDataFixture = fanexDataFixture;

            forecastService = new ForecastServiceImpl(fanexDataFixture.DynamicRepository, fanexDataFixture.CacheService);
        }

        [Fact]
        public void GetBetTypeIdsSupportScoreMap_Always_ReturnsAllBetTypeIdsSupportScoreMap()
        {
            // Arrange
            var expectedBettypeIds = scoreMaps.Select(s => s.FirstId.ToListInt()).SelectMany(x => x).Distinct().ToList();
            fanexDataFixture.CacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<ScoreMap>>>(), Arg.Any<CacheItemOptions>()).Returns(x =>
            {
                var func = x.ArgAt<Func<IEnumerable<ScoreMap>>>(1);
                return func();
            });
            fanexDataFixture.DynamicRepository.Fetch<ScoreMap>(Arg.Any<GetScoreMapBetTypesCriteria>()).Returns(scoreMaps);

            // Act
            var actualBettypeIds = forecastService.GetBetTypeIdsSupportScoreMap();

            // Assert
            Assert.Equal(expectedBettypeIds, actualBettypeIds);
        }
    }
}
