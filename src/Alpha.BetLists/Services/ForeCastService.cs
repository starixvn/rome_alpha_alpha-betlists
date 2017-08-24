namespace Alpha.BetLists.Services
{
    using Criteria;
    using Extensions;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface ForecastService
    {
        IEnumerable<int> GetBetTypeIdsSupportScoreMap();
    }

    public class ForecastServiceImpl : ForecastService
    {
        private const int ScoreMapBetTypesCacheTimeInHours = 2;
        private const string ScoreMapBetTypesCacheKey = "Alpha.BetLists.Services.ForeCastServiceImpl.ScoreMapBetTypes";
        private readonly IDynamicRepository dynamicRepository;

        public ForecastServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public IEnumerable<int> GetBetTypeIdsSupportScoreMap()
        {
            var scoreMaps = GetScoreMaps();
            var bettypeIds = scoreMaps.Select(s => s.FirstId.ToListInt()).SelectMany(x => x).Distinct().ToList();

            return bettypeIds;
        }

        private IEnumerable<ScoreMap> GetScoreMaps()
        {
            var scoreMaps = ConfiguredCacheService
                .GetOrSet(
                ScoreMapBetTypesCacheKey,
                () => dynamicRepository.Fetch<ScoreMap>(new GetScoreMapBetTypesCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromHours(ScoreMapBetTypesCacheTimeInHours)));

            return scoreMaps;
        }
    }
}
