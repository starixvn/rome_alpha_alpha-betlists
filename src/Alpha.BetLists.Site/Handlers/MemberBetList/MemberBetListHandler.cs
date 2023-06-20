namespace Alpha.BetLists.Site.Handlers.MemberBetList
{
    using System.Threading.Tasks;
    using Alpha.IPInfo.Services;
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Params;
    using Requests.MemberBetList;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels.MemberBetList;

    public class MemberBetListHandler : BetListBaseHandler, IAsyncRequestHandler<MemberBetListQuery, MemberBetListViewModel>
    {
        private readonly MemberBetListService memberBetListService;
        private readonly ProductService productService;
        private readonly ForecastService forecastService;

        public MemberBetListHandler(
            MemberBetListService memberBetListService,
            ProductService productService,
            ForecastService forecastService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            IIPInfoService ipInfoService)
            : base(builder, ticketHelper, monitorService, ipService, ipInfoService)
        {
            this.memberBetListService = memberBetListService;
            this.productService = productService;
            this.forecastService = forecastService;
        }

        public async Task<MemberBetListViewModel> Handle(MemberBetListQuery message)
        {
            var validUserSelectedProductIds = productService.FilterProductIdsByCustomer(message.ViewerId, 
                message.Site, message.CurrencyId, message.MinDate, message.ProductIds.ToListInt(), message.IsEnableSabaPromotion);

            var memberBetListParam = new MemberBetListParam(
                message.CustId,
                validUserSelectedProductIds,
                message.FromDate,
                message.ToDate,
                message.LimitedDateToShowInfo,
                message.Language,
                message.CustName);

            var betlistData = await memberBetListService.GetBetList(memberBetListParam);
            await AssignCountry(betlistData.Tickets, message.DisplayVNIP);
            var products = productService.GetProductsForCustomer(message.ViewerId, message.Site, message.CurrencyId, message.MinDate, message.IsEnableSabaPromotion);
            var supportScoreMapBetTypeIds = forecastService.GetBetTypeIdsSupportScoreMap();

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

            return new MemberBetListViewModel(
                message,
                memberBetListParam,
                betlistData,
                BuildProductSelectListItems(memberBetListParam.ProductIds, products),
                supportScoreMapBetTypeIds,
                Builder,
                TicketHelper);
        }
    }
}
