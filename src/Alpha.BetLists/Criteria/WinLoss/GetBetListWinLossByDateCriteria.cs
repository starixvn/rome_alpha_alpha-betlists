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
            string productDetails,
            DateTime fromDate,
            DateTime toDate,
            DateTime moveBetsDate,
            string subfix,
            bool showProgressiveBonus = false,
            int maxTotalRow = 10000,
            bool isExportExcel = false)
        {
            CustId = custId;
            FromDate = fromDate;
            ToDate = toDate;
            MoveBetsDate = moveBetsDate;
            ProductIds = productIds;
            ProductDetails = productDetails;
            ShowProgressiveBonus = showProgressiveBonus;
            ErrCode = 0;
            ErrMsg = string.Empty;
            MaxTotalRow = maxTotalRow;
            IsExportExcel = isExportExcel;
            Subfix = subfix;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        [SpParam(SqlParamName = "movebetsday")]
        public DateTime MoveBetsDate { get; }

        public string ProductIds { get; }

        public string ProductDetails { get; }

        public bool ShowProgressiveBonus { get; }

        public int MaxTotalRow { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public bool IsExportExcel { get; }

        [SpParam(Ignored = true)]
        public string Subfix { get; }

        public override string GetSettingKey() => $"Age_BetList_SelWinLossByDate_Mem{Subfix}";

        public override bool IsValid() =>
            CustId > 0
            && !string.IsNullOrWhiteSpace(ProductIds)
            && ToDate >= FromDate
            && FromDate > DateTime.MinValue;
    }
}