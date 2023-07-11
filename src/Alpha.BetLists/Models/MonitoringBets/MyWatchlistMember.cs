namespace Alpha.BetLists.Models.MonitoringBets
{
    public class MyWatchlistMember
    {
        public MyWatchlistMember(int custId, string username)
        {
            CustId = custId;
            Username = username;
        }

        public int CustId { get; }

        public string Username { get; }
    }
}
