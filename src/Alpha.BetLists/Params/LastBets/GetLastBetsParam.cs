namespace Alpha.BetLists.Params.LastBets
{
    public class GetLastBetsParam
    {
        public GetLastBetsParam(
            int betType,
            int custId,
            long transId,
            int pageSize)
        {
            BetType = betType;
            CustId = custId;
            TransId = transId;
            PageSize = pageSize;
        }

        public int BetType { get; }

        public int CustId { get; }

        public long TransId { get; }

        public int PageSize { get; }
    }
}
