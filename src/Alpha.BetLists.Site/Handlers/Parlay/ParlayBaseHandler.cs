namespace Alpha.BetLists.Site.Handlers
{
    using System.Threading.Tasks;
    using Fanex.BetList.Core.Entities;
    using Requests.Parlay;
    using Services;
    using ViewModels.Parlay;

    public class ParlayBaseHandler : BetListBaseHandler
    {
        public ParlayBaseHandler(
            ParlayService parlayService,
            ITicketHelper ticketHelper)
        {
            ParlayService = parlayService;
            TicketHelper = ticketHelper;
        }

        protected ITicketHelper TicketHelper { get; }

        protected ParlayService ParlayService { get; }

        protected async Task<ComboParlayDetailViewModel> GetComboParlayDetail(ComboParlayDetailQuery message)
        {
            var comboParlayDetailData = await ParlayService.GetComboParlayDetail(
                message.TransId,
                message.RefNo,
                message.BetId,
                message.CustId,
                message.Language);

            return new ComboParlayDetailViewModel(message, comboParlayDetailData, TicketHelper);
        }
    }
}