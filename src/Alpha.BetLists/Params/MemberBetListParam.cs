namespace Alpha.BetLists.Params
{
    using Extensions;
    using System;
    using System.Collections.Generic;

    public class MemberBetListParam
    {
        public MemberBetListParam(
            int custId,
            IEnumerable<int> productIds,
            DateTime fromDate,
            DateTime toDate,
            string language,
            string userName)
        {
            CustId = custId;
            ProductIds = productIds;
            FromDate = fromDate.ToStartOfDay();
            ToDate = toDate.ToEndOfDay();
            Language = language;
            UserName = userName;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string Language { get; }

        public string UserName { get; }

        public IEnumerable<int> ProductIds { get; }
    }
}