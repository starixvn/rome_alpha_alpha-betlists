namespace Alpha.BetLists.Site.ViewModels
{
    using System;
    using BetLists.Models;
    using Requests;

    public abstract class BetListBaseViewModel
    {
        protected readonly BaseQuery baseQuery;

        protected BetListBaseViewModel(BaseQuery query, BetListData betListData)
        {
            baseQuery = query;
            CustName = query.CustName;
            FromDate = query.FromDate;
            ToDate = query.ToDate;
            BetListData = betListData;
        }

        public string CustName { get; protected set; }

        public DateTime FromDate { get; protected set; }

        public DateTime ToDate { get; protected set; }

        public BetListData BetListData { get; protected set; }
    }
}