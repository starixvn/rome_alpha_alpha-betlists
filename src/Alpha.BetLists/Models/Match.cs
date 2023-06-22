namespace Alpha.BetLists.Models
{
    using System;
    using Alpha.BetLists.Extensions;

    public class Match
    {
        public Match()
        {
        }

        public Match(
            int sportId,
            int leagueId,
            int matchId,
            int homeId,
            int awayId,
            bool isNeutral,
            DateTime? kickOffTime = null)
        {
            SportId = sportId;
            LeagueId = leagueId;
            MatchId = matchId;
            HomeId = homeId;
            AwayId = awayId;
            IsNeutral = isNeutral;
            KickOffTime = kickOffTime;
        }

        public int SportId { get; }

        public int LeagueId { get; }

        public int MatchId { get; }

        public string MatchName { get; set; }

        public int HomeId { get; }

        public int AwayId { get; }

        public bool IsNeutral { get; }

        public DateTime? KickOffTime { get; }

        public string KickOffTimeStr
            => KickOffTime.HasValue ? KickOffTime.Value.ToLongDateTimeWithSlash() : string.Empty;
    }
}
