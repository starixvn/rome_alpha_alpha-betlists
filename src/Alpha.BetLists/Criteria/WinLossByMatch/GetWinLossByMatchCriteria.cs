namespace Alpha.BetLists.Criteria.WinLossByMatch
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetWinLossByMatchCriteria : CriteriaBase
    {
        public GetWinLossByMatchCriteria(
             int custId,
             DateTime fromDate,
             DateTime toDate,
             DateTime moveBetsDay,
             int matchId,
             int betType,
             string custName,
             bool liveIndicator,
             int roleId)
        {
            FromDate = fromDate;
            ToDate = toDate;
            MoveBetsDay = moveBetsDay;
            CustId = custId;
            MatchId = matchId;
            BetType = betType;

            CustName = custName;
            LiveIndicator = liveIndicator;
            RoleId = roleId;
        }

        public int CustId { get; }

        public int MatchId { get; }

        public int BetType { get; }

        [SpParam(SqlParamName = "Username")]
        public string CustName { get; }

        public bool LiveIndicator { get; }

        public int RoleId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime MoveBetsDay { get; }

        public override string GetSettingKey() => "Age_BetList_SelWinlossByBettypeMatch";

        public override bool IsValid() => CustId > 0 && FromDate <= ToDate && DateTime.MinValue < FromDate;
    }
}
