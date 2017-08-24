using Fanex.Data.Repository;

namespace Alpha.BetLists.Criteria.LastBets
{
    public class GetRunningOtherMatchCriteria : CriteriaBase
    {
        public int MatchId => 0;

        public override string GetSettingKey() => "Age_Monitor_MatchSel";

        public override bool IsValid() => true;
    }
}
