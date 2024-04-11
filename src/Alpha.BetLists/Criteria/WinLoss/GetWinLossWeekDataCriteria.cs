namespace Alpha.BetLists.Criteria.WinLossByDate
{
    using System;
    using Fanex.Data.Repository;

    public class GetWinLossWeekDataCriteria : CriteriaBase
    {
        public GetWinLossWeekDataCriteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime balanceUpDay,
            DateTime moveBetsDay,
            string productIds,
            string productDetails,
            string username)
        {
            CustId = custId;
            FromDate = fromDate.Date;
            ToDate = toDate.Date;
            BalanceUpDay = balanceUpDay;
            MoveBetsDay = moveBetsDay;
            ProductIds = productIds;
            ProductDetails = productDetails;
            Username = username;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime BalanceUpDay { get; }

        public DateTime MoveBetsDay { get; }

        public string ProductIds { get; }

        public string ProductDetails { get; }

        public string Username { get; }

        public override string GetSettingKey() => "Age_Rpt_WinlossDetail_WeekDataSel";

        public override bool IsValid() => CustId > 0 && FromDate <= ToDate && !string.IsNullOrWhiteSpace(Username);
    }
}
