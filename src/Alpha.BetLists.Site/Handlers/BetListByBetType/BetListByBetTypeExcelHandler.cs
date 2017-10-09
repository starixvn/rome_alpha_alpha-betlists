namespace Alpha.BetLists.Site.Handlers.BetListByBetType
{
    using System.Threading.Tasks;
    using AppResources;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using Requests.BetListByBetType;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels;
    using ViewModels.BetListByBetType;

    public class BetListByBetTypeExcelHandler : BetListByBetTypeExcelBaseHandler, IAsyncRequestHandler<BetListByBetTypeExcelQuery, ExcelFileViewModel>
    {
        private Workbook<TicketDto> workbook;
        private readonly RunningBetListByBetTypeService runningBetListByBetTypeService;

        public BetListByBetTypeExcelHandler(
            RunningBetListByBetTypeService runningBetListByBetTypeService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            Workbook<TicketDto> workbook)
            : base(builder, excelBuilder, ticketHelper, monitorService, ipService)
        {
            this.workbook = workbook;
            this.runningBetListByBetTypeService = runningBetListByBetTypeService;
        }

        public async Task<ExcelFileViewModel> Handle(BetListByBetTypeExcelQuery message)
        {
            var param = BetListByBetTypeHandler.CreateRunningBetListByBetTypeParam(message);
            var tickets = await runningBetListByBetTypeService.GetBetList(param);

            var viewModel = new BetListByBetTypeViewModel(message, new BetListData(tickets, null), null, TicketHelper);

            workbook = workbook.UseTemplate(GetTemplateInfo(message.ViewerLevel));
            RtfHelper = message.RtfHelper ?? InitRTFHelper((workbook as NpoiWorkbook<TicketDto>).Workbook);

            var fileContent = workbook
                .Header(Resources.ResourceManager.GetString)
                .Title(viewModel.Title)
                .Body(tickets, GetBodyRowMapper(message.ViewerLevel, viewModel))
                .ExportToBytes();

            return new ExcelFileViewModel(viewModel.Title, fileContent);
        }
    }
}
