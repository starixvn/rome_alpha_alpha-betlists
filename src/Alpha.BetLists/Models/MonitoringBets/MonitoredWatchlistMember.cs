namespace Alpha.BetLists.Models.MonitoringBets
{
    public class MonitoredWatchlistMember
    {
        public MonitoredWatchlistMember(int custId, string username)
        {
            CustId = custId;
            Username = username;
        }

        public int CustId { get; }

        public string Username { get; }
    }
}
