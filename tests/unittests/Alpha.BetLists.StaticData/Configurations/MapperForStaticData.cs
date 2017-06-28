using Alpha.BetLists.StaticData.DataAccesses;
using Alpha.IntegrationTest.Mapper;
using System;
using System.Collections.Generic;

namespace Alpha.BetLists.StaticData.Configurations
{
    public class MapperForStaticData : IMapperForStaticData
    {
        public IDictionary<string, string> ResultModelToExcelFile
            => new Dictionary<string, string>
            {
                ["Product"] = "Products",
                ["ScoreMap"] = "ScoreMaps"
            };

        public IDictionary<string, Type> CriteriaToRepository
            => new Dictionary<string, Type>
            {
                ["GetProductsCriteria"] = typeof(ProductExcelRepository),
                ["GetScoreMapBetTypesCriteria"] = typeof(ForcastExcelRepository),
                ["GetBetListRunningByDateCriteria"] = typeof(BetListExcelRepository),
                ["GetBetListAllByDateCriteria"] = typeof(BetListExcelRepository)
            };

        public IDictionary<string, string> CriteriaToExcelFile
            => new Dictionary<string, string>
            {
                ["GetBetListRunningByDateCriteria"] = "RunningBetlist",
                ["GetBetListAllByDateCriteria"] = "MemberBetlist"
            };
    }
}
