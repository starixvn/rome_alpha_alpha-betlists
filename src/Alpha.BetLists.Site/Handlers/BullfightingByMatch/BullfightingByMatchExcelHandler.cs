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
    using ViewModels.BullfightingByMatch;
    using static ONEbook.SpreadSheets.Column;

    public class BullfightingByMatchExcelHandler : BetListExcelBaseHandler, IAsyncRequestHandler<BullfightingByMatchExcelQuery, ExcelFileViewModel>
    {
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
                .ExportToBytes();

            return new ExcelFileViewModel(viewModel.Title, fileContent);
        }

        protected internal override Mapper<TicketDto> GetBodyRowMapper(UserLevel viewerLevel, BetListBaseViewModel viewModel)
        {
            var ticketData = viewModel.BetListData.TicketData;

            var mapper = GetTicketDtoRowMapperIncludeUsername(viewModel, false);

            mapper.Map(H, ticket => BuildPtCommmission(ticket, ticketData, viewerLevel));

            if (viewerLevel.IsMasterLevel())
            {
                mapper.Map(I, ticket => BuildPtCommmission(ticket, ticketData, UserLevel.Agent));
            }

            AddPtCommissionForSuper(mapper, viewerLevel, ticketData);

            return mapper;
        }
    }
}
