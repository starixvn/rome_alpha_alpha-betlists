namespace Alpha.BetLists.Criteria.BetBuilder
{
    using Fanex.Data.Repository;

    public class GetBetBuilderDetailCriteria : CriteriaBase
    {
        public GetBetBuilderDetailCriteria(long custId, long transId)
        {
            CustId = custId;
            TransId = transId;
        }

        public long CustId { get; }

        public long TransId { get; }

        public override string GetSettingKey() => "Acc_BetList_BetBuilder_Detail";

        public override bool IsValid() => CustId > 0 && TransId > 0;
    }
}