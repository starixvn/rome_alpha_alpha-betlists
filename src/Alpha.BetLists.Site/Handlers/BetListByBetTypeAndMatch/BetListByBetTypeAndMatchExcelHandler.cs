namespace Alpha.BetLists.Site.Handlers.BetListByBetTypeAndMatch
{
    using System.Threading.Tasks;
    using AppResources;
    using BetListByBetType;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using Requests.BetListByBetTypeAndMatch;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels;
    using ViewModels.BetListByBetTypeAndMatch;

    public class BetListByBetTypeAndMatchExcelHandler : BetListByBetTypeExcelBaseHandler,
        IAsyncRequestHandler<BetListByBetTypeAndMatchExcelQuery, ExcelFileViewModel>
    {
        private Workbook<TicketDto> workbook;
        private readonly RunningBetListByBetTypeAndMatchService runningBetListByBetTypeAndMatchService;

        public BetListByBetTypeAndMatchExcelHandler(
            RunningBetListByBetTypeAndMatchService runningBetListByBetTypeAndMatchService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            Workbook<TicketDto> workbook)
            : base(builder, excelBuilder, ticketHelper, monitorService, ipService)
        {
            this.workbook = workbook;
            this.runningBetListByBetTypeAndMatchService = runningBetListByBetTypeAndMatchService;
        }

        public async Task<ExcelFileViewModel> Handle(BetListByBetTypeAndMatchExcelQuery message)
        {

            var param = BetListByBetTypeAndMatchHandler.CreateRunningBetListByBetTypeAndMatchParam(message);
            var betListData = await runningBetListByBetTypeAndMatchService.GetBetList(param);

            var viewModel = new BetListByBetTypeAndMatchViewModel(message, betListData, null, TicketHelper);

            workbook = workbook.UseTemplate(GetTemplateInfo(message.ViewerLevel));
            RtfHelper = message.RtfHelper ?? InitRTFHelper((workbook as NpoiWorkbook<TicketDto>).Workbook);

            var fileContent = workbook
                .Header(Resources.ResourceManager.GetString)
                .Title(viewModel.Title)
                .Body(betListData.Tickets, GetBodyRowMapper(message.ViewerLevel, viewModel))
                .ExportToBytes();

            return new ExcelFileViewModel(viewModel.Title, fileContent);
        }
    }
}
