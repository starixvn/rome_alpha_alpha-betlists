namespace Alpha.BetLists.Site.Handlers.CombinationMixParlay
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
    using Requests.CombinationMixParlay;
    using Services;
    using System.Threading.Tasks;
    using ViewModels;
    using ViewModels.CombinationMixParlay;

    public class CombinationMixParlayExcelHandler : BetListByBetTypeExcelBaseHandler,
        IAsyncRequestHandler<CombinationMixParlayExcelQuery, ExcelFileViewModel>
    {
        private Workbook<TicketDto> workbook;
        private readonly RunningBetListByBetTypeService runningBetListByBetTypeService;

        public CombinationMixParlayExcelHandler(
            RunningBetListByBetTypeService runningBetListByBetTypeService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            Workbook<TicketDto> workbook,
            IIPInfoService ipInfoService)
            : base(builder, excelBuilder, ticketHelper, monitorService, ipService, ipInfoService)
        {
            this.runningBetListByBetTypeService = runningBetListByBetTypeService;
            this.workbook = workbook;
        }

        public async Task<ExcelFileViewModel> Handle(CombinationMixParlayExcelQuery message)
        {
            var param = BetListByBetTypeHandler.CreateRunningBetListByBetTypeParam(message);
            var tickets = await runningBetListByBetTypeService.GetBetList(param);
            await AssignCountry(tickets, message.DisplayVNIP);

            var viewModel = new CombinationMixParlayViewModel(message, new BetListData(tickets, null), null, TicketHelper);

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
            var ticketData = viewModel.BetListData.TicketData;

            var mapper = GetTicketDtoRowMapperExcludeUsername(viewModel);
             mapper.Map(Column.H, ticket => BuildPtCommmission(ticket, ticketData, viewerLevel));
            if(viewerLevel.IsSuperLevel())
            {
                AddPtCommissionForSuper(mapper, viewerLevel, ticketData);
                mapper.Map(Column.K, ticket => ticket.MemberWinText);
                mapper.Map(Column.L, ticket => viewModel.GetSMARiskAmount(ticket));
                mapper.Map(Column.M, ticket => BuildPlatform(ticket.Platform));
            }   
            
            if(viewerLevel.IsMasterLevel())
            {
                mapper.Map(Column.I , ticket => BuildPtCommmission(ticket, ticketData, UserLevel.Agent));
                mapper.Map(Column.J, ticket => ticket.MemberWinText);
                mapper.Map(Column.K, ticket => viewModel.GetSMARiskAmount(ticket));
                mapper.Map(Column.L, ticket => BuildPlatform(ticket.Platform));
            }    
            
            if(viewerLevel.IsAgentLevel())
            {
                mapper.Map(Column.I, ticket => ticket.MemberWinText);
                mapper.Map(Column.J, ticket => viewModel.GetSMARiskAmount(ticket));
                mapper.Map(Column.K, ticket => BuildPlatform(ticket.Platform));
            }

            return mapper;
        }
    }
}
