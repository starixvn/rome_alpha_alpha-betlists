namespace Alpha.BetLists.Params
{
    using Models;

    public class RacingByMatchParam
    {
        public RacingByMatchParam(
            int custId,
            int betType,
            int matchId,
            string language,
            bool isShowRacing,
            UserLevel custLevel)
        {
            CustId = custId;
            BetType = betType;
            MatchId = matchId;
            Language = language;
            IsShowRacing = isShowRacing;
            CustLevel = custLevel;
        }

        public int CustId { get; }

        public int BetType { get; }

        public int MatchId { get; }

        public string Language { get; }

        public bool IsShowRacing { get; }

        public UserLevel CustLevel { get; }
    }
}
