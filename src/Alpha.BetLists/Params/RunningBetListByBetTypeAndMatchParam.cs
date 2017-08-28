namespace Alpha.BetLists.Params
{
    using Models;

    public class RunningBetListByBetTypeAndMatchParam
    {
        public RunningBetListByBetTypeAndMatchParam(
            int matchId,
            int custId,
            int betType,
            string language,
            int liveIndicator,
            string betTeam,
            bool isShowRacing,
            UserLevel custLevel)
        {
            MatchId = matchId;
            CustId = custId;
            BetType = betType;
            Language = language;
            LiveIndicator = liveIndicator;
            BetTeam = betTeam;
            IsShowRacing = isShowRacing;
            CustLevel = custLevel;
        }

        public int MatchId { get; }

        public int CustId { get; }

        public int BetType { get; }

        public string Language { get; }

        public int LiveIndicator { get; }

        public string BetTeam { get; }

        public bool IsShowRacing { get; }

        public UserLevel CustLevel { get; }
    }
}
