namespace Alpha.BetLists.Params
{
    using Alpha.BetLists.Models;

    public class FindTicketsParam
    {
        public FindTicketsParam(
            long refNo,
            int uplineId,
            UserLevel custLevel)        {
            RefNo = refNo;
            UplineId = uplineId;
            CustLevel = custLevel;
        }

        public long RefNo { get; }
        public int UplineId { get; }
        public UserLevel CustLevel { get; }
    }
}
