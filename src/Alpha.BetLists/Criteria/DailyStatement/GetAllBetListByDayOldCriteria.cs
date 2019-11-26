namespace Alpha.BetLists.Criteria.DailyStatement
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetAllBetListByDayOldCriteria : CriteriaBase
    {
        public GetAllBetListByDayOldCriteria(
            int custId,
            DateTime winLossDate,
            int transType,
            string language)
        {
            CustId = custId;
            WinLossDate = winLossDate;
            TransType = transType;
            Language = language;
        }

        public int CustId { get; }

        [SpParam(SqlParamName = "FromDate")]
        public DateTime WinLossDate { get; }

        public int TransType { get; }

        [SpParam(Ignored = true)]
        public string Language { get; }

        public override string GetSettingKey() => "Age_BetList_SelByDayAll_Mem_WH";

        public override bool IsValid() => CustId > 0;
    }
}
