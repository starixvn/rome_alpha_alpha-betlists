namespace Alpha.BetLists.Models.Parlay
{
    public class MatchDetail
    {
        public MatchDetail()
        {
        }

        public MatchDetail(long serial, int homeId, int awayId, bool isNeutral)
        {
            Serial = serial;
            HomeId = homeId;
            AwayId = awayId;
            IsNeutral = isNeutral;
        }

        public long Serial { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public bool IsNeutral { get; set; }
    }
}