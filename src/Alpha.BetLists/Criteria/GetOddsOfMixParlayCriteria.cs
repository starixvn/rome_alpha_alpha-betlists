namespace Alpha.BetLists.Criteria
{
    using Fanex.Data.Repository;

    public class GetOddsOfMixParlayCriteria : CriteriaBase
    {
        public GetOddsOfMixParlayCriteria(string transIdList)
            => TransIdList = transIdList;

        public string TransIdList { get; }

        public override string GetSettingKey() => "Age_BetList_MixParlay_OddsSel";

        public override bool IsValid() => !string.IsNullOrEmpty(TransIdList);
    }
}