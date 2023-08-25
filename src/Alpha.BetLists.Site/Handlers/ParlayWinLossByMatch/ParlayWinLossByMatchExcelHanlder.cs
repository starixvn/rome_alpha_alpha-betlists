namespace Alpha.BetLists.Site.Handlers.ParlayWinLossByMatch
{
    using Alpha.IPInfo.Services;
    using AlphaPlus.SmartPunter.Services;
    using AlphaPlus.Utilities.IP;
    using AppResources;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.BetListNPOI;
    using Requests.WinLossByMatch;
    using Services;
    using System.Threading.Tasks;
    using ViewModels;
    using ViewModels.ParlayWinLossByMatch;
    using WinLossByMatch;

    public class ParlayWinLossByMatchExcelHanlder
        : BetListExcelBaseHandler,
        IAsyncRequestHandler<ParlayWinLossByMatchExcelQuery, ExcelFileViewModel>
    {
        private readonly ParlayWinLossByMatchService parlayWinLossByMatchService;
        private readonly WinLossByMatchExcelHandler winLossByMatchExcelHandler;
        private Workbook<TicketDto> workbook;

        public ParlayWinLossByMatchExcelHanlder(
            ParlayWinLossByMatchService parlayWinLossByMatchService,
            ISmartPunterBetListMonitor monitorService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            IPService ipService,
            IIPInfoService ipInfoService,
            Workbook<TicketDto> workbook,
            WinLossByMatchExcelHandler winLossByMatchExcelHandler = null)
            : base(builder, excelBuilder, ticketHelper, monitorService, ipService, ipInfoService)
        {
            this.workbook = workbook;
            this.winLossByMatchExcelHandler = winLossByMatchExcelHandler ?? new WinLossByMatchExcelHandler(
                null,
                builder,
                excelBuilder,
                ticketHelper,
                monitorService,
                ipService,
                workbook,
                ipInfoService);
            this.parlayWinLossByMatchService = parlayWinLossByMatchService;
        }

        public async Task<ExcelFileViewModel> Handle(ParlayWinLossByMatchExcelQuery message)
        {
            var winLossParam = ParlayWinLossByMatchHandler.CreateWinLossParam(message);
            var tickets = await parlayWinLossByMatchService.GetBetList(winLossParam);
            await AssignCountry(tickets, message.DisplayVNIP);

            var viewModel = new ParlayWinLossByMatchViewModel(message, new BetListData(tickets, null), null, TicketHelper);

            workbook = workbook.UseTemplate(winLossByMatchExcelHandler.GetTemplateInfo(message.CustLevel));
            winLossByMatchExcelHandler.RtfHelper = message.RtfHelper ?? InitRTFHelper((workbook as NpoiWorkbook<TicketDto>).Workbook);

            var fileContent = workbook
                .Header(Resources.ResourceManager.GetString)
                .Title(viewModel.Title)
                .Body(tickets, winLossByMatchExcelHandler.GetBodyRowMapper(message.ViewerLevel, viewModel))
                .Footer(new TicketDto(), winLossByMatchExcelHandler.GetFooterRowMapper(viewModel.Footer))
                .ExportToBytes();

            return new ExcelFileViewModel(viewModel.Title, fileContent);
        }
    }
}
