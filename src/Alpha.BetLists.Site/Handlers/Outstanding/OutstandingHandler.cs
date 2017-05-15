namespace Alpha.BetLists.Site.Handlers.Outstanding
{
    using BetLists.Models;
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Params;
    using Requests.Outstanding;
    using Services;
    using SmartPunter.Services;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Utilities.IP;
    using ViewModels.Outstanding;

    public class OutstandingHandler : BetListBaseHandler, IAsyncRequestHandler<OutstandingQuery, OutstandingViewModel>
    {
        private readonly OutstandingService outstandingService;
        private readonly ProductService productService;

        public OutstandingHandler(
            OutstandingService oustandingService,
            ProductService productService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService)
            : base(builder, excelBuilder, ticketHelper, monitorService, ipService)
        {
            this.outstandingService = oustandingService;
            this.productService = productService;
        }

        public async Task<OutstandingViewModel> Handle(OutstandingQuery message)
        {
            var products = productService.GetProductsForCustomer(message.CustId);

            var outstandingParam = new OutstandingParam(
                message.CustId,
                message.ProductIds.ToListInt(),
                message.FromDate,
                message.ToDate,
                message.GetAll,
                message.Language,
                message.CustName);

            var betlistData = await outstandingService.GetBetList(outstandingParam);

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

            return new OutstandingViewModel(message, betlistData, BuildProductSelectListItems(outstandingParam.ProductIds, products), Builder, TicketHelper);
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