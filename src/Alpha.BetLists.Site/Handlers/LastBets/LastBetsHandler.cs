namespace Alpha.BetLists.Site.Handlers.LastBets
{
    using System.Linq;
    using System.Threading.Tasks;
    using AppResources;
    using BetLists.Constants;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Params.LastBets;
    using Requests.LastBets;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels.LastBets;
    using Web.Models;

    public class LastBetsHandler :
        BetListBaseHandler,
        IRequestHandler<LastBetsQuery, LastBetsViewModel>,
        IAsyncRequestHandler<GetLastBetsQuery, BetDataViewModel>,
        IRequestHandler<CheckCanViewLastBetsQuery, Result>
    {
        private readonly LastBetsService lastBetsService;
        private readonly IBetListHTMLBuilder builder;
        private readonly ITicketHelper ticketHelper;
        private readonly ForecastService forecastService;
        private readonly EncryptionService encryptionService;

        public LastBetsHandler(
            LastBetsService lastBetsService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ForecastService forecastService,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            EncryptionService encryptionService) :
            base(builder, ticketHelper, monitorService, ipService)
        {
            this.lastBetsService = lastBetsService;
            this.builder = builder;
            this.ticketHelper = ticketHelper;
            this.forecastService = forecastService;
            this.encryptionService = encryptionService;
        }

        public LastBetsViewModel Handle(LastBetsQuery message)
            => new LastBetsViewModel(
                message,
                null,
                builder,
                ticketHelper,
                encryptionService.CfsEncode($"LastTransId_{message.CustId}", numberOfEncodeResult: 6),
                encryptionService.CfsEncode($"LatestTransId_{message.CustId}", numberOfEncodeResult: 6));

        public async Task<BetDataViewModel> Handle(GetLastBetsQuery message)
        {
            var isManualLoad = !message.IsAutoLoad;

            var param = new GetLastBetsParam(message.BetType, message.CustId, message.TransId, message.PageSize);
            var tickets = await lastBetsService.GetLastBets(param);
            var betListData = new BetListData(tickets, null);
            var supportScoreMapBetTypeIds = forecastService.GetBetTypeIdsSupportScoreMap();

            var previousLatestTransId = isManualLoad ? 0 : message.LatestTransId;

            var newTickets = tickets.Where(t => t.TransId > previousLatestTransId);

            if (isManualLoad || newTickets.Any())
            {
                WriteLog(message.CustId, newTickets);
            }

            var latestTransId = newTickets.Any() ? newTickets.First().TransId : message.LatestTransId;

            return new BetDataViewModel(latestTransId, message, betListData, builder, ticketHelper, supportScoreMapBetTypeIds);
        }

        public Result Handle(CheckCanViewLastBetsQuery message)
        {
            var canViewLastBets = lastBetsService.CanViewLastBets(
                new CheckCanViewLastBetsParam(
                    message.CustId,
                    message.IsInternal,
                    message.IsEnableLastBetsMonitoring,
                    message.UsersAlwaysAllowAccessBetsListMonitoring));

            var errorCode = canViewLastBets ? ErrorCodeConstants.None : ErrorCodeConstants.FunctionTurnOff;
            var errorMessage = canViewLastBets ? string.Empty : Resources.FunctionTurnOffError;

            return new Result(errorCode, errorMessage);
        }
    }
}