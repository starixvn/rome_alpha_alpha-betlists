namespace Alpha.BetLists.Criteria.DailyStatement
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System;

    public class GetAllBetListByDayCriteria : CriteriaBase
    {
        public GetAllBetListByDayCriteria(
            int custId,
            DateTime winLossDate,
            DateTime moveBetsDate,
            int transType,
            string language)
        {
            CustId = custId;
            WinLossDate = winLossDate;
            MoveBetsDate = moveBetsDate;
            TransType = transType;
            Language = language;
        }

        public int CustId { get; }

        [SpParam(SqlParamName = "FromDate")]
        public DateTime WinLossDate { get; }

        [SpParam(SqlParamName = "MoveBetsDay")]
        public DateTime MoveBetsDate { get; }

        public int TransType { get; }

        public string Language { get; }

        public override string GetSettingKey() => "Age_BetList_SelByDayAll_Mem";

        public override bool IsValid() => CustId > 0;
    }
}
