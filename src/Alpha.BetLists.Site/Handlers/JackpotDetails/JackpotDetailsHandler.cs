namespace Alpha.BetLists.Site.Handlers.JackpotDetails
{
    using System.Threading.Tasks;
    using Alpha.IPInfo.Services;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Params;
    using Requests.JackpotDetails;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels.JackpotDetails;

    public class JackpotDetailsHandler : BetListBaseHandler, IAsyncRequestHandler<JackpotDetailsQuery, JackpotDetailsViewModel>
    {
        private readonly JackpotDetailsService jackpotDetailsService;

        public JackpotDetailsHandler(
            JackpotDetailsService jackpotDetailsService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            IIPInfoService ipInfoService)
            : base(builder, ticketHelper, monitorService, ipService, ipInfoService)
        {
            this.jackpotDetailsService = jackpotDetailsService;
        }

        public async Task<JackpotDetailsViewModel> Handle(JackpotDetailsQuery message)
        {
            var param = CreateJackpotDetailsParam(message);
            var tickets = await jackpotDetailsService.GetJackpotDetails(param);

            return new JackpotDetailsViewModel(message, new BetListData(tickets, null), Builder, TicketHelper);
        }

        protected internal static JackpotDetailsParam CreateJackpotDetailsParam(JackpotDetailsQuery message)
            => new JackpotDetailsParam(message.TransId, message.CustId, message.BetType);
    }
}