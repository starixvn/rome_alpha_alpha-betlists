namespace Alpha.BetLists.Site.Handlers.RunningBetList
{
    using BetLists.Models;
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Params;
    using Requests.RunningBetList;
    using Services;
    using SmartPunter.Services;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Utilities.IP;
    using ViewModels.RunningBetList;

    public class RunningBetListHandler : BetListBaseHandler, IAsyncRequestHandler<RunningBetListQuery, RunningBetListViewModel>
    {
        private readonly RunningBetListService runningBetListService;
        private readonly ProductService productService;
        private readonly ForeCastService foreCastService;

        public RunningBetListHandler(
            RunningBetListService oustandingService,
            ProductService productService,
            ForeCastService foreCastService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService)
            : base(builder, excelBuilder, ticketHelper, monitorService, ipService)
        {
            this.runningBetListService = oustandingService;
            this.productService = productService;
            this.foreCastService = foreCastService;
        }

        public async Task<RunningBetListViewModel> Handle(RunningBetListQuery message)
        {
            var validUserSelectedProductIds = productService.FilterProductIdsByCustomer(message.CustId, message.ProductIds.ToListInt());

            var runningBetListParam = new RunningBetListParam(
                message.CustId,
                validUserSelectedProductIds,
                message.FromDate,
                message.ToDate,
                message.GetAll,
                message.Language,
                message.CustName);

            var betlistData = await runningBetListService.GetBetList(runningBetListParam);
            var products = productService.GetProductsForCustomer(message.CustId);
            var supportScoreMapBettypeIds = foreCastService.GetBetTypeIdsSupportScoreMap();

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

            return new RunningBetListViewModel(
                message,
                runningBetListParam,
                betlistData,
                BuildProductSelectListItems(runningBetListParam.ProductIds, products),
                supportScoreMapBettypeIds,
                Builder,
                TicketHelper);
        }

        private IEnumerable<SelectListItem> BuildProductSelectListItems(
            IEnumerable<int> userSelectedProductIds,
            IEnumerable<Product> products)
        {
            if (products == null
                || !products.Any())
            {
                return Enumerable.Empty<SelectListItem>();
            }

            return products.Select(product =>
            {
                var productSelectListItem = new SelectListItem
                {
                    Text = product.DisplayName,
                    Value = product.Id.ToString(),
                    Selected = userSelectedProductIds.Contains(product.Id)
                };

                return productSelectListItem;
            });
        }
    }
}