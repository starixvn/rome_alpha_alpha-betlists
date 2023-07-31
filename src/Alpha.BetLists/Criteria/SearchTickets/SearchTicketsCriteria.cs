namespace Alpha.BetLists.Criteria.SearchTickets
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models;

    public class SearchTicketsCriteria : CriteriaBase
    {
        public SearchTicketsCriteria(
             long refNo, int uplineId, UserLevel custLevel, DateTime moveBetsDay)
        {
            RefNo = refNo;
            UplineId = uplineId;
            RoleId = custLevel.Value;
            MoveBetsDay = moveBetsDay;
        }

        public long RefNo { get; }
        public int UplineId { get; }
        public byte RoleId { get; }
        public DateTime MoveBetsDay { get; }

        public override string GetSettingKey() => "Age_BetList_SearchByRefNo";

        public override bool IsValid() => RefNo > 0 && UplineId > 0;
    }
}
