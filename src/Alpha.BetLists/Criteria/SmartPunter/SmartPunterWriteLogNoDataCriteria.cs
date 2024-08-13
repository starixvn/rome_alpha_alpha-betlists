namespace Alpha.BetLists.Criteria.SmartPunter
{
    using Fanex.Data.Repository;

    public class SmartPunterWriteLogNoDataCommand : NonQueryCommand
    {
        public SmartPunterWriteLogNoDataCommand(
            int? custId,
            int actionId,
            bool isRestricted,
            int userId,
            string ip)
        {
            CustId = custId;
            ActionId = actionId;
            IsRestricted = isRestricted;
            UserId = userId;
            IP = ip;
        }

        public int? CustId { get; }

        public int ActionId { get; }

        public bool IsRestricted { get; }

        public int UserId { get; }

        public string IP { get; }

        public override string GetSettingKey() => "NAP_UserLog_SmartPunterIns";

        public override bool IsValid() => !CustId.HasValue || CustId.Value > 0;
    }
}