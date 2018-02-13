namespace Alpha.BetLists.Params
{
    public class JackpotDetailsParam
    {
        public JackpotDetailsParam(
            long transId,
            int custId,
            int betType)
        {
            TransId = transId;
            CustId = custId;
            BetType = betType;
        }

        public long TransId { get; }

        public int CustId { get; }

        public int BetType { get; }
    }
}