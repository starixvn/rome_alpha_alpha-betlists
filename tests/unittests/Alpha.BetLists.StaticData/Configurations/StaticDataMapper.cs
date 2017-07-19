using System;
using System.Collections.Generic;
using Alpha.BetLists.Criteria.MemberBetList;
using Alpha.BetLists.Criteria.RunningBetList;
using Alpha.BetLists.Models;

namespace Alpha.BetLists.StaticData.Configurations
{
    public static class StaticDataMapper
    {
        [ThreadStatic]
        private static IDictionary<string, string> modelMaps;

        [ThreadStatic]
        private static IDictionary<string, string> criteriaMaps;

        public static IDictionary<string, string> ModelMaps
        {
            get
            {
                if (modelMaps == null)
                {
                    modelMaps = GetModelMaps();
                }

                return modelMaps;
            }

            set { modelMaps = value; }
        }

        public static IDictionary<string, string> CriteriaMaps
        {
            get
            {
                if (criteriaMaps == null)
                {
                    criteriaMaps = GetCriteriaMaps();
                }

                return criteriaMaps;
            }

            set { criteriaMaps = value; }
        }

        private static IDictionary<string, string> GetModelMaps() => new Dictionary<string, string>
        {
            [nameof(Product)] = "Products",
            [nameof(ScoreMap)] = "ScoreMaps"
        };

        private static IDictionary<string, string> GetCriteriaMaps() => new Dictionary<string, string>
        {
            [nameof(GetBetListRunningByDateCriteria)] = "RunningBetlist",
            [nameof(GetBetListAllByDateCriteria)] = "MemberBetlist"
        };
    }
}
