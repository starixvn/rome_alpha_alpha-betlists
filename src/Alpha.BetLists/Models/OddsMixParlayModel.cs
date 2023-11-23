namespace Alpha.BetLists.Models
{
    public class OddsMixParlayModel
    {
        public OddsMixParlayModel()
        {
        }

        public OddsMixParlayModel(long transId, decimal odds, decimal masterOdds)
            => (TransId, Odds, MasterOdds) = (transId, odds, masterOdds);

        public long TransId { get; set; }

        public decimal Odds { get; set; }

        public decimal MasterOdds { get; set; }
    }
}
