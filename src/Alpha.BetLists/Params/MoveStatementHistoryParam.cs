namespace Alpha.BetLists.Params
{
    public class MoveStatementHistoryParam
    {
        public MoveStatementHistoryParam(int custId, long transId)
        {
            CustId = custId;
            TransId = transId;
        }

        public int CustId { get; }

        public long TransId { get; }
    }
}
