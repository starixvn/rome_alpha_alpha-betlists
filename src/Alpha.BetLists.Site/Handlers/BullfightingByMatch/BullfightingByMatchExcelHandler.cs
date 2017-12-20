namespace Alpha.BetLists.Site.Handlers.BullfightingByMatch
{
    using System.Threading.Tasks;
    using AppResources;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using Requests.BullfightingByMatch;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels;
    using ViewModels.Blocks;
    using ViewModels.BullfightingByMatch;
    using static ONEbook.SpreadSheets.Column;

    public class BullfightingByMatchExcelHandler : BetListExcelBaseHandler, IAsyncRequestHandler<BullfightingByMatchExcelQuery, ExcelFileViewModel>
    {
        private int currentRowIndex;
        private Workbook<TicketDto> workbook;
        private readonly BullfightingByMatchService service;

        public BullfightingByMatchExcelHandler(
            BullfightingByMatchService service,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            Workbook<TicketDto> workbook)
            : base(builder, excelBuilder, ticketHelper, monitorService, ipService)
        {
            currentRowIndex = 1;
            this.workbook = workbook;
            this.service = service;
        }

        public async Task<ExcelFileViewModel> Handle(BullfightingByMatchExcelQuery message)
        {
            var param = BullfightingByMatchHandler.CreateParam(message);
            var tickets = await service.GetBetList(param);

            var viewModel = new BullfightingByMatchViewModel(message, new BetListData(tickets, null), null, TicketHelper);

            workbook = workbook.UseTemplate(GetTemplateInfo(message.ViewerLevel));
            RtfHelper = message.RtfHelper ?? InitRTFHelper((workbook as NpoiWorkbook<TicketDto>).Workbook);

            var fileContent = workbook
                .Header(Resources.ResourceManager.GetString)
                .Title(viewModel.Title)
                .Body(tickets, GetBodyRowMapper(message.ViewerLevel, viewModel))
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
