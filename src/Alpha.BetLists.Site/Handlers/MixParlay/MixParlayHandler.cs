namespace Alpha.BetLists.Site.Handlers.MixParlay
{
    using System.Threading.Tasks;
    using BetListByBetType;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Requests.MixParlay;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels.MixParlay;

    public class MixParlayHandler : BetListBaseHandler, IAsyncRequestHandler<MixParlayQuery, MixParlayViewModel>
    {
        private readonly ForecastService forecastService;
        private readonly RunningBetListByBetTypeService runningBetListByBetTypeService;

        public MixParlayHandler(
            RunningBetListByBetTypeService runningBetListByBetTypeService,
            ForecastService forecastService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService)
            : base(builder, ticketHelper, monitorService, ipService)
        {
            this.forecastService = forecastService;
            this.runningBetListByBetTypeService = runningBetListByBetTypeService;
        }

        public async Task<MixParlayViewModel> Handle(MixParlayQuery message)
        {
            var param = BetListByBetTypeHandler.CreateRunningBetListByBetTypeParam(message);

            var tickets = await runningBetListByBetTypeService.GetBetList(param);
            var supportScoreMapBetTypeIds = forecastService.GetBetTypeIdsSupportScoreMap();

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, tickets);

            return new MixParlayViewModel(
                message,
                new BetListData(tickets, null),
                Builder,
                TicketHelper,
                supportScoreMapBetTypeIds);
        }
    }
}
