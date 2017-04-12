namespace Alpha.BetLists.Criteria.SmartPunter
{
    using Alpha.SmartPunter.Criteria;
    using Fanex.Data.Repository;

    public class SmartPunterWriteLogNoDataCommand : NonQueryCommand
    {
        public SmartPunterWriteLogNoDataCommand(SmartPunterWriteLogNoData commandData)
        {
            CustId = commandData.CustId;
            ActionId = commandData.ActionId;
            IsRestricted = commandData.IsRestricted;
            UserId = commandData.UserId;
            IP = commandData.IP;
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