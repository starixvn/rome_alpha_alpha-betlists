namespace Alpha.BetLists.Criteria.MonitoringBets
{
    using Fanex.Data.Repository;

    public class GetMatchesCriteria : CriteriaBase
    {
        public override string GetSettingKey()
            => "Age_BetList_LastBets_MatchSel";

        public override bool IsValid()
            => true;
    }
}
