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
            UserLevel custLevel)
        {
            CustId = custId;
            BetType = betType;
            MatchId = matchId;
            Language = language;
            CustLevel = custLevel;
        }

        public int CustId { get; }

        public int BetType { get; }

        public int MatchId { get; }

        public string Language { get; }

        public UserLevel CustLevel { get; }
    }
}
