namespace Alpha.BetLists.Site.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using Alpha.BetLists.Site.Constants;
    using AlphaPlus.User.Models;
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
        private static readonly IDictionary<UserLevel, int> TotalColumnByViewLevelId = new Dictionary<UserLevel, int>
        {
            { UserLevel.Agent, 9 },
            { UserLevel.Master, 10 },
            { UserLevel.Super, 11 }
        };

        protected readonly BaseQuery baseQuery;

        protected BetListBaseViewModel(
            BaseQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            bool showMemberName = false)
        {
            baseQuery = query;
            CustName = query.CustName;
            FromDate = query.FromDate;
            ToDate = query.ToDate;
            BetListData = betListData;
            Builder = builder;
            TicketHelper = ticketHelper;
            ProductsId = baseQuery.ProductIds;
            ResourceIds = baseQuery.ResourceIds;
            BetListType = baseQuery.Type;
            CustId = baseQuery.CustId;
            IsHistoricReport = baseQuery.IsHistoricReport;
            ViewerIdOrViewerAdminId = baseQuery.ViewerIdOrViewerAdminId;
            ViewerLevelId = baseQuery.ViewerLevelId;
            ViewerLevel = baseQuery.ViewerLevel;
            DisplayVNIP = baseQuery.DisplayVNIP;
            Title = GetTitle(query).ToUpperInvariant();
            TotalColumn = TotalColumnByViewLevelId[ViewerLevel] + (showMemberName ? 1 : 0);
            ShowMemberName = showMemberName;
        }

        public string CustName { get; protected set; }

        public int CustId { get; protected set; }

        public DateTime FromDate { get; protected set; }

        public DateTime ToDate { get; protected set; }

        public BetListData BetListData { get; protected set; }

        public IBetListHTMLBuilder Builder { get; protected set; }

        public ITicketHelper TicketHelper { get; protected set; }

        public bool DisplayVNIP { get; }

        public string ProductsId { get; protected set; }

        public string ResourceIds { get; protected set; }

        public string BetListType { get; protected set; }

        public bool IsHistoricReport { get; protected set; }

        public string ResultsPath
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return AppPathHelper.GetAppPath(AppSettings.ResultAppPath, "~");
                }
                return HttpContext.Current.Session.IsCookieless ?
                        $"/site-results/(S({UserSession.Current.GetProperty<string>(SessionKey.MainSessionID)}))/"
                        : AppPathHelper.GetAppPath(AppSettings.ResultAppPath, "~");
            }
        }

        public string OldMainPath
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return MicroSites.OldMainUrl("~");
                }
                return HttpContext.Current.Session.IsCookieless ?
                    $"/ex-main/(S({UserSession.Current.GetProperty<string>(SessionKey.MainSessionID)}))/" : MicroSites.OldMainUrl("~");
            }
        }

        public string TotalBetsForecastPath
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return AppPathHelper.GetAppPath(AppSettings.TotalBetsForecastAppPath, "~");
                }
                return HttpContext.Current.Session.IsCookieless ?
                $"/ex-totalbetsforecast/(S({UserSession.Current.GetProperty<string>(SessionKey.MainSessionID)}))/" : AppPathHelper.GetAppPath(AppSettings.TotalBetsForecastAppPath, "~");
            }
        }

        public string SiteMainPath => AppPathHelper.GetAppPath(AppSettings.MainAppPath, "~");

        public int ViewerIdOrViewerAdminId { get; protected set; }

        public byte ViewerLevelId { get; protected set; }

        public UserLevel ViewerLevel { get; protected set; }

        public string Title { get; protected set; }

        public int TotalColumn { get; protected set; }

        public bool ShowMemberName { get; }

        public string GetTitle(BaseQuery query)
            => HasDateParam(query)
            ? $"{Resources.memberBetList} {query.FromDate.ToShortDateString()} --> {query.ToDate.ToShortDateString()} - {query.CustName}"
            : Resources.memberBetList;

        public bool HasDateParam(BaseQuery query)
            => query.FromDate != DateTime.MinValue && query.ToDate != DateTime.MinValue;

        // TODO: will implement here
        public virtual bool CanViewScoreMap(ITicket ticket) => false;
    }
}