namespace Alpha.BetLists.Site.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using BetLists.Models;
    using Configurations;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Requests;
    using Sunplus.Agent.Model.Entities;
    using Web.Helpers;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1304:NonPrivateReadonlyFieldsMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
    public abstract class BetListBaseViewModel
    {
        private static readonly IDictionary<UserLevel, int> TotalColumnByViewLevelId = new Dictionary<UserLevel, int>
        {
            { UserLevel.Agent, 8 },
            { UserLevel.Master, 9 },
            { UserLevel.Super, 10 }
        };

        private static readonly IDictionary<UserLevel, int> FooterStatusColSpanByViewLevelId = new Dictionary<UserLevel, int>
        {
            { UserLevel.Agent, 2 },
            { UserLevel.Master, 3 },
            { UserLevel.Super, 4 }
        };

        private static readonly string[] FooterTotalExcludeStatus = { BetStatus.Running, BetStatus.Waiting, BetStatus.Void, BetStatus.Reject };

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
            GenerateFooterData();
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

        // TODO: will refactor soon
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "VNIP", Justification = "Reviewed!")]
        public bool DisplayVNIP => UserSession.Current.IsAdminCanViewVNIP;

        public string ProductsId { get; protected set; }

        public string BetListType { get; protected set; }

        public bool IsHistoricReport { get; protected set; }

        public string ResultsDetailPath => AppPathHelper.GetAppPath(AppSettings.ResultAppPath, "~") + "Normal/ResultDetails";

        public string OldMainPath => MicroSites.OldMainUrl("~");

        public int ViewerIdOrViewerAdminId { get; protected set; }

        public byte ViewerLevelId { get; protected set; }

        public UserLevel ViewerLevel { get; set; }

        public int FooterSubTotalColSpan { get; protected set; } = 5;

        public virtual int FooterStatusColSpan
            => FooterStatusColSpanByViewLevelId[ViewerLevel];

        public string FooterSubTotalWinLoss { get; protected set; }

        public string FooterSubTotalCommission { get; protected set; }

        public string FooterTotal { get; protected set; }

        public int TotalColumn => TotalColumnByViewLevelId[ViewerLevel];

        private void GenerateFooterData()
        {
            var filteredTickets = BetListData.Tickets
               .Where(ticket => !FooterTotalExcludeStatus.Contains(ticket.Status.ToLowerInvariant()));

            var subTotalWinLoss = filteredTickets.Where(ticket => ticket.BetTypeId != 500).Sum(ticket => ticket.Winlost);
            var subTotalCommission = filteredTickets.Sum(ticket => ticket.PlayerComm);

            FooterSubTotalWinLoss = ConvertByBetType.Winloss(subTotalWinLoss);
            FooterSubTotalCommission = ConvertByBetType.Comm(subTotalCommission);
            FooterTotal = ConvertByBetType.Winloss(subTotalWinLoss + subTotalCommission);
        }
    }
}