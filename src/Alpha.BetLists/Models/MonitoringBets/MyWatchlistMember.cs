namespace Alpha.BetLists.Models.MonitoringBets
{
    public class MyWatchlistMember
    {
        public MyWatchlistMember(int custId, string username, string username2)
        {
            CustId = custId;
            Username = username;
            Username2 = username2;
        }

        public int CustId { get; }

        public string Username { get; }

        public string Username2 { get; }
    }
}
