namespace Alpha.BetLists.Site.Handlers.JackpotDetails
{
    using System.Threading.Tasks;
    using AppResources;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.BetListNPOI;
    using Requests.JackpotDetails;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels;
    using ViewModels.JackpotDetails;

    public class JackpotDetailsExcelHandler : BetListExcelBaseHandler, IAsyncRequestHandler<JackpotDetailsExcelQuery, ExcelFileViewModel>
    {
        private Workbook<TicketDto> workbook;
        private readonly JackpotDetailsService jackpotDetailsService;

        public JackpotDetailsExcelHandler(
            JackpotDetailsService jackpotDetailsService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            Workbook<TicketDto> workbook)
            : base(builder, excelBuilder, ticketHelper, monitorService, ipService)

        {
            this.jackpotDetailsService = jackpotDetailsService;
            this.workbook = workbook;
        }

        public async Task<ExcelFileViewModel> Handle(JackpotDetailsExcelQuery message)
        {
            var param = JackpotDetailsHandler.CreateJackpotDetailsParam(message);
            var tickets = await jackpotDetailsService.GetJackpotDetails(param);

            var viewModel = new JackpotDetailsViewModel(message, new BetListData(tickets, null), null, TicketHelper);

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