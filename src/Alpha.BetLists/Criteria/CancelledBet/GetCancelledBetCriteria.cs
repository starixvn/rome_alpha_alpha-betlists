namespace Alpha.BetLists.Criteria.CancelledBet
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetCancelledBetCriteria : CriteriaBase
    {
        public GetCancelledBetCriteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime moveBetsDay,
            byte roleId,
            string productIds,
            string statusIds,
            int pageSize,
            int pageIndex)
        {
            CustId = custId;
            FromDate = fromDate;
            ToDate = toDate;
            RoleId = roleId;
            ProductIds = productIds;
            StatusIds = statusIds;
            PageIndex = pageIndex;
            PageSize = pageSize;
            MoveBetsDay = moveBetsDay;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime MoveBetsDay { get; }

        public byte RoleId { get; }

        public string ProductIds { get; }

        [SpParam(SqlParamName = "StatusIdList")]
        public string StatusIds { get; }
        
        public int PageSize { get; }

        public int PageIndex { get; }

        public int TotalRows { get; set; }

        public override string GetSettingKey() => "Age_BetList_SelDeleted";

        public override bool IsValid()
            => CustId > 0 &&
            FromDate < ToDate &&
            !string.IsNullOrWhiteSpace(ProductIds) &&
            !string.IsNullOrWhiteSpace(StatusIds);
    }
}
