namespace Alpha.BetLists.Models
{
    using System;

    public class MatchInfo
    {
        public MatchInfo()
        {
        }

        public MatchInfo(
            int sportType,
            string status,
            int matchId,
            int leagueId,
            string matchCode,
            int homeId,
            int awayId,
            string race,
            string showTime,
            byte settlementStatus,
            DateTime eventDate,
            int leagueGroupId)
            : this(sportType, status, matchId, leagueId, matchCode, homeId, awayId, race, showTime, false, string.Empty, settlementStatus, eventDate, leagueGroupId)
        {
        }

        public MatchInfo(
            int sportType,
            string status,
            int matchId,
            int leagueId,
            string matchCode,
            int homeId,
            int awayId,
            string race,
            string showTime,
            bool isNeutral,
            string betStatus,
            byte settlementStatus,
            DateTime eventDate,
            int leagueGroupId)
        {
            SportType = sportType;
            Status = status;
            MatchId = matchId;
            LeagueId = leagueId;
            MatchCode = matchCode;
            HomeId = homeId;
            AwayId = awayId;
            Race = race;
            ShowTime = showTime;
            IsNeutral = isNeutral;
            BetStatus = betStatus;
            SettlementStatus = settlementStatus;
            EventDate = eventDate;
            LeagueGroupId = leagueGroupId;
        }

        public int MatchId { get; set; }

        public string MatchCode { get; set; }

        public int SportType { get; set; }

        public int LeagueId { get; set; }

        public int LeagueGroupId { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public string Race { get; set; }

        public string ShowTime { get; set; }

        public string Status { get; set; }

        public bool IsNeutral { get; set; }

        public string BetStatus { get; set; }

        public byte SettlementStatus { get; set; }

        public DateTime EventDate { get; set; }
    }
}
