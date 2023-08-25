namespace Alpha.BetLists.Site.ViewModels.SearchTickets
{
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests.SearchTickets;
    using Alpha.BetLists.Site.ViewModels.WinLoss;
    using AppResources;
    using Alpha.BetLists.Extensions;

    public class SearchTicketsViewModel : WinLossViewModel
    {
        public SearchTicketsViewModel(
            SearchTicketsQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper)
            : base(query, betListData, builder, ticketHelper, query.IsEnabledJackpot, false, true)
        {
            RefNo = query.RefNo;
            ViewDataInstructionMessage = string.Format(
                Resources.CanOnlyViewDataAfterTimeHistoryReportWithFormat,
                query.MinDate.ToShortDateWithSlash());
            Title = Resources.SearchTickets;
            MainDomain = query.IsInternal ? query.MainDomain : string.Empty;
        }

        public long RefNo { get; }

        public string ViewDataInstructionMessage { get; }

        public string MainDomain { get; }
    }
}