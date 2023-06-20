namespace Alpha.BetLists.Site.Handlers.CombinationMixParlay
{
    using System.Threading.Tasks;
    using Alpha.IPInfo.Services;
    using BetListByBetType;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Requests.CombinationMixParlay;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels.CombinationMixParlay;

    public class CombinationMixParlayHandler : BetListBaseHandler,
        IAsyncRequestHandler<CombinationMixParlayQuery, CombinationMixParlayViewModel>
    {
        private readonly ForecastService forecastService;
        private readonly RunningBetListByBetTypeService runningBetListByBetTypeService;

        public CombinationMixParlayHandler(
            RunningBetListByBetTypeService runningBetListByBetTypeService,
            ForecastService forecastService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            IIPInfoService ipInfoService)
            : base(builder, ticketHelper, monitorService, ipService, ipInfoService)
        {
            this.forecastService = forecastService;
            this.runningBetListByBetTypeService = runningBetListByBetTypeService;
        }

        public async Task<CombinationMixParlayViewModel> Handle(CombinationMixParlayQuery message)
        {
            var param = BetListByBetTypeHandler.CreateRunningBetListByBetTypeParam(message);

            var tickets = await runningBetListByBetTypeService.GetBetList(param);
            await AssignCountry(tickets, message.DisplayVNIP);
            var supportScoreMapBetTypeIds = forecastService.GetBetTypeIdsSupportScoreMap();

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, tickets);

            return new CombinationMixParlayViewModel(
                message,
                new BetListData(tickets, null),
                Builder,
                TicketHelper,
                supportScoreMapBetTypeIds);
        }
    }
}
