namespace Alpha.BetLists.Site.Handlers.CancelledBet
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.BetLists.AppResources;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Params;
    using Alpha.BetLists.Services;
    using Alpha.BetLists.Site.Requests.CancelledBet;
    using Alpha.BetLists.Site.ViewModels;
    using Alpha.BetLists.Site.ViewModels.CancelledBet;
    using Alpha.SmartPunter.Services;
    using Alpha.Utilities.IP;
    using BetLists.Constants;
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;

    public class CancelledBetExcelHandler : BetListExcelBaseHandler, IAsyncRequestHandler<CancelledBetExcelQuery, ExcelFileViewModel>
    {
        private readonly CancelledBetService service;
        private readonly ProductService productService;
        private readonly ReportDateService reportDateService;

        private Workbook<TicketDto> workbook;

        public CancelledBetExcelHandler(
            CancelledBetService service,
            ProductService productService,
            ReportDateService reportDateService,
            ISmartPunterBetListMonitor monitorService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            IPService ipService,
            Workbook<TicketDto> workbook) : base(builder, excelBuilder, ticketHelper, monitorService, ipService)
        {
            this.service = service;
            this.workbook = workbook;
            this.productService = productService;
            this.reportDateService = reportDateService;
        }

        public async Task<ExcelFileViewModel> Handle(CancelledBetExcelQuery query)
        {
            var validUserSelectedProductIds = productService.FilterProductIdsByCustomer(query.CustId, query.MinDate, query.ProductIds.ToListInt());
            var products = productService.GetProductsForCustomer(query.CustId, query.MinDate);
            var parameter = new CancelledBetParam(
                query.CustId,
                query.FromDate,
                query.ToDate,
                reportDateService.GetMoveBetsDay(),
                query.ViewerLevelId,
                validUserSelectedProductIds,
                query.StatusIds,
                query.Language,
                DefaultVariables.MaximumnNumberRowExcelFile,
                query.PageIndex);
            var cancelledBet = await service.GetCancelledBet(parameter);

            var viewModel = new CancelledBetViewModel(
                query,
                parameter,
                cancelledBet,
                BuildProductSelectListItems(parameter.ProductIds, products),
                Builder,
                TicketHelper);

            workbook = workbook.UseTemplate(GetTemplateInfo(query.ViewerLevel));
            RtfHelper = query.RtfHelper ?? InitRTFHelper((workbook as NpoiWorkbook<TicketDto>).Workbook);
            var fileContent = workbook
                .Header(Resources.ResourceManager.GetString)
                .Title(GetExcelTitle(viewModel))
                .Body(cancelledBet.DataSource.FirstOrDefault().Tickets, GetBodyRowMapper(query.ViewerLevel, viewModel))
                .ExportToBytes();

            return new ExcelFileViewModel(Resources.CancelledBet, fileContent);
        }

        protected internal override string ExcelTemplatePath(UserLevel viewerLevel)
           => GetExcelTemplatePath(viewerLevel.DisplayName);

        protected internal override Mapper<TicketDto> GetBodyRowMapper(UserLevel viewerLevel, BetListBaseViewModel viewModel)
        {
            return GetTicketDtoRowMapperIncludeUsername(viewModel, showWinLoss: false);
        }

        private string GetExcelTitle(CancelledBetViewModel viewModel)
        {
            var statuses = new List<StatusModel>
                {
                    new StatusModel(TicketStatusIdConstants.Void, Resources.voided),
                    new StatusModel(TicketStatusIdConstants.Reject, Resources.reject),
                    new StatusModel(TicketStatusIdConstants.Refund, Resources.refund)
                };

            var status = statuses.Where(i => viewModel.StatusIds.Contains(i.Id.ToString())).Select(i => i.Name).JoinWithSlash();

            return $"{status} {Resources.BetList} {viewModel.FromDate.ToShortDateWithSlash()} --> {viewModel.ToDate.ToShortDateWithSlash()} " +
                $"{Resources.of} {productService.ConcatProductNames(viewModel.ProductsId)}.";
        }
    }
}