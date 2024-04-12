namespace Alpha.BetLists.Params
{
    using System;
    using Constants;

    public class BookedBetParam
    {   
        public BookedBetParam(
            int currentUserId,
            DateTime fromDate,
            DateTime toDate,
            byte roleId,
            string statusIds,
            int pageSize,
            int pageIndex)
        {
            CustId = currentUserId;         
            FromDate = fromDate;
            ToDate = toDate;
            StatusIds = statusIds;
            RoleId = roleId;
            PageIndex = pageIndex == 0 ? DefaultVariables.PageIndex : pageIndex;
            PageSize = pageSize == 0 ? DefaultVariables.PageSize : pageSize;
        }

        public int CustId { get; }

        public byte RoleId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string StatusIds { get; }
        
        public int PageSize { get; }

        public int PageIndex { get; }
    }
}
