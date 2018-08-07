namespace Alpha.BetLists.Params
{
    using System;
    using System.Collections.Generic;
    using Constants;

    public class CancelledBetParam
    {
        public CancelledBetParam(
            int currentUserId,
            DateTime fromDate,
            DateTime toDate,
            DateTime moveBetsDay,
            byte roleId,
            IEnumerable<int> productIds,
            string statusIds,
            string language,
            int pageSize,
            int pageIndex)
        {
            CustId = currentUserId;         
            Language = language;
            ProductIds = productIds;
            FromDate = fromDate;
            ToDate = toDate;
            StatusIds = statusIds;
            MoveBetsDay = moveBetsDay;
            RoleId = roleId;
            PageIndex = pageIndex == 0 ? DefaultVariables.PageIndex : pageIndex;
            PageSize = 10;
        }

        public int CustId { get; }

        public byte RoleId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime MoveBetsDay { get; }

        public IEnumerable<int> ProductIds { get; }

        public string StatusIds { get; }

        public string Language { get; }

        public int PageSize { get; }

        public int PageIndex { get; }
    }
}
