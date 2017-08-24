namespace Alpha.BetLists.Site.Handlers.DailyStatement
{
    using AppResources;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using Requests.DailyStatement;
    using Services;
    using SmartPunter.Services;
    using System.Threading.Tasks;
    using Utilities.IP;
    using ViewModels;
    using ViewModels.DailyStatement;
    using static ONEbook.SpreadSheets.Column;

    public class DailyStatementExcelHandler : BetListExcelBaseHandler, IAsyncRequestHandler<DailyStatementExcelQuery, ExcelFileViewModel>
    {
        private readonly DailyStatementService dailyStatementService;
        private Workbook<TicketDto> workbook;
        private int currentRowIndex = 1;

        public DailyStatementExcelHandler(
            DailyStatementService dailyStatementService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            Workbook<TicketDto> workbook)
            : base(builder, excelBuilder, ticketHelper, monitorService, ipService)
        {
            this.workbook = workbook;
            this.dailyStatementService = dailyStatementService;
        }

        public async Task<ExcelFileViewModel> Handle(DailyStatementExcelQuery message)
        {
            var param = DailyStatementHandler.CreateDailyStatementParam(message);
            var betlistData = await dailyStatementService.GetBetList(param);

            var dailyStatementViewModel = new DailyStatementViewModel(message, betlistData, Builder, TicketHelper);

            workbook = workbook.UseTemplate(GetTemplateInfo(UserLevel.Member));
            RtfHelper = message.RtfHelper ?? InitRTFHelper((workbook as NpoiWorkbook<TicketDto>).Workbook);

            var fileContent = workbook
                .Header(Resources.ResourceManager.GetString)
                .Title(dailyStatementViewModel.Title)
                .Body(betlistData.Tickets, GetBodyRowMapper(message.ViewerLevel, dailyStatementViewModel))
                .Footer(new TicketDto(), GetFooterRowMapper(dailyStatementViewModel.Footer))
                .ExportToBytes();

            return new ExcelFileViewModel("BetList", fileContent);
        }

        protected internal override Mapper<TicketDto> GetBodyRowMapper(UserLevel viewerLevel, BetListBaseViewModel viewModel)
        {
            var mapper = new Mapper<TicketDto>()
                .Map(A, _ => currentRowIndex++)
                .Map(B, ticket => BuildTrans(ticket))
                .Map(C, ticket => BuildChoice(ticket, viewModel))
                .Map(D, ticket => BuildOdds(ticket, viewModel))
                .Map(E, ticket => BuildStake(ticket))
                .Map(F, ticket => BuildWinLoss(ticket))
                .Map(G, ticket => BuildStatus(ticket, viewModel));

            return mapper;
        }
    }
}
