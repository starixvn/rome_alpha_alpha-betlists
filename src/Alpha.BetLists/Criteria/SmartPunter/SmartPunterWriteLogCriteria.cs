namespace Alpha.BetLists.Criteria.SmartPunter
{
    using Fanex.Data.Repository;

    public class SmartPunterWriteLogCommand : NonQueryCommand
    {
        public SmartPunterWriteLogCommand(
            string listUserName,
            int actionId,
            bool isRestricted,
            int userId,
            string ip)
        {
            ListUserName = listUserName;
            ActionId = actionId;
            IsRestricted = isRestricted;
            UserId = userId;
            IP = ip;
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