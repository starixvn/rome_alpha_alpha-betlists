namespace Alpha.BetLists.Criteria.LastBets
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetRunningOutrightMatchCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "isvsracing")]
        public bool IsVirtualSportsRacing => false;

        public override string GetSettingKey() => "Age_Monitor_OutrightSel";

        public override bool IsValid() => true;
    }
}
