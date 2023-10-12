namespace Alpha.BetLists.Criteria
{
    using Fanex.Data.Repository;
    using System.Collections.Generic;

    public class GetOddsOfMixParlayCriteria : CriteriaBase
    {
        public GetOddsOfMixParlayCriteria(IEnumerable<long> transids)
            => TransIdList = string.Join(",", transids);

        public string TransIdList { get; set; }

        public override string GetSettingKey() => "Age_BetList_MixParlay_OddsSel";

        public override bool IsValid() => !string.IsNullOrEmpty(TransIdList);
    }
}