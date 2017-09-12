namespace Alpha.BetLists.Site.Handlers.BetDetail
{
    using System.Threading.Tasks;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Params.BetDetail;
    using Requests.BetDetail;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels.BetDetail;

    public class BetDetailHander : BetListBaseHandler, IAsyncRequestHandler<RNGCasinoDetailQuery, RNGCasinoDetailViewModel>, IAsyncRequestHandler<AllbetDetailQuery, AllbetDetailViewModel>
    {
        private readonly BetDetailService betDetailService;

        public BetDetailHander(
            BetDetailService betDetailService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService)
            : base(builder, ticketHelper, monitorService, ipService)
        {
            this.betDetailService = betDetailService;
        }

        public async Task<AllbetDetailViewModel> Handle(AllbetDetailQuery message)
        {
            var param = new BetDetailParam(
                message.RefNo,
                message.CustId,
                message.TransType,
                message.PageIndex,
                message.PageSize);

            var tickets = await betDetailService.GetAllbetDetails(param);
            var viewModel = new AllbetDetailViewModel(message, new BetListData(tickets, null), Builder, TicketHelper);
            viewModel.TotalRows = param.TotalRows;

            return viewModel;
        }

        public async Task<RNGCasinoDetailViewModel> Handle(RNGCasinoDetailQuery message)
        {
            var param = new BetDetailParam(
                message.RefNo,
                message.CustId,
                message.TransType,
                message.PageIndex,
                message.PageSize);

            var tickets = await betDetailService.GetRNGCasinoDetails(param);

            var viewModel = new RNGCasinoDetailViewModel(message, new BetListData(tickets, null), Builder, TicketHelper);
            viewModel.TotalRows = param.TotalRows;

            return viewModel;
        }
    }
}
