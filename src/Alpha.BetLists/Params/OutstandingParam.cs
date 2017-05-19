namespace Alpha.BetLists.Params
{
    using System;
    using System.Collections.Generic;

    public class OutstandingParam
    {
        public OutstandingParam(
            int custId,
            IEnumerable<int> productIds,
            DateTime fromDate,
            DateTime toDate,
            bool getAll,
            string language,
            string userName)
        {
            CustId = custId;

            if (getAll)
            {
                FromDate = DateTime.Today.AddDays(-7);
                ToDate = DateTime.Today;
            }
            else
            {
                FromDate = fromDate == DateTime.MinValue ? DateTime.Today.AddDays(-1) : fromDate;
                ToDate = toDate == DateTime.MinValue ? DateTime.Today.AddDays(-1) : toDate;
            }

            GetAll = getAll;
            Language = language;
            UserName = userName;
            ProductIds = productIds;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public bool GetAll { get; }

        public string Language { get; }

        public string UserName { get; }

        public IEnumerable<int> ProductIds { get; }
    }
}