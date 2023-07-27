namespace Alpha.BetLists.Site.ViewModels.FindTickets
{
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests.FindTickets;
    using FindTickets;
    using Alpha.BetLists.Site.ViewModels.WinLoss;
    using AppResources;
    using Alpha.BetLists.Extensions;

    public class FindTicketsViewModel : WinLossViewModel
    {
        public FindTicketsViewModel(
            FindTicketsQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper)
            : base(query, betListData, builder, ticketHelper, query.IsEnabledJackpot)
        {

            RefNo = query.RefNo;
            ViewDataInstructionMessage = string.Format(
                Resources.CanOnlyViewDataAfterTimeHistoryReport,
                query.MinDate.ToShortDateWithSlash());
            Title = Resources.FindTickets;
            MainDomain = query.IsInternal ? query.MainDomain : string.Empty;
        }

        public long RefNo { get; }
        
        public string ViewDataInstructionMessage { get; }
        
        public string MainDomain { get; }
    }
}