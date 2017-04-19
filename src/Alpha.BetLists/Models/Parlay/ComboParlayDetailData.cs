namespace Alpha.BetLists.Models.Parlay
{
    using System.Collections.Generic;

    public class ComboParlayDetailData
    {
        public ComboParlayDetailData(
            IEnumerable<ComboParlayDetail> comboParlayDetails,
            IEnumerable<MatchDetail> matchDetails,
            decimal outstanding)
        {
            ComboParlayDetails = comboParlayDetails;
            MatchDetails = matchDetails;
            Outstanding = outstanding;
        }

        public IEnumerable<ComboParlayDetail> ComboParlayDetails { get; }

        public IEnumerable<MatchDetail> MatchDetails { get; }

        public decimal Outstanding { get; }
    }
}