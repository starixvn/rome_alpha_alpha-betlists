namespace Alpha.BetLists.Criteria.WinLossByMatch
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetWinLossByBetTypeCriteria : CriteriaBase
    {
        public GetWinLossByBetTypeCriteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime moveBetsDate,
            byte roleId,
            int bettype)
        {
            CustId = custId;
            FromDate = fromDate;
            ToDate = toDate;
            MoveBetsDate = moveBetsDate;
            RoleId = roleId;
            BetType = bettype;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        [SpParam(SqlParamName = "MoveBetsDay")]
        public DateTime MoveBetsDate { get; }

        public byte RoleId { get; }

        public int BetType { get; }

        public override string GetSettingKey()
            => "Age_BetList_SelWinLossByBetType";

        public override bool IsValid() =>
            CustId > 0
            && ToDate >= FromDate
            && FromDate > DateTime.MinValue;
    }
}
