namespace Alpha.BetLists.Site.Handlers.CasinoDetails
{
    using Alpha.IPInfo.Services;
    using AlphaPlus.SmartPunter.Services;
    using AlphaPlus.Utilities.IP;
    using Fanex.BetDetails.Casino.Params;
    using Fanex.BetDetails.Casino.Services;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Requests.CasinoDetails;
    using System.Threading.Tasks;
    using ViewModels.CasinoDetails;

    public class CasinoDetailsHandler : BetListBaseHandler,
        IAsyncRequestHandler<CasinoDetailsQuery, CasinoDetailsViewModel>,
        IAsyncRequestHandler<LiveCasinoDetailsQuery, CasinoDetailsViewModel>,
        IAsyncRequestHandler<RNGCasinoDetailsQuery, CasinoDetailsViewModel>
    {
        private readonly ICasinoDetailsService casinoDetailsService;

        public CasinoDetailsHandler(
            ICasinoDetailsService casinoDetailsService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            IIPInfoService ipInfoService)
            : base(builder, ticketHelper, monitorService, ipService, ipInfoService)
        {
            this.casinoDetailsService = casinoDetailsService;
        }

        public async Task<CasinoDetailsViewModel> Handle(CasinoDetailsQuery message)
        {
            var param = CreateCasinoDetailsParam(message);
            var betlistData = casinoDetailsService.GetCasinoDetails(param);
            await AssignCountry(betlistData.Tickets, message.DisplayVNIP);

            return new CasinoDetailsViewModel(message, param, betlistData, Builder, TicketHelper);
        }

        public async Task<CasinoDetailsViewModel> Handle(LiveCasinoDetailsQuery message)
        {
            var param = CreateCasinoDetailsParam(message);
            var betlistData = casinoDetailsService.GetLiveCasinoDetails(param);
            await AssignCountry(betlistData.Tickets, message.DisplayVNIP);

            return new CasinoDetailsViewModel(message, param, betlistData, Builder, TicketHelper, "LiveCasinoExcel");
        }

        public async Task<CasinoDetailsViewModel> Handle(RNGCasinoDetailsQuery message)
        {
            var param = CreateCasinoDetailsParam(message);
            var betlistData = casinoDetailsService.GetRNGCasinoDetails(param);
            await AssignCountry(betlistData.Tickets, message.DisplayVNIP);

            return new CasinoDetailsViewModel(message, param, betlistData, Builder, TicketHelper, "RNGExcel");
        }

        protected internal static CasinoDetailsParam CreateCasinoDetailsParam(CasinoDetailsQuery message)
            => new CasinoDetailsParam(
                message.RefNo,
                message.CustId,
                message.TransType,
                message.ProductId,
                message.PageIndex,
                message.PageSize);
    }
}
