namespace Alpha.BetLists.Site.ViewModels
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using AppResources;
    using BetLists.Models;
    using Configurations;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests;
    using Web.Helpers;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1304:NonPrivateReadonlyFieldsMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
    public abstract class BetListBaseViewModel
    {
        protected readonly BaseQuery baseQuery;

        protected BetListBaseViewModel(
            BaseQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper)
        {
            baseQuery = query;
            CustName = query.CustName;
            FromDate = query.FromDate;
            ToDate = query.ToDate;
            BetListData = betListData;
            Builder = builder;
            TicketHelper = ticketHelper;
            ProductsId = baseQuery.ProductIds;
            BetListType = baseQuery.Type;
            CustId = baseQuery.CustId;
            IsHistoricReport = baseQuery.IsHistoricReport;
            ViewerIdOrViewerAdminId = baseQuery.ViewerIdOrViewerAdminId;
            ViewerLevelId = baseQuery.ViewerLevelId;
            ViewerLevel = baseQuery.ViewerLevel;
            DisplayVNIP = baseQuery.DisplayVNIP;
        }

        public string CustName { get; protected set; }

        public int CustId { get; protected set; }

        public DateTime FromDate { get; protected set; }

        public DateTime ToDate { get; protected set; }

        public BetListData BetListData { get; protected set; }

        public IBetListHTMLBuilder Builder { get; protected set; }

        public ITicketHelper TicketHelper { get; protected set; }

        // TODO: will implement here
        public bool CanViewScoreMap { get; }

        public bool DisplayVNIP { get; }

        public string ProductsId { get; protected set; }

        public string BetListType { get; protected set; }

        public bool IsHistoricReport { get; protected set; }

        public string ResultsPath => AppPathHelper.GetAppPath(AppSettings.ResultAppPath, "~");

        public string OldMainPath => MicroSites.OldMainUrl("~");

        public int ViewerIdOrViewerAdminId { get; protected set; }

        public byte ViewerLevelId { get; protected set; }

        public UserLevel ViewerLevel { get; protected set; }

        protected string GetTitle(BaseQuery query)
            => query.FromDate == DateTime.MinValue || query.ToDate == DateTime.MinValue
            ? Resources.memberBetList
            : $"{Resources.memberBetList} {query.FromDate.ToShortDateString()} --> {query.ToDate.ToShortDateString()} - {query.CustName}";
    }
}