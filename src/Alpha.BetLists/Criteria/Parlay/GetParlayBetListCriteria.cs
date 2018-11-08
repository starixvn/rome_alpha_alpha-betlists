namespace Alpha.BetLists.Criteria.Parlay
{
    using Fanex.Data.Repository;

    public class GetParlayBetListCriteria : CriteriaBase
    {
        public GetParlayBetListCriteria(
            long transId,
            int custId)
        {
            TransId = transId;
            CustId = custId;
        }

        public long TransId { get; }

        public int CustId { get; }

        public override string GetSettingKey() => "Age_BetList_SelParlayDetail";

        public override bool IsValid() => TransId > 0 && CustId > 0;
    }
}