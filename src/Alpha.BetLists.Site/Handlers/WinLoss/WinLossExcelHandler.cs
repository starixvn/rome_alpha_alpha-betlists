namespace Alpha.BetLists.Site.Handlers.WinLoss
{
    using System.Threading.Tasks;
    using AppResources;
    using BetLists.Models;
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using Params;
    using Requests.WinLoss;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels;
    using ViewModels.WinLoss;

    public class WinLossExcelHandler : BetListExcelBaseHandler, IAsyncRequestHandler<WinLossExcelQuery, ExcelFileViewModel>
    {
        private readonly WinLossService winLossService;
        private readonly ProductService productService;
        private Workbook<TicketDto> workbook;

        public WinLossExcelHandler(
            WinLossService winLossService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            Workbook<TicketDto> workbook,
            ProductService productService)
            : base(builder, excelBuilder, ticketHelper, monitorService, ipService)
        {
            this.workbook = workbook;
            this.winLossService = winLossService;
            this.productService = productService;
        }

        public async Task<ExcelFileViewModel> Handle(WinLossExcelQuery message)
        {
            var validUserSelectedProductIds = message.IsGroupHeadline
                ? productService.FilterProductIdsByGroupHeadline(message.ProductIds.ToListInt(), new GetProductsByGroupHeadlineParam(message.CustId, message.MinDate))
                : productService.FilterProductIdsByCustomer(message.CustId, message.ProductIds.ToListInt());

            var winLossParam = WinLossHandler.CreateWinLossParam(message, validUserSelectedProductIds);
            var betlistData = await winLossService.GetBetList(winLossParam);

            var winLossViewModel = new WinLossViewModel(message, betlistData, null, TicketHelper);

            workbook = workbook.UseTemplate(GetTemplateInfo(message.ViewerLevel));
            RtfHelper = message.RtfHelper ?? InitRTFHelper((workbook as NpoiWorkbook<TicketDto>).Workbook);

            var fileContent = workbook
                    .Header(Resources.ResourceManager.GetString)
                    .Title(winLossViewModel.Title)
                    .Body(betlistData.Tickets, GetBodyRowMapper(message.ViewerLevel, winLossViewModel))
                    .Footer(new TicketDto(), GetFooterRowMapper(winLossViewModel.Footer))
                    .ExportToBytes();

            return new ExcelFileViewModel(winLossViewModel.Title, fileContent);
        }
    }
}
