namespace Alpha.BetLists.Site.Handlers.CasinoDetails
{
    using System.Threading.Tasks;
    using Alpha.IPInfo.Services;
    using Fanex.BetDetails.Casino.Params;
    using Fanex.BetDetails.Casino.Services;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Requests.CasinoDetails;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels.CasinoDetails;

    public class CasinoDetailsHandler : BetListBaseHandler, IAsyncRequestHandler<CasinoDetailsQuery, CasinoDetailsViewModel>
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
