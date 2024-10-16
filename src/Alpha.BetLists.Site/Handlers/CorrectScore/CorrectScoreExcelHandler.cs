namespace Alpha.BetLists.Site.Handlers.CorrectScore
{
    using Alpha.IPInfo.Services;
    using AlphaPlus.SmartPunter.Services;
    using AlphaPlus.Utilities.IP;
    using AppResources;
    using BetListByBetType;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.BetListNPOI;
    using Requests.CorrectScore;
    using Services;
    using System.Threading.Tasks;
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
            Workbook<TicketDto> workbook,
            IIPInfoService ipInfoService)
            : base(builder, excelBuilder, ticketHelper, monitorService, ipService, ipInfoService)
        {
            this.correctScoreService = correctScoreService;
            this.workbook = workbook;
        }

        public async Task<ExcelFileViewModel> Handle(CorrectScoreExcelQuery message)
        {
            var correctScoreParam = CorrectScoreHandler.CreateCorrectScoreParam(message);
            var tickets = await correctScoreService.GetBetList(correctScoreParam);
            await AssignCountry(tickets, message.DisplayVNIP);

            var viewModel = new CorrectScoreViewModel(message, new BetListData(tickets, null), null, TicketHelper);

            workbook = workbook.UseTemplate(GetTemplateInfo(message.ViewerLevel));
            RtfHelper = message.RtfHelper ?? InitRTFHelper((workbook as NpoiWorkbook<TicketDto>).Workbook);

            var fileContent = workbook
                .Header(Resources.ResourceManager.GetString)
                .Title(viewModel.Title)
                .Body(tickets, GetBodyRowMapper(message.ViewerLevel, viewModel))
                .ExportToBytes();

            return new ExcelFileViewModel(viewModel.Title, fileContent);
        }

        protected internal override Mapper<TicketDto> GetBodyRowMapper(UserLevel viewerLevel, BetListBaseViewModel viewModel)
        {
            var mapper = GetTicketDtoRowMapperExcludeUsername(viewModel);
            mapper.Map(Column.I, ticket => ticket.MemberWinText);
            mapper.Map(Column.J, ticket => viewModel.GetSMARiskAmount(ticket));
            mapper.Map(Column.K, ticket => BuildPlatform(ticket.Platform));

            return mapper;
        }
    }
}
