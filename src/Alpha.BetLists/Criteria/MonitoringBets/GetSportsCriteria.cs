namespace Alpha.BetLists.Criteria.MonitoringBets
{
    using Fanex.Data.Repository;

    public class GetSportsCriteria : CriteriaBase
    {
        public override string GetSettingKey()
            => "Age_BetList_LastBets_SportSel";

        public override bool IsValid()
            => true;
    }
}
