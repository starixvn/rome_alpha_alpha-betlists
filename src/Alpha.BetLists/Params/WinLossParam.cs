namespace Alpha.BetLists.Params
{
    using System;
    using System.Collections.Generic;

    public class WinLossParam
    {
        public WinLossParam(
            int custId,
            IEnumerable<int> productIds,
            DateTime fromDate,
            DateTime toDate,
            DateTime limitedDate,
            string language)
        {
            CustId = custId;
            FromDate = fromDate;
            ToDate = toDate;
            Language = language;
            ProductIds = productIds;
            LimitedDate = limitedDate;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string Language { get; }

        public IEnumerable<int> ProductIds { get; }

        public DateTime LimitedDate { get; }
    }
}