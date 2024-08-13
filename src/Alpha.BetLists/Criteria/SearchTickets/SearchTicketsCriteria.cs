namespace Alpha.BetLists.Criteria.SearchTickets
{
    using System;
    using Fanex.Data.Repository;

    public class SearchTicketsCriteria : CriteriaBase
    {
        public SearchTicketsCriteria(
             long refNo, int uplineId, byte roleId, DateTime moveBetsDay, DateTime balanceUpDay)
        {
            RefNo = refNo;
            UplineId = uplineId;
            RoleId = roleId;
            MoveBetsDay = moveBetsDay;
            BalanceUpDay = balanceUpDay;
        }

        public long RefNo { get; }
        public int UplineId { get; }
        public byte RoleId { get; }
        public DateTime MoveBetsDay { get; }
        public DateTime BalanceUpDay { get; }

        public override string GetSettingKey() => "Age_BetList_SearchByRefNo";

        public override bool IsValid() => RefNo > 0 && UplineId > 0;
    }
}
