namespace Alpha.BetLists.Criteria.MonitoringBets
{
    using Fanex.Data.Repository;

    public class GetLeaguesCriteria : CriteriaBase
    {
        public override string GetSettingKey()
            => "Age_BetList_LastBets_LeagueSel";

        public override bool IsValid()
            => true;
    }
}
