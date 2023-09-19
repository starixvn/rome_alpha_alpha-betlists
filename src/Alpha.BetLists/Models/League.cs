namespace Alpha.BetLists.Models
{
    public class League
    {
        public League()
        {
        }

        public League(int sportType, int leagueId, string leagueName = null)
        {
            SportType = sportType;
            LeagueId = leagueId;
            LeagueName = leagueName;
        }

        public int SportType { get; }

        public int LeagueId { get; }

        public string LeagueName { get; }
    }
}
