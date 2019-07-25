namespace Alpha.BetLists.Criteria.DailyStatement
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetAllBetListByDayCriteria : CriteriaBase
    {
        public GetAllBetListByDayCriteria(
            int custId,
            DateTime winLossDate,
            DateTime moveBetsDate,
            int transType)
        {
            CustId = custId;
            WinLossDate = winLossDate;
            MoveBetsDate = moveBetsDate;
            TransType = transType;
        }

        public int CustId { get; }

        [SpParam(SqlParamName = "FromDate")]
        public DateTime WinLossDate { get; }

        [SpParam(SqlParamName = "MoveBetsDay")]
        public DateTime MoveBetsDate { get; }

        public int TransType { get; }

        public override string GetSettingKey() => "Age_BetList_SelByDayAll_Mem";

        public override bool IsValid() => CustId > 0;
    }
}
