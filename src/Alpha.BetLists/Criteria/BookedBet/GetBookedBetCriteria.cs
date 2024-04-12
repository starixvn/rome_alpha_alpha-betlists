namespace Alpha.BetLists.Criteria.CancelledBet
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetBookedBetCriteria : CriteriaBase
    {
        public GetBookedBetCriteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            byte roleId,
            string statusIds,
            int pageSize,
            int pageIndex)
        {
            CustId = custId;
            FromDate = fromDate;
            ToDate = toDate;
            RoleId = roleId;
            StatusIds = statusIds;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public byte RoleId { get; }

        [SpParam(SqlParamName = "StatusIdList")]
        public string StatusIds { get; }

        public int PageSize { get; }

        public int PageIndex { get; }

        public int TotalRows { get; set; }

        public override string GetSettingKey() => "Age_BetList_SelBookedBet";

        public override bool IsValid()
            => CustId > 0 &&
            FromDate <= ToDate &&
            RoleId > 0 &&
            !string.IsNullOrWhiteSpace(StatusIds);
    }
}
