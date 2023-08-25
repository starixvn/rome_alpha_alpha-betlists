namespace Alpha.BetLists.Criteria.SmartPunter
{
    using AlphaPlus.SmartPunter.Criteria;

    public class SmartPunterSMACheckerWriteLogCommand : SmartPunterWriteLogCommand
    {
        public SmartPunterSMACheckerWriteLogCommand(SmartPunterSMACheckerWriteLog commandData)
            : base(commandData)
        {
        }

        public override string GetSettingKey() => "Age_CustomerSelCheckListSmartPunter";
    }
}