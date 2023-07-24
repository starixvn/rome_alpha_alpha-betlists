namespace Alpha.BetLists.Models.MonitoringBets
{
    public class MemberErrCode
    {
        public MemberErrCode(int custId, string username, int errCode)
        {
            CustId = custId;
            Username = username;
            ErrCode = errCode;
        }

        public int CustId { get; }

        public string Username { get; }

        public int ErrCode { get; }

        public string ErrMsg { get; set; }
    }
}
