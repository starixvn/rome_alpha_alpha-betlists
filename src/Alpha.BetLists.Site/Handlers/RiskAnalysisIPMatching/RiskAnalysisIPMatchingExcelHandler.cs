namespace Alpha.BetLists.Site.Handlers.RiskAnalysisIPMatching
{
    using Alpha.BetLists.Builders.ExcelBuilders;
    using Alpha.BetLists.Params;
    using Alpha.BetLists.Site.Requests.RiskAnalysisIPMatching;
    using Alpha.BetLists.Site.ViewModels.Blocks;
    using Alpha.BetLists.Site.ViewModels.RiskAnalysisIPMatching;
    using AppResources;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using Services;
    using SmartPunter.Services;
    using System.Threading.Tasks;
    using Utilities.IP;
    using ViewModels;
    using static ONEbook.SpreadSheets.Column;

    public class RiskAnalysisIPMatchingExcelHandler : BetListExcelBaseHandler, IAsyncRequestHandler<RiskAnalysisIPMatchingExcelQuery, ExcelFileViewModel>
    {
        private readonly RiskAnalysisIPMatchingService riskAnalysisIPMatchingService;
        private readonly ReportDateService reportDateService;
        private int currentRowIndex = 1;

        private Workbook<TicketDto> workbook;

        public RiskAnalysisIPMatchingExcelHandler(
            RiskAnalysisIPMatchingService riskAnalysisIPMatchingService,
            ReportDateService reportDateService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            Workbook<TicketDto> workbook)
            : base(builder, excelBuilder, ticketHelper, monitorService, ipService)
        {
            this.workbook = workbook;
            this.riskAnalysisIPMatchingService = riskAnalysisIPMatchingService;
            this.reportDateService = reportDateService;
        }

        public async Task<ExcelFileViewModel> Handle(RiskAnalysisIPMatchingExcelQuery message)
        {
            var param = new RiskAnalysisIPMatchingParam(
               message.CustId,
               message.Date,
               reportDateService.GetMoveBetsDay(),
               message.IP);

            var betlistData = await riskAnalysisIPMatchingService.GetBetList(param);

            var viewModel = new RiskAnalysisIPMatchingViewModel(message, betlistData, Builder, TicketHelper);

            workbook = workbook.UseTemplate(GetTemplateInfo(message.ViewerLevel));
            RtfHelper = message.RtfHelper ?? InitRTFHelper((workbook as NpoiWorkbook<TicketDto>).Workbook);

            var fileContent = workbook
                    .Header(Resources.ResourceManager.GetString)
                    .Title(viewModel.Title)
                    .Body(betlistData.Tickets, GetBodyRowMapper(message.ViewerLevel, viewModel))
                    .Footer(new TicketDto(), GetFooterRowMapper(viewModel.Footer))
                    .ExportToBytes();

            return new ExcelFileViewModel(viewModel.Title, fileContent);
        }

        protected internal override Mapper<TicketDto> GetBodyRowMapper(UserLevel viewerLevel, BetListBaseViewModel viewModel)
        {
            var ticketData = viewModel.BetListData.TicketData;
            var mapper = new Mapper<TicketDto>()
                .Map(A, _ => currentRowIndex++)
                .Map(B, ticket => ticket.UserName)
                .Map(C, ticket => BuildTrans(ticket))
                .Map(D, ticket => BuildChoice(ticket, viewModel))
                .Map(E, ticket => BuildOdds(ticket, viewModel))
                .Map(F, ticket => BuildStake(ticket))
                .Map(G, ticket => BuildWinLoss(ticket))
                .Map(H, ticket => BuildStatus(ticket, viewModel))
                .Map(I, ticket => BuildPtCommmission(ticket, ticketData, viewerLevel))
                .Map(J, ticket => BuildPtCommmission(ticket, ticketData, UserLevel.Master))
                .Map(K, ticket => BuildPtCommmission(ticket, ticketData, UserLevel.Agent));

            return mapper;
        }

        protected internal override Mapper<TicketDto> GetFooterRowMapper(Footer footer)
        {
            var mapper = new Mapper<TicketDto>()
                .Map(A, _ => BetListExcelBuilderExt.BuildFooterHeader(RtfHelper))
                .Map(G, _ => BuildFooter(footer));

            return mapper;
        }
    }
}