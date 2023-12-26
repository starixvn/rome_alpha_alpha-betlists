namespace Alpha.BetLists.Site.Handlers.MemberBetList
{
    using Alpha.IPInfo.Services;
    using AlphaPlus.SmartPunter.Services;
    using AlphaPlus.Utilities.IP;
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Params;
    using Requests.MemberBetList;
    using Services;
    using System.Threading.Tasks;
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
            var validUserSelectedProductIds = productService.FilterProductIds(message.Site, message.CurrencyId, message.MinDate, message.ProductIds.ToListInt());

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
            var products = productService.GetAllOfferedProducts(message.Site, message.CurrencyId, message.MinDate);
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
