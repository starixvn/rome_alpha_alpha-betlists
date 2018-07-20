namespace Alpha.BetLists.Params.LastBets
{
    public class GetLastBetsParam
    {
        public GetLastBetsParam(
            int betType,
            int custId,
            long transId,
            int pageSize,
            int runningMatchCacheTime)
        {
            BetType = betType;
            CustId = custId;
            TransId = transId;
            PageSize = pageSize;
            RunningMatchCacheTime = runningMatchCacheTime;
        }

        public int BetType { get; }

        public int CustId { get; }

        public long TransId { get; }

        public int PageSize { get; }

        public int RunningMatchCacheTime { get; }
    }
}
