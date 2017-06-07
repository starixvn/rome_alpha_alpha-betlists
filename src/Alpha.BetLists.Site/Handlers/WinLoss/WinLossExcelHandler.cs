namespace Alpha.BetLists.Site.Handlers.WinLoss
{
    using System.Threading.Tasks;
    using AppResources;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Requests.WinLoss;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels;
    using ViewModels.WinLoss;

    public class WinLossExcelHandler : BetListExcelBaseHandler, IAsyncRequestHandler<WinLossExcelQuery, ExcelFileViewModel>
    {
        private readonly WinLossService winLossService;
        private readonly Workbook<TicketDto> workbook;

        public WinLossExcelHandler(
            WinLossService winLossService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            Workbook<TicketDto> workbook)
            : base(builder, excelBuilder, ticketHelper, monitorService, ipService)
        {
            this.workbook = workbook;
            this.winLossService = winLossService;
        }

        public async Task<ExcelFileViewModel> Handle(WinLossExcelQuery message)
        {
            var winLossParam = WinLossHandler.CreateWinLossParam(message);
            var betlistData = await winLossService.GetBetList(winLossParam);

            var winLossViewModel = new WinLossViewModel(message, betlistData, null, TicketHelper);

            var fileContent = workbook
                    .UseTemplate(GetTemplateInfo(message.ViewerLevel))
                    .Header(Resources.ResourceManager.GetString)
                    .Title(winLossViewModel.Title)
                    .Body(betlistData.Tickets, GetBodyRowMapper(message.ViewerLevel, winLossViewModel))
                    .Footer(new TicketDto(), GetFooterRowMapper(winLossViewModel.Footer))
                    .Sheet(Resources.BetList)
                    .ExportToBytes();

            return new ExcelFileViewModel(winLossViewModel.Title, fileContent);
        }
    }
}