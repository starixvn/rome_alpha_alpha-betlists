namespace Alpha.BetLists.Criteria.WinLossByDate
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetBetListWinLossByDateOldCriteria : CriteriaBase
    {
        public GetBetListWinLossByDateOldCriteria(
            int custId,
            string productIds,
            DateTime fromDate,
            DateTime toDate,
            string language,
            bool showProgressiveBonus = false)
        {
            CustId = custId;
            FromDate = fromDate;
            ToDate = toDate;
            Language = language;
            ProductIds = productIds;
            ShowProgressiveBonus = showProgressiveBonus;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        [SpParam(Ignored = true)]
        public string Language { get; }

        public string ProductIds { get; }

        public bool ShowProgressiveBonus { get; }

        public override string GetSettingKey() => "Age_BetList_SelWinLossByDate_Mem_Old";

        public override bool IsValid() =>
            CustId > 0
            && !string.IsNullOrWhiteSpace(ProductIds)
            && ToDate >= FromDate
            && FromDate > DateTime.MinValue;
    }
}
