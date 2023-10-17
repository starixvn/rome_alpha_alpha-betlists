namespace Alpha.BetLists.Criteria.ParlayWinLossByMatch
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models;

    public class GetParlayWinLossByMatchCriteria : CriteriaBase
    {
        public GetParlayWinLossByMatchCriteria(
            int custId,
            int betType,
            DateTime fromDate,
            DateTime toDate,
            DateTime moveBetsDate,
            UserLevel custLevel)
        {
            CustId = custId;
            BetType = betType;
            FromDate = fromDate;
            ToDate = toDate;
            MoveBetsDate = moveBetsDate;
            CustLevel = custLevel;
        }

        public int CustId { get; }

        public int BetType { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        [SpParam(SqlParamName = "MoveBetsDay")]
        public DateTime MoveBetsDate { get; }

        [SpParam(Ignored = true)]
        public UserLevel CustLevel { get; }

        public override string GetSettingKey() =>
            CustLevel.IsSuperLevel()
            ? "Age_BetList_SelWinlossParlay_SAg"
            : CustLevel.IsMasterLevel()
            ? "Age_BetList_SelWinlossParlay_MAg"
            : "Age_BetList_SelWinlossParlay_Ag";

        public override bool IsValid()
            => CustId > 0
            && BetType > 0
            && ToDate >= FromDate
            && FromDate > DateTime.MinValue;
    }
}
