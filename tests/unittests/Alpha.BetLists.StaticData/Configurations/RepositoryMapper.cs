using System;
using System.Collections.Generic;
using Alpha.BetLists.StaticData.DataAccesses;
using Alpha.IntegrationTest.Mapper;

namespace Alpha.BetLists.StaticData.Configurations
{
    public class RepositoryMapper : IRepositoryMapper
    {
        public IDictionary<string, Type> Maps
            => new Dictionary<string, Type>
            {
                ["GetProductsCriteria"] = typeof(ProductExcelRepository),
                ["GetScoreMapBetTypesCriteria"] = typeof(ForcastExcelRepository),
                ["GetBetListRunningByDateCriteria"] = typeof(BetListExcelRepository),
                ["GetBetListAllByDateCriteria"] = typeof(BetListExcelRepository)
            };
    }
}
