namespace Alpha.BetLists.StaticData.Mappers
{
    using System;
    using System.Collections.Generic;
    using BetLists.Models;
    using Criteria.MemberBetList;
    using Criteria.RunningBetList;

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
                    modelMaps = FromExpectedModelToExcel();
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
                    criteriaMaps = FromCriteriaToExcel();
                }

                return criteriaMaps;
            }

            set { criteriaMaps = value; }
        }

        private static IDictionary<string, string> FromExpectedModelToExcel() => new Dictionary<string, string>
        {
            [nameof(Product)] = "Products",
            [nameof(ScoreMap)] = "ScoreMaps"
        };

        private static IDictionary<string, string> FromCriteriaToExcel() => new Dictionary<string, string>
        {
            [nameof(GetBetListRunningByDateCriteria)] = "RunningBetlist",
            [nameof(GetBetListAllByDateCriteria)] = "MemberBetlist"
        };
    }
}
