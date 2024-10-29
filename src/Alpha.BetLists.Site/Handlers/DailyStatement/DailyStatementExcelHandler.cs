namespace Alpha.BetLists.Site.Handlers.DailyStatement
{
    using Alpha.IPInfo.Services;
    using AlphaPlus.SmartPunter.Services;
    using AlphaPlus.Utilities.IP;
    using AppResources;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.BetListNPOI;
    using Requests.DailyStatement;
    using Services;
    using System.Threading.Tasks;
    using ViewModels;
    using ViewModels.DailyStatement;

    public class DailyStatementExcelHandler : BetListExcelBaseHandler, IAsyncRequestHandler<DailyStatementExcelQuery, ExcelFileViewModel>
    {
        private readonly DailyStatementService dailyStatementService;
        private Workbook<TicketDto> workbook;

        public DailyStatementExcelHandler(
            DailyStatementService dailyStatementService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            Workbook<TicketDto> workbook,
            IIPInfoService ipInfoService)
            : base(builder, excelBuilder, ticketHelper, monitorService, ipService, ipInfoService)
        {
            this.workbook = workbook;
            this.dailyStatementService = dailyStatementService;
        }

        public async Task<ExcelFileViewModel> Handle(DailyStatementExcelQuery message)
        {
            var param = DailyStatementHandler.CreateDailyStatementParam(message);
            var betlistData = await dailyStatementService.GetBetList(param);
            await AssignCountry(betlistData.Tickets, message.DisplayVNIP);

            var dailyStatementViewModel = new DailyStatementViewModel(message, betlistData, Builder, TicketHelper, null, message.SupportRiskAmount);

            workbook = workbook.UseTemplate(message.SupportRiskAmount ? GetRiskAmountTemplateInfo(message.ViewerLevel) : GetTemplateInfo(message.ViewerLevel));

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
            var mapper = GetTicketDtoRowMapperExcludeUsername(viewModel);
            if (viewModel.SupportRiskAmount)
            {
                mapper.Map(Column.H, ticket => ticket.MemberWinText);
                mapper.Map(Column.I, ticket => viewModel.GetSMARiskAmount(ticket));
                mapper.Map(Column.J, ticket => BuildPlatform(ticket.Platform));
            }
            else
            {
                mapper.Map(Column.H, ticket => BuildPlatform(ticket.Platform));
            }

            return mapper;
        }

        private TemplateInfo GetRiskAmountTemplateInfo(UserLevel viewerLevel)
            => new TemplateInfo(
                   GetExcelTemplatePath($"{viewerLevel.DisplayName}-RiskAmount"),
                   noInfomationText: Resources.NoInformationIsAvailable,
                   bodyStartIndex: 3,
                   bodyEndIndex: 4,
                   footerEndIndex: 5);
    }
}
