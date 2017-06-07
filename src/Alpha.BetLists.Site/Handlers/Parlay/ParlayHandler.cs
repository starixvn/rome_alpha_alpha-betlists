namespace Alpha.BetLists.Site.Handlers.Parlay
{
    using System.Threading.Tasks;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Requests.Parlay;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels.Parlay;

    public class ParlayHandler : BetListBaseHandler,
        IAsyncRequestHandler<MixParlayQuery, MixParlayViewModel>,
        IAsyncRequestHandler<ComboParlayQuery, ComboParlayViewModel>,
        IAsyncRequestHandler<ComboParlayDetailQuery, ComboParlayDetailViewModel>
    {
        private readonly ParlayService parlayService;

        public ParlayHandler(
            ParlayService parlayService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService)
            : base(builder, ticketHelper, monitorService, ipService)
        {
            this.parlayService = parlayService;
        }

        public async Task<MixParlayViewModel> Handle(MixParlayQuery message)
        {
            var tickets = await parlayService.GetParlayBetList(message.TransId, message.Language);

            return new MixParlayViewModel(message, new BetListData(tickets, null), Builder, TicketHelper);
        }

        public async Task<ComboParlayViewModel> Handle(ComboParlayQuery message)
        {
            var tickets = await parlayService.GetParlayBetList(message.TransId, message.Language);

            return new ComboParlayViewModel(message, new BetListData(tickets, null), Builder, TicketHelper);
        }

        public async Task<ComboParlayDetailViewModel> Handle(ComboParlayDetailQuery message)
        {
            var comboParlayDetailData = await parlayService.GetComboParlayDetail(
                          message.TransId,
                          message.RefNo,
                          message.BetId,
                          message.CustId,
                          message.Language);

            return new ComboParlayDetailViewModel(message, comboParlayDetailData, TicketHelper, isExportingExcel: false);
        }
    }
}