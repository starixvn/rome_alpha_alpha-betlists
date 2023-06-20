namespace Alpha.BetLists.Site.Handlers.MonitoringBets
{
    using System.Threading.Tasks;
    using Alpha.BetLists.Services;
    using Alpha.BetLists.Site.Requests.MonitoringBets;
    using Alpha.BetLists.Site.ViewModels.MonitoringBets;
    using Alpha.IPInfo.Services;
    using Alpha.SmartPunter.Services;
    using Alpha.Utilities.IP;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;

    public class MonitoringBetsHandler :
        BetListBaseHandler,
        IAsyncRequestHandler<MonitoringBetsQuery, MonitoringBetsViewModel>
    {
        private readonly MonitoringBetsService monitoringBetService;

        public MonitoringBetsHandler(
            MonitoringBetsService monitoringBetService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            IIPInfoService ipInfoService) : base(builder, ticketHelper, monitorService, ipService, ipInfoService)
        {
            this.monitoringBetService = monitoringBetService;
        }

        public Task<MonitoringBetsViewModel> Handle(MonitoringBetsQuery message)
        {
            return Task.FromResult(new MonitoringBetsViewModel(message, null, Builder, TicketHelper));
        }
    }
}