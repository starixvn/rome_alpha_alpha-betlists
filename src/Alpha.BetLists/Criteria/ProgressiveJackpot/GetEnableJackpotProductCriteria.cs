namespace Alpha.BetLists.Criteria.ProgressiveJackpot
{
    using Alpha.BetLists.Configurations;
    using Fanex.Data.Repository;

    public class GetEnableJackpotProductCriteria : CriteriaBase
    {

        public bool IsTest => AppSettings.LocalTest;

        public override string GetSettingKey() => "Acc_GameLaunchSel";

        public override bool IsValid() => true;
    }
}