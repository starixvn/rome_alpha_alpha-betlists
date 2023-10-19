namespace Alpha.BetLists.Models
{
    public class OddsMixParlayModel
    {
        public OddsMixParlayModel()
        {
        }

        public OddsMixParlayModel(long transId, decimal odds, decimal masterOdds)
            => (TransId, Odds, MasterOdds) = (transId, odds, masterOdds);

        public long TransId { get; }

        public decimal Odds { get; }

        public decimal MasterOdds { get; }
    }
}
