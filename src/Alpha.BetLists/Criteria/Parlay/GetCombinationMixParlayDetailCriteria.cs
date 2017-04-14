namespace Alpha.BetLists.Criteria.Parlay
{
    public class GetCombinationMixParlayDetailCriteria : GetMixParlayDetailCriteria
    {
        public GetCombinationMixParlayDetailCriteria(long transId, string language) : base(transId, language)
        {
        }

        public override string GetSettingKey() => "Age_BetList_SelParlayDetail";
    }
}