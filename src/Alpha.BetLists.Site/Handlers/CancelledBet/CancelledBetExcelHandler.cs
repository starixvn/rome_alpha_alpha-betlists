namespace Alpha.BetLists.Site.Handlers.CancelledBet
{
    using Alpha.BetLists.AppResources;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Params;
    using Alpha.BetLists.Services;
    using Alpha.BetLists.Site.Requests.CancelledBet;
    using Alpha.BetLists.Site.ViewModels;
    using Alpha.BetLists.Site.ViewModels.CancelledBet;
    using Alpha.IPInfo.Services;
    using AlphaPlus.SmartPunter.Services;
    using AlphaPlus.Utilities.IP;
    using BetLists.Constants;
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.BetListNPOI;
    using static ONEbook.SpreadSheets.Column;
    using System.Linq;
    using System.Threading.Tasks;

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
            Workbook<TicketDto> workbook,
            IIPInfoService ipInfoService) : base(builder, excelBuilder, ticketHelper, monitorService, ipService, ipInfoService)
        {
            this.service = service;
            this.workbook = workbook;
            this.productService = productService;
            this.reportDateService = reportDateService;
        }

        public async Task<ExcelFileViewModel> Handle(CancelledBetExcelQuery query)
        {
            var validUserSelectedProductIds = productService.FilterProductIdsByCustomer(query.ViewerId, 
                query.Site, query.CurrencyId, query.MinDate, query.ProductIds.ToListInt(), query.IsEnableSabaPromotion);
            var products = productService.GetProductsForCustomer(query.ViewerId, query.Site, query.CurrencyId, query.MinDate, query.IsEnableSabaPromotion);
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
            var betlistData = cancelledBet.DataSource.FirstOrDefault();
            if (betlistData != null)
            {
                await AssignCountry(betlistData.Tickets, query.DisplayVNIP);
            }

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
            var fileName = $"{Resources.CancelledBet}-{query.CustName}-{viewModel.FromDate.ToShortDate()}-{viewModel.ToDate.ToShortDate()}";

            return new ExcelFileViewModel(fileName, fileContent);
        }

        protected internal override string ExcelTemplatePath(UserLevel viewerLevel)
           => GetExcelTemplatePath(viewerLevel.DisplayName);

        protected internal override Mapper<TicketDto> GetBodyRowMapper(UserLevel viewerLevel, BetListBaseViewModel viewModel)
        {
            var mapper = GetTicketDtoRowMapperIncludeUsername(viewModel, showWinLoss: false);
            mapper.Map(H, ticket => BuildPlatform(ticket.Platform));

            return mapper;
        }

        private string GetExcelTitle(CancelledBetViewModel viewModel)
        {
            var statuses = new List<StatusModel>
                {
                    new StatusModel(TicketStatusIdConstants.Void, Resources.voided),
                    new StatusModel(TicketStatusIdConstants.Reject, Resources.rejected),
                    new StatusModel(TicketStatusIdConstants.Refund, Resources.refunded)
                };

            var status = statuses.Where(i => viewModel.StatusIds.Contains(i.Id.ToString())).Select(i => i.Name).JoinWithSlash();

            return $"{status} {Resources.BetList} {viewModel.FromDate.ToShortDateWithSlash()} --> {viewModel.ToDate.ToShortDateWithSlash()} " +
                $"{Resources.of} {productService.ConcatProductNames(viewModel.ProductsId)}.";
        }
    }
}