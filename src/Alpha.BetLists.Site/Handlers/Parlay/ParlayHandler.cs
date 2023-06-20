namespace Alpha.BetLists.Site.Handlers.Parlay
{
    using System.Threading.Tasks;
    using Alpha.BetLists.Site.Helpers;
    using Alpha.IPInfo.Services;
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
        private readonly BrowserHelper browserHelper;

        public ParlayHandler(
            ParlayService parlayService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            BrowserHelper browserHelper,
            IIPInfoService ipInfoService)
            : base(builder, ticketHelper, monitorService, ipService, ipInfoService)
        {
            this.parlayService = parlayService;
            this.browserHelper = browserHelper;
        }

        public async Task<MixParlayViewModel> Handle(MixParlayQuery message)
        {
            var tickets = await parlayService.GetParlayBetList(message.TransId, message.CustId);

            return new MixParlayViewModel(message, new BetListData(tickets, null), Builder, TicketHelper);
        }

        public async Task<ComboParlayViewModel> Handle(ComboParlayQuery message)
        {
            var tickets = await parlayService.GetParlayBetList(message.TransId, message.CustId);

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

            return new ComboParlayDetailViewModel(message, comboParlayDetailData, TicketHelper, browserHelper.IsMobile(false), isExportingExcel: false);
        }
    }
}