namespace Alpha.BetLists.Models.MonitoringBets
{
    public class MemberErrCode : MyWatchlistMember
    {
        public MemberErrCode(int custId, string username, int errCode) : base(custId, username)
            => ErrCode = errCode;

        public int ErrCode { get; }

        public string ErrMsg { get; set; }
    }
}
