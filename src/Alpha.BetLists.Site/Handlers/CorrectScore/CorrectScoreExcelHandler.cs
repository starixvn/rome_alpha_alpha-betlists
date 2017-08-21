namespace Alpha.BetLists.Site.Handlers.CorrectScore
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
    using Requests.CorrectScore;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels;
    using ViewModels.CorrectScore;

    public class CorrectScoreExcelHandler : BetListByBetTypeExcelBaseHandler, IAsyncRequestHandler<CorrectScoreExcelQuery, ExcelFileViewModel>
    {
        private Workbook<TicketDto> workbook;
        private readonly CorrectScoreService correctScoreService;

        public CorrectScoreExcelHandler(
            CorrectScoreService correctScoreService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            Workbook<TicketDto> workbook)
            : base(builder, excelBuilder, ticketHelper, monitorService, ipService)
        {
            this.correctScoreService = correctScoreService;
            this.workbook = workbook;
        }

        public async Task<ExcelFileViewModel> Handle(CorrectScoreExcelQuery message)
        {
            var correctScoreParam = CorrectScoreHandler.CreateCorrectScoreParam(message);
            var betListData = await correctScoreService.GetBetList(correctScoreParam);

            var viewModel = new CorrectScoreViewModel(message, betListData, Builder, TicketHelper);

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
