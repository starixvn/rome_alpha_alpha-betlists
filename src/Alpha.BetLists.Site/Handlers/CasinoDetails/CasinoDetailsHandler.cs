namespace Alpha.BetLists.Site.Handlers.CasinoDetails
{
    using Fanex.BetDetails.Casino.Params;
    using Fanex.BetDetails.Casino.Services;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Requests.CasinoDetails;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels.CasinoDetails;

    public class CasinoDetailsHandler : BetListBaseHandler, IRequestHandler<CasinoDetailsQuery, CasinoDetailsViewModel>
    {
        private readonly ICasinoDetailsService casinoDetailsService;

        public CasinoDetailsHandler(
            ICasinoDetailsService casinoDetailsService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService)
            : base(builder, ticketHelper, monitorService, ipService)
        {
            this.casinoDetailsService = casinoDetailsService;
        }

        public CasinoDetailsViewModel Handle(CasinoDetailsQuery message)
        {
            var param = CreateCasinoDetailsParam(message);

            var tickets = casinoDetailsService.GetCasinoDetails(param);

            return new CasinoDetailsViewModel(message, param, tickets, Builder, TicketHelper);
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
