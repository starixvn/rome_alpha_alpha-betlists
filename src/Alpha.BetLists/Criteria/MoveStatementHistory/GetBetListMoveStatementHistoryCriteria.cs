namespace Alpha.BetLists.Criteria.MoveStatementHistory
{
    using Fanex.Data.Repository;

    public class GetBetListMoveStatementHistoryCriteria : CriteriaBase
    {
        public GetBetListMoveStatementHistoryCriteria(int custId, long transId)
        {
            CustId = custId;
            TransId = transId;
        }

        public int CustId { get; }

        public long TransId { get; }

        public override string GetSettingKey() => "Age_BetList_SelMovingEventByTransId";

        public override bool IsValid() => CustId > 0 && TransId > 0;
    }
}
