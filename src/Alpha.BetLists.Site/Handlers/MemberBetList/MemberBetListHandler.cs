namespace Alpha.BetLists.Site.Handlers.MemberBetList
{
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Params;
    using Requests.MemberBetList;
    using Services;
    using SmartPunter.Services;
    using System.Threading.Tasks;
    using Utilities.IP;
    using ViewModels.MemberBetList;

    public class MemberBetListHandler : BetListBaseHandler, IAsyncRequestHandler<MemberBetListQuery, MemberBetListViewModel>
    {
        private readonly MemberBetListService memberBetListService;
        private readonly ProductService productService;
        private readonly ForeCastService foreCastService;

        public MemberBetListHandler(
            MemberBetListService memberBetListService,
            ProductService productService,
            ForeCastService foreCastService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService)
            : base(builder, ticketHelper, monitorService, ipService)
        {
            this.memberBetListService = memberBetListService;
            this.productService = productService;
            this.foreCastService = foreCastService;
        }

        public async Task<MemberBetListViewModel> Handle(MemberBetListQuery message)
        {
            var validUserSelectedProductIds = productService.FilterProductIdsByCustomer(message.CustId, message.ProductIds.ToListInt());

            var memberBetListParam = new MemberBetListParam(
                message.CustId,
                validUserSelectedProductIds,
                message.FromDate,
                message.ToDate,
                message.Language,
                message.CustName);

            var betlistData = await memberBetListService.GetBetList(memberBetListParam);
            var products = productService.GetProductsForCustomer(message.CustId);
            var supportScoreMapBettypeIds = foreCastService.GetBetTypeIdsSupportScoreMap();

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

            return new MemberBetListViewModel(
                message,
                memberBetListParam,
                betlistData,
                BuildProductSelectListItems(memberBetListParam.ProductIds, products),
                supportScoreMapBettypeIds,
                Builder,
                TicketHelper);
        }
    }
}