namespace Alpha.BetLists.Criteria.SmartPunter
{
    public class SmartPunterSMACheckerWriteLogCommand : SmartPunterWriteLogCommand
    {
        public SmartPunterSMACheckerWriteLogCommand(
            string listUserName,
            int actionId,
            bool isRestricted,
            int userId,
            string ip) : base(listUserName, actionId, isRestricted, userId, ip)
        {
        }

        public override string GetSettingKey() => "Age_CustomerSelCheckListSmartPunter";
    }
}