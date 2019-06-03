namespace Alpha.BetLists.Tests.TestData
{
    using System;
    using System.Collections.Generic;
    using BetLists.Models;
    using Constants;

    public static class LastBetsTestData
    {
        public static readonly IEnumerable<TicketDto> AllTickets = new List<TicketDto>
        {
            CreateTicketDto(BetTypeIdConstants.Outright),
            CreateTicketDto(BetTypeIdConstants.Handicap),
            CreateTicketDto(BetTypeIdConstants.VirtualHorseRacing_Win),
            CreateTicketDto(BetTypeIdConstants.Handicap, 1002),
            CreateTicketDto(BetTypeIdConstants.Outright, 1003)
        };

        [ThreadStatic]
        public static readonly List<MatchInfo> RunningOutrightMatch = new List<MatchInfo>
        {
            CreateMatchInfo(matchId: 1000)
        };

        [ThreadStatic]
        public static readonly List<MatchInfo> RunningOtherMatch = new List<MatchInfo>
        {
            CreateMatchInfo(matchId: 1001)
        };

        private static TicketDto CreateTicketDto(int betType, int matchId = 0)
                => new TicketDto
                {
                    BetTypeId = betType,
                    MatchId = matchId
                };

        private static MatchInfo CreateMatchInfo(int matchId)
            => new MatchInfo(0, string.Empty, matchId, 0, string.Empty, 0, 0, string.Empty, string.Empty, 0, DateTime.Today);
    }
}
