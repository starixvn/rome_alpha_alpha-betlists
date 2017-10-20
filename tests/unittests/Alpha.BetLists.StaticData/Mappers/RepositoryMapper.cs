namespace Alpha.BetLists.StaticData.Mappers
{
    using System;
    using System.Collections.Generic;
    using Alpha.StaticData.Mappers;
    using Criteria;
    using Criteria.MemberBetList;
    using Criteria.RunningBetList;
    using DataAccesses;

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
