namespace Alpha.BetLists.Criteria
{
    using Fanex.Data.Repository;

    public class GetMixParlayDetailCriteria : CriteriaBase
    {
        public GetMixParlayDetailCriteria(
            long transId,
            string language)
        {
            TransId = transId;
            Language = language;
        }

        public long TransId { get; }

        public string Language { get; }

        public override string GetSettingKey() => "Age_BetList_SelParlayDetail";

        public override bool IsValid() => TransId > 0;
    }
}