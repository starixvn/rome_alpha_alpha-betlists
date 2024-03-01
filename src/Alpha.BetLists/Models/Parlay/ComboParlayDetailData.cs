namespace Alpha.BetLists.Models.Parlay
{
    using System.Collections.Generic;

    public class ComboParlayDetailData
    {
        public ComboParlayDetailData(
            IEnumerable<ComboParlayDetail> comboParlayDetails,
            IEnumerable<MatchDetail> matchDetails,
            decimal outstanding,
            bool isParlayBoost,
            bool isMixParlay)
        {
            ComboParlayDetails = comboParlayDetails;
            MatchDetails = matchDetails;
            Outstanding = outstanding;
            IsParlayBoost = isParlayBoost;
            IsMixParlay = isMixParlay;
        }

        public IEnumerable<ComboParlayDetail> ComboParlayDetails { get; }

        public IEnumerable<MatchDetail> MatchDetails { get; }

        public decimal Outstanding { get; }

        public bool IsParlayBoost { get; }

        public bool IsMixParlay { get; }
    }
}