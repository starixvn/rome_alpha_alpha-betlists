namespace Alpha.BetLists.Site.ViewModels
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests;
    using Sunplus.Agent.Model.Entities;

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
        }

        public string CustName { get; protected set; }

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
    }
}