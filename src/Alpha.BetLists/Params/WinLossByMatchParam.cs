namespace Alpha.BetLists.Params
{
    using System;
    using Alpha.BetLists.Helpers;
    using Models;

    public class WinLossByMatchParam
    {
        public WinLossByMatchParam(
            int custId,
            int leagueId,
            byte leagueType,
            int matchId,
            int betType,
            string race,
            int liveIndicator,
            string language,
            string custName,
            DateTime fromDate,
            DateTime toDate,
            UserLevel custLevel)
        {
            CustId = custId;

            LeagueId = leagueId;
            MatchId = matchId;
            BetType = betType;
            Race = race;
            LiveIndicator = liveIndicator == 1;

            Language = language;
            CustName = custName;

            FromDate = fromDate;
            ToDate = toDate;
            CustLevel = custLevel;
            EventType = BetTypeHelper.GetEventType(leagueType);
        }

        public int CustId { get; }

        public int BetType { get; }

        public int LeagueId { get; }

        public byte EventType { get; }

        public int MatchId { get; }

        public string Race { get; }

        public string Language { get; }

        public string CustName { get; }

        public bool LiveIndicator { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public UserLevel CustLevel { get; }
    }
}
