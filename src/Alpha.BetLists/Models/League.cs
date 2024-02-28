namespace Alpha.BetLists.Models
{
    public class League
    {
        public League()
        {
        }

        public League(int sportType, int leagueId, int leagueGroupId = 0, string leagueName = null)
        {
            SportType = sportType;
            LeagueId = leagueId;
            LeagueName = leagueName;
            LeagueGroupId = leagueGroupId;
        }

        public int SportType { get; }

        public int LeagueId { get; }

        public string LeagueName { get; }

        public int LeagueGroupId { get; }
    }
}
