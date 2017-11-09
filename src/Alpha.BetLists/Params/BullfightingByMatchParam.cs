namespace Alpha.BetLists.Params
{
    using Models;

    public class BullfightingByMatchParam
    {
        public BullfightingByMatchParam(
            int custId,
            int matchId,
            UserLevel custLevel)
        {
            CustId = custId;
            MatchId = matchId;
            CustLevel = custLevel;
        }

        public int CustId { get; }

        public int MatchId { get; }
        
        public UserLevel CustLevel { get; }
    }
}
