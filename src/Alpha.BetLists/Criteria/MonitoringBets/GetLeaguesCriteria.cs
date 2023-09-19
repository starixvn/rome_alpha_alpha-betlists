namespace Alpha.BetLists.Criteria.MonitoringBets
{
    using Alpha.BetLists.Configurations;
    using Fanex.Data.Repository;

    public class GetLeaguesCriteria : CriteriaBase
    {
        public override string GetSettingKey()
            => "Age_BetList_LastBets_LeagueSel";

        public bool IsShowAll { get; } = AppSettings.LocalTest;

        public override bool IsValid()
            => true;
    }
}
