using Alpha.BetLists.AppResources;

namespace Alpha.BetLists.Models.CancelledBet
{
    public class UserBetCount
    {
        public UserBetCount(int custId, string userName, int betCount)
        {
            CustId = custId;
            UserName = userName;
            BetCount = betCount;
        }

        public int CustId { get; }

        public string UserName { get; }

        public int BetCount { get; }

        public override string ToString() => $"{UserName} ({BetCount} {Resources.Bets})";
    }
}
