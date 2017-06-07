namespace Alpha.BetLists.Site.Handlers.ParlayWinLossByMatch
{
    using System.Threading.Tasks;
    using AppResources;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Requests.WinLossByMatch;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels;
    using ViewModels.ParlayWinLossByMatch;
    using WinLossByMatch;

    public class ParlayWinLossByMatchExcelHanlder
        : BetListExcelBaseHandler,
        IAsyncRequestHandler<ParlayWinLossByMatchExcelQuery, ExcelFileViewModel>
    {
        private readonly ParlayWinLossByMatchService parlayWinLossByMatchService;
        private readonly Workbook<TicketDto> workbook;
        private readonly WinLossByMatchExcelHandler winLossByMatchExcelHandler;

        public ParlayWinLossByMatchExcelHanlder(
            ParlayWinLossByMatchService parlayWinLossByMatchService,
            ISmartPunterBetListMonitor monitorService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            IPService ipService,
            Workbook<TicketDto> workbook,
            WinLossByMatchExcelHandler winLossByMatchExcelHandler = null)
            : base(builder, excelBuilder, ticketHelper, monitorService, ipService)
        {
            this.workbook = workbook;
            this.winLossByMatchExcelHandler = winLossByMatchExcelHandler ?? new WinLossByMatchExcelHandler(
                null,
                builder,
                excelBuilder,
                ticketHelper,
                monitorService,
                ipService,
                workbook);
            this.parlayWinLossByMatchService = parlayWinLossByMatchService;
        }

        public async Task<ExcelFileViewModel> Handle(ParlayWinLossByMatchExcelQuery message)
        {
            var winLossParam = ParlayWinLossByMatchHandler.CreateWinLossParam(message);
            var tickets = await parlayWinLossByMatchService.GetBetList(winLossParam);

            var viewModel = new ParlayWinLossByMatchViewModel(message, new BetListData(tickets, null), null, TicketHelper);

            winLossByMatchExcelHandler.SetCurrentRowIndex(rowIndex: 1);

            var fileContent = workbook
                .UseTemplate(winLossByMatchExcelHandler.GetTemplateInfo(message.CustLevel))
                .Header(Resources.ResourceManager.GetString)
                .Title(viewModel.Title)
                .Body(tickets, winLossByMatchExcelHandler.GetBodyRowMapper(message.ViewerLevel, viewModel))
                .Footer(new TicketDto(), winLossByMatchExcelHandler.GetFooterRowMapper(viewModel.Footer))
                .Sheet(Resources.BetList)
                .ExportToBytes();

            return new ExcelFileViewModel(viewModel.Title, fileContent);
        }
    }
}