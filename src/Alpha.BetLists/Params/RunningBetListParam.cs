namespace Alpha.BetLists.Params
{
    using System;
    using System.Collections.Generic;
    using Extensions;

    public class RunningBetListParam
    {
        public RunningBetListParam(
            int custId,
            IEnumerable<int> productIds,
            DateTime fromDate,
            DateTime toDate,
            DateTime limitedDate,
            bool getAll,
            string language,
            string userName)
        {
            CustId = custId;

            FromDate = fromDate.ToStartOfDay();
            ToDate = toDate.ToEndOfDay();

            GetAll = getAll;
            Language = language;
            UserName = userName;
            ProductIds = productIds;
            LimitedDate = limitedDate;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public bool GetAll { get; }

        public string Language { get; }

        public string UserName { get; }

        public IEnumerable<int> ProductIds { get; }

        public DateTime LimitedDate { get; }
    }
}