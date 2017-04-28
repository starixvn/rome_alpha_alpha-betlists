using System.Threading.Tasks;
using Alpha.BetLists.AppResources;
using Alpha.BetLists.Models;
using Alpha.BetLists.Services;
using Alpha.BetLists.Site.Handlers.WinLossByMatch;
using Alpha.BetLists.Site.Requests.WinLossByMatch;
using Alpha.BetLists.Site.ViewModels;
using Alpha.BetLists.Site.ViewModels.ParlayWinLossByMatch;
using Alpha.SmartPunter.Services;
using Alpha.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;
using ONEbook.SpreadSheets;

namespace Alpha.BetLists.Site.Handlers.ParlayWinLossByMatch
{
    public class ParlayWinLossByMatchExcelHanlder
        : ParlayWinLossByMatchBaseHandler,
        IAsyncRequestHandler<ParlayWinLossByMatchExcelQuery, ExcelFileViewModel>
    {
        private readonly Workbook<ITicket> workbook;
        private readonly WinLossByMatchExcelHandler winLossByMatchExcelHandler;

        public ParlayWinLossByMatchExcelHanlder(
            ParlayWinLossByMatchService winLossService,
            ISmartPunterBetListMonitor monitorService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            IPService ipService,
            Workbook<ITicket> workbook,
            WinLossByMatchExcelHandler winLossByMatchExcelHandler = null)
            : base(winLossService, monitorService, builder, excelBuilder, ticketHelper, ipService)
        {
            this.workbook = workbook;
            this.winLossByMatchExcelHandler = winLossByMatchExcelHandler ?? new WinLossByMatchExcelHandler(
                null,
                monitorService,
                builder,
                excelBuilder,
                ticketHelper,
                ipService,
                workbook);
        }

        public async Task<ExcelFileViewModel> Handle(ParlayWinLossByMatchExcelQuery message)
        {
            var winLossParam = CreateWinLossParam(message);
            var tickets = await WinLossService.GetBetList(winLossParam);

            var viewModel = new ParlayWinLossByMatchViewModel(message, new BetListData(tickets, null), null, TicketHelper);

            winLossByMatchExcelHandler.SetCurrentRowIndex(rowIndex: 1);

            var fileContent = workbook
                .UseTemplate(winLossByMatchExcelHandler.GetTemplateInfo(message.CustLevel))
                .Header(Resources.ResourceManager.GetString)
                .Title(viewModel.Title)
                .Body(tickets, winLossByMatchExcelHandler.GetBodyRowMapper(message.ViewerLevel, viewModel))
                .Footer(new Ticket(), winLossByMatchExcelHandler.GetFooterRowMapper(viewModel.Footer))
                .Sheet(Resources.BetList)
                .ExportToBytes();

            return new ExcelFileViewModel(viewModel.Title, fileContent);
        }
    }
}