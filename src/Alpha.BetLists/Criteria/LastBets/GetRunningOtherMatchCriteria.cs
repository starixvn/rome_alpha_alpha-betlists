using Fanex.Data.Repository;

namespace Alpha.BetLists.Criteria.LastBets
{
    public class GetRunningOtherMatchCriteria : CriteriaBase
    {
        public GetRunningOtherMatchCriteria(int matchId = 0)
        {
            MatchId = matchId;
        }

        public int MatchId { get; }

        public override string GetSettingKey() => "Age_Monitor_MatchSel";

        public override bool IsValid() => true;
    }
}
