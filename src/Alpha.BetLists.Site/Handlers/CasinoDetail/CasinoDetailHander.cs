namespace Alpha.BetLists.Site.Handlers.CasinoDetail
{
    using System.Threading.Tasks;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Params;
    using Requests.CasioDetail;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels.CasinoDetail;

    public class CasinoDetailHander : BetListBaseHandler, IAsyncRequestHandler<CasioDetailQuery, CasinoDetailViewModel>
    {
        private readonly CasinoDetailService casinoDetailService;

        public CasinoDetailHander(
            CasinoDetailService casinoDetailService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService)
            : base(builder, ticketHelper, monitorService, ipService)
        {
            this.casinoDetailService = casinoDetailService;
        }

        public async Task<CasinoDetailViewModel> Handle(CasioDetailQuery message)
        {
            var param = new CasinoDetailParam(
                message.RefNo,
                message.CustId,
                message.TransType,
                message.ProductId,
                message.PageIndex,
                message.PageSize);

            var tickets = await casinoDetailService.GetCasinoDetails(param);

            var viewModel = new CasinoDetailViewModel(message, new BetListData(tickets, null), Builder, TicketHelper);
            viewModel.TotalRows = param.TotalRows;

            return viewModel;
        }
    }
}
