namespace Alpha.BetLists.Criteria.WinLossByDate
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetBetListWinLossByDateCriteria : CriteriaBase
    {
        public GetBetListWinLossByDateCriteria(
            int custId,
            string productIds,
            DateTime fromDate,
            DateTime toDate,
            DateTime moveBetsDate,
            string language,
            bool showProgressiveBonus = false)
        {
            CustId = custId;
            FromDate = fromDate;
            ToDate = toDate;
            MoveBetsDate = moveBetsDate;
            Language = language;
            ProductIds = productIds;
            ShowProgressiveBonus = showProgressiveBonus;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        [SpParam(SqlParamName = "movebetsday")]
        public DateTime MoveBetsDate { get; }

        public string Language { get; }

        public string ProductIds { get; }

        public bool ShowProgressiveBonus { get; } = false;

        public override string GetSettingKey() => "Age_BetList_SelWinLossByDate_Mem";

        public override bool IsValid() =>
            CustId > 0
            && !string.IsNullOrWhiteSpace(ProductIds)
            && ToDate >= FromDate;
    }
}