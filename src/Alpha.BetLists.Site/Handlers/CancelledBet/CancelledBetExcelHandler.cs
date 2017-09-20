namespace Alpha.BetLists.Site.Handlers.CancelledBet
{
    using System.Threading.Tasks;
    using Alpha.BetLists.AppResources;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Params;
    using Alpha.BetLists.Services;
    using Alpha.BetLists.Site.Requests.CancelledBet;
    using Alpha.BetLists.Site.ViewModels;
    using Alpha.BetLists.Site.ViewModels.CancelledBet;
    using Alpha.SmartPunter.Services;
    using Alpha.Utilities.IP;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using static ONEbook.SpreadSheets.Column;

    public class CancelledBetExcelHandler : BetListExcelBaseHandler, IAsyncRequestHandler<CancelledBetExcelQuery, ExcelFileViewModel>
    {
        private readonly CancelledBetService service;
        private Workbook<TicketDto> workbook;
        private int currentRowIndex;

        public CancelledBetExcelHandler(
            CancelledBetService service,
            ISmartPunterBetListMonitor monitorService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            IPService ipService,
            Workbook<TicketDto> workbook) : base(builder, excelBuilder, ticketHelper, monitorService, ipService)
        {
            this.service = service;
            this.workbook = workbook;
            currentRowIndex = 1;
        }

        public async Task<ExcelFileViewModel> Handle(CancelledBetExcelQuery message)
        {
            var parameter = new CancelledBetParam(message.ViewerLevel, message.ViewerId, message.MasterId, message.AgentId, message.MemberId, message.Language);
            var cancelledBet = await service.GetCancelledBet(parameter);

            var viewModel = new CancelledBetViewModel(message, cancelledBet, Builder, TicketHelper);

            workbook = workbook.UseTemplate(GetTemplateInfo(message.ViewerLevel));
            RtfHelper = message.RtfHelper ?? InitRTFHelper((workbook as NpoiWorkbook<TicketDto>).Workbook);
            var fileContent = workbook
                .Header(Resources.ResourceManager.GetString)
                .Title(Resources.VoidAndReject)
                .Body(cancelledBet.BetListData.Tickets, GetBodyRowMapper(message.ViewerLevel, viewModel))
                .ExportToBytes();

            return new ExcelFileViewModel(Resources.CancelledBet, fileContent);
        }

        protected internal override string ExcelTemplatePath(UserLevel viewerLevel)
           => GetExcelTemplatePath(viewerLevel.DisplayName);

        protected internal override Mapper<TicketDto> GetBodyRowMapper(UserLevel viewerLevel, BetListBaseViewModel viewModel)
        {
            var mapper = new Mapper<TicketDto>()
                .Map(A, _ => currentRowIndex++)
                .Map(B, ticket => ticket.UserName)
                .Map(C, ticket => BuildTrans(ticket))
                .Map(D, ticket => BuildChoice(ticket, viewModel))
                .Map(E, ticket => BuildOdds(ticket, viewModel))
                .Map(F, ticket => BuildStake(ticket))
                .Map(G, ticket => BuildStatus(ticket, viewModel));

            return mapper;
        }
    }
}