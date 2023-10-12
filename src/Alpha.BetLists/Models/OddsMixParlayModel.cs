namespace Alpha.BetLists.Models
{
    public class OddsMixParlayModel
    {
        public OddsMixParlayModel()
        {
        }

        public OddsMixParlayModel(long transId, decimal odds)
            => (TransId, Odds) = (transId, odds);

        public long TransId { get; }

        public decimal Odds { get; }
    }
}
