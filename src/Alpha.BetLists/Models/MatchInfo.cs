using System;

namespace Alpha.BetLists.Models
{
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
            DateTime eventDate)
            : this(sportType, status, matchId, leagueId, matchCode, homeId, awayId, race, showTime, false, string.Empty, settlementStatus, eventDate)
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
            DateTime eventDate)
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
        }

        public int MatchId { get; }

        public string MatchCode { get; }

        public int SportType { get; }

        public int LeagueId { get; }

        public int HomeId { get; }

        public int AwayId { get; }

        public string Race { get; }

        public string ShowTime { get; }

        public string Status { get; }

        public bool IsNeutral { get; }

        public string BetStatus { get; }

        public byte SettlementStatus { get; }

        public DateTime EventDate { get; }
    }
}
