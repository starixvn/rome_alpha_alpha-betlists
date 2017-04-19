namespace Alpha.BetLists.Models.Parlay
{
    public class MatchDetail
    {
        public MatchDetail(long serial, int homeId, int awayId)
        {
            Serial = serial;
            HomeId = homeId;
            AwayId = awayId;
        }

        public long Serial { get; }

        public int HomeId { get; }

        public int AwayId { get; }
    }
}