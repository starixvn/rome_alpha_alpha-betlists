namespace Alpha.BetLists.Criteria.SmartPunter
{
    using AlphaPlus.SmartPunter.Criteria;
    using Fanex.Data.Repository;

    public class SmartPunterWriteLogCommand : NonQueryCommand
    {
        public SmartPunterWriteLogCommand(SmartPunterWriteLog commandData)
        {
            ListUserName = commandData.ListUserName;
            ActionId = commandData.ActionId;
            IsRestricted = commandData.IsRestricted;
            UserId = commandData.UserId;
            IP = commandData.IP;
        }

        public string ListUserName { get; }

        public int ActionId { get; }

        public bool IsRestricted { get; }

        public int UserId { get; }

        public string IP { get; }

        public override string GetSettingKey() => "Age_UserLog_SmartPunterInsMultiple";

        public override bool IsValid() => !string.IsNullOrEmpty(ListUserName);
    }
}