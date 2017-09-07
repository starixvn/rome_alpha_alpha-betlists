namespace Alpha.BetLists.Site.Handlers.BetDetail
{
    using System.Threading.Tasks;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Params;
    using Requests.BetDetail;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels.BetDetail;

    public class BetDetailHander : BetListBaseHandler, IAsyncRequestHandler<RNGCasinoQuery, BetDetailViewModel>
    {
        private readonly RNGBetDetailService rngBetDetailService;

        public BetDetailHander(
            RNGBetDetailService rngBetDetailService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService)
            : base(builder, ticketHelper, monitorService, ipService)
        {
            this.rngBetDetailService = rngBetDetailService;
        }

        public async Task<BetDetailViewModel> Handle(RNGCasinoQuery message)
        {
            var param = new RNGBetDetailParam(
                message.RefNo,
                message.CustId,
                message.TransType,
                message.PageIndex,
                message.PageSize);

            var tickets = await rngBetDetailService.GetBetDetails(param);

            var viewModel = new BetDetailViewModel(message, new BetListData(tickets, null), Builder, TicketHelper);
            viewModel.TotalRows = param.TotalRows;

            return viewModel;
        }
    }
}
