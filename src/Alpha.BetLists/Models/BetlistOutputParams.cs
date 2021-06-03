namespace Alpha.BetLists.Models
{
    public class BetlistOutputParams
    {
        public BetlistOutputParams()
        {
        }

        public BetlistOutputParams(int errCode, string errMsg, int maxTotalRow)
        {
            ErrCode = errCode;
            ErrMsg = errMsg;
            MaxTotalRow = maxTotalRow;
        }

        public int ErrCode { get; }

        public string ErrMsg { get; }

        public int MaxTotalRow { get; }
    }
}