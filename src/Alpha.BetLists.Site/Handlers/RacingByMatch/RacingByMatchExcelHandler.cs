namespace Alpha.BetLists.Site.Handlers.RacingByMatch
{
    using System.Threading.Tasks;
    using AppResources;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using Requests.RacingByMatch;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels;
    using ViewModels.Blocks;
    using ViewModels.RacingByMatch;
    using static ONEbook.SpreadSheets.Column;

    public class RacingByMatchExcelHandler : BetListExcelBaseHandler, IAsyncRequestHandler<RacingByMatchExcelQuery, ExcelFileViewModel>
    {
        private Workbook<TicketDto> workbook;
        private readonly RacingByMatchService racingByMatchService;

        public RacingByMatchExcelHandler(
            RacingByMatchService racingByMatchService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            Workbook<TicketDto> workbook)
            : base(builder, excelBuilder, ticketHelper, monitorService, ipService)
        {
            this.workbook = workbook;
            this.racingByMatchService = racingByMatchService;
        }

        public async Task<ExcelFileViewModel> Handle(RacingByMatchExcelQuery message)
        {
            var param = RacingByMatchHandler.CreateRacingByMatchParam(message);
            var tickets = await racingByMatchService.GetBetList(param);

            var viewModel = new RacingByMatchViewModel(message, new BetListData(tickets, null), null, TicketHelper);

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

            var mapper = GetTicketDtoRowMapperIncludeUsername(viewModel, showWinLoss: true);

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
