namespace Alpha.BetLists.Criteria.BullfightingByMatch
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models;

    public class GetBetListBullfightingByMatchCriteria : CriteriaBase
    {
        public GetBetListBullfightingByMatchCriteria(
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
        
        [SpParam(Ignored = true)]
        public UserLevel CustLevel { get; }

        public override string GetSettingKey()
            => CustLevel.IsAgentLevel()
            ? "Age_BetList_TotalBets_BF_Ag"
            : CustLevel.IsMasterLevel()
            ? "Age_BetList_TotalBets_BF_MAg"
            : "Age_BetList_TotalBets_BF_SAg";

        public override bool IsValid()
            => CustId > 0 && MatchId > 0;
    }
}
