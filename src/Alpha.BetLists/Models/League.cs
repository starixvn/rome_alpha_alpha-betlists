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

        public int SportType { get; set; }

        public int LeagueId { get; set; }

        public string LeagueName { get; set; }

        public int LeagueGroupId { get; set; }
    }
}
