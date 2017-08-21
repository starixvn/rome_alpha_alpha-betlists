namespace Alpha.BetLists.Site.Handlers.WinLossByMatch
{
    using System.Threading.Tasks;
    using AppResources;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using Requests.WinLossByMatch;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels;
    using ViewModels.Blocks;
    using ViewModels.WinLossByMatch;
    using static ONEbook.SpreadSheets.Column;

    public class WinLossByMatchExcelHandler : BetListExcelBaseHandler, IAsyncRequestHandler<WinLossByMatchExcelQuery, ExcelFileViewModel>
    {
        private readonly WinLossByMatchService winLossByMatchService;
        private Workbook<TicketDto> workbook;
        private int currentRowIndex;

        public WinLossByMatchExcelHandler(
            WinLossByMatchService winLossByMatchService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            Workbook<TicketDto> workbook)
            : base(builder, excelBuilder, ticketHelper, monitorService, ipService)
        {
            this.winLossByMatchService = winLossByMatchService;
            this.workbook = workbook;
        }

        public async Task<ExcelFileViewModel> Handle(WinLossByMatchExcelQuery message)
        {
            var winLossParam = WinLossByMatchHandler.CreateWinLossParam(message);
            var betlistData = await winLossByMatchService.GetBetList(winLossParam);

            var viewModel = new WinLossByMatchViewModel(message, betlistData, null, TicketHelper);

            currentRowIndex = 1;

            workbook = workbook.UseTemplate(GetTemplateInfo(message.CustLevel));
            RtfHelper = message.RtfHelper ?? InitRTFHelper((workbook as NpoiWorkbook<TicketDto>).Workbook);

            var fileContent = workbook
                .Header(Resources.ResourceManager.GetString)
                .Title(viewModel.Title)
                .Body(betlistData.Tickets, GetBodyRowMapper(message.ViewerLevel, viewModel))
                .Footer(new TicketDto(), GetFooterRowMapper(viewModel.Footer))
                .ExportToBytes();

            return new ExcelFileViewModel(viewModel.Title, fileContent);
        }

        public void SetCurrentRowIndex(int rowIndex)
        {
            currentRowIndex = rowIndex;
        }

        protected internal override string ExcelTemplatePath(UserLevel viewerLevel)
            => GetExcelTemplatePath(viewerLevel.DisplayName);

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
                .Map(H, ticket => BuildStatus(ticket, viewModel));

            mapper.Map(I, ticket => BuildPtCommmission(ticket, ticketData, viewerLevel));

            if (viewerLevel.IsMasterLevel())
            {
                mapper.Map(J, ticket => BuildPtCommmission(ticket, ticketData, UserLevel.Agent));
            }

            if (viewerLevel.IsSuperLevel())
            {
                mapper
                    .Map(J, ticket => BuildPtCommmission(ticket, ticketData, UserLevel.Master))
                    .Map(K, ticket => BuildPtCommmission(ticket, ticketData, UserLevel.Agent));
            }

            return mapper;
        }

        protected internal override Mapper<TicketDto> GetFooterRowMapper(Footer footer)
        {
            var mapper = base.GetFooterRowMapper(footer);

            mapper.Map(G, _ => BuildFooter(footer));

            return mapper;
        }
    }
}
