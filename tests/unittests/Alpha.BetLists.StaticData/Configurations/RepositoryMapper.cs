using System;
using System.Collections.Generic;
using Alpha.BetLists.Criteria;
using Alpha.BetLists.Criteria.MemberBetList;
using Alpha.BetLists.Criteria.RunningBetList;
using Alpha.BetLists.StaticData.DataAccesses;
using Alpha.IntegrationTest.Mapper;

namespace Alpha.BetLists.StaticData.Configurations
{
    public class RepositoryMapper : IRepositoryMapper
    {
        public IDictionary<string, Type> Maps
            => new Dictionary<string, Type>
            {
                [nameof(GetProductsCriteria)] = typeof(ProductExcelRepository),
                [nameof(GetScoreMapBetTypesCriteria)] = typeof(ForcastExcelRepository),
                [nameof(GetBetListRunningByDateCriteria)] = typeof(BetListExcelRepository),
                [nameof(GetBetListAllByDateCriteria)] = typeof(BetListExcelRepository)
            };
    }
}
