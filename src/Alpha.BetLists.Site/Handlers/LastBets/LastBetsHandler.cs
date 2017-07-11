using System.Threading.Tasks;
using Alpha.BetLists.AppResources;
using Alpha.BetLists.Constants;
using Alpha.BetLists.Models;
using Alpha.BetLists.Params.LastBets;
using Alpha.BetLists.Services;
using Alpha.BetLists.Site.Requests.LastBets;
using Alpha.BetLists.Site.ViewModels.LastBets;
using Alpha.SmartPunter.Services;
using Alpha.Utilities.IP;
using Alpha.Web.Models;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;

namespace Alpha.BetLists.Site.Handlers.LastBets
{
    public class LastBetsHandler :
        BetListBaseHandler,
        IRequestHandler<LastBetsQuery, LastBetsViewModel>,
        IAsyncRequestHandler<GetLastBetsQuery, BetDataViewModel>,
        IRequestHandler<CheckCanViewLastBetsQuery, Result>
    {
        private readonly LastBetsService lastBetsService;
        private readonly IBetListHTMLBuilder builder;
        private readonly ITicketHelper ticketHelper;
        private readonly ForeCastService foreCastService;
        private readonly EncryptionService encryptionService;

        public LastBetsHandler(
            LastBetsService lastBetsService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ForeCastService foreCastService,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            EncryptionService encryptionService) :
            base(builder, ticketHelper, monitorService, ipService)
        {
            this.lastBetsService = lastBetsService;
            this.builder = builder;
            this.ticketHelper = ticketHelper;
            this.foreCastService = foreCastService;
            this.encryptionService = encryptionService;
        }

        public LastBetsViewModel Handle(LastBetsQuery message)
            => new LastBetsViewModel(message, null, builder, ticketHelper, encryptionService.CfsEncode($"LastTransId_{message.CustId}", numberOfEncodeResult: 6));

        public async Task<BetDataViewModel> Handle(GetLastBetsQuery message)
        {
            var param = new GetLastBetsParam(message.BetType, message.CustId, message.TransId, message.PageSize);
            var tickets = await lastBetsService.GetLastBets(param);
            var betListData = new BetListData(tickets, null);
            var supportScoreMapBettypeIds = foreCastService.GetBetTypeIdsSupportScoreMap();

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, tickets);

            return new BetDataViewModel(message, betListData, builder, ticketHelper, supportScoreMapBettypeIds);
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