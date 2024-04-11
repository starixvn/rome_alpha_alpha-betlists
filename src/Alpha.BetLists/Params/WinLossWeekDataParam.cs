namespace Alpha.BetLists.Params
{
    using System;
    using System.Collections.Generic;

    public class WinLossWeekDataParam
    {
        public WinLossWeekDataParam(
            int viewerId,
            DateTime fromDate,
            DateTime toDate,
            DateTime limitedDate,
            IEnumerable<int> productIds,
            string productDetailsXML,
            string username)
        {
            ViewerId = viewerId;
            FromDate = fromDate;
            ToDate = toDate;
            ProductIds = productIds;
            LimitedDate = limitedDate;
            ProductDetailsXML = productDetailsXML;
            Username = username;
        }

        public int ViewerId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime LimitedDate { get; }

        public IEnumerable<int> ProductIds { get; }

        public string ProductDetailsXML { get; }

        public string Username { get; }
    }
}
