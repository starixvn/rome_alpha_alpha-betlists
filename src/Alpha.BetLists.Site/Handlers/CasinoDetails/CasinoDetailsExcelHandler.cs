namespace Alpha.BetLists.Site.Handlers.CasinoDetails
{
    using Alpha.IPInfo.Services;
    using AlphaPlus.SmartPunter.Services;
    using AlphaPlus.Utilities.IP;
    using AppResources;
    using BetListNPOI.SS.UserModel;
    using BetLists.Models;
    using Fanex.BetDetails.Casino.Models;
    using Fanex.BetDetails.Casino.Params;
    using Fanex.BetDetails.Casino.Services;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.BetListNPOI;
    using Requests.CasinoDetails;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ViewModels;
    using ViewModels.CasinoDetails;
    using static ONEbook.SpreadSheets.Column;

    public class CasinoDetailsExcelHandler : BetListExcelBaseHandler,
        IAsyncRequestHandler<CasinoDetailsExcelQuery, ExcelFileViewModel>,
        IAsyncRequestHandler<LiveCasinoDetailsExcelQuery, ExcelFileViewModel>,
        IAsyncRequestHandler<RNGCasinoDetailsExcelQuery, ExcelFileViewModel>
    {
        private int currentRowIndex = 1;
        private Workbook<TicketDto> workbook;
        private readonly ICasinoDetailsService casinoDetailsService;

        public CasinoDetailsExcelHandler(
            ICasinoDetailsService casinoDetailsService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            Workbook<TicketDto> workbook,
            IIPInfoService ipInfoService)
            : base(builder, excelBuilder, ticketHelper, monitorService, ipService, ipInfoService)
        {
            this.casinoDetailsService = casinoDetailsService;
            this.workbook = workbook;
        }
        public async Task<ExcelFileViewModel> Handle(CasinoDetailsExcelQuery message)
        {
            var param = CasinoDetailsHandler.CreateCasinoDetailsParam(message);
            var betlistData = casinoDetailsService.GetCasinoDetails(param);

            return await BuildExcelViewModel(betlistData, message, param);
        }

        public async Task<ExcelFileViewModel> Handle(LiveCasinoDetailsExcelQuery message)
        {
            var param = CasinoDetailsHandler.CreateCasinoDetailsParam(message);
            var betlistData = casinoDetailsService.GetLiveCasinoDetails(param);

            return await BuildExcelViewModel(betlistData, message, param);
        }

        public async Task<ExcelFileViewModel> Handle(RNGCasinoDetailsExcelQuery message)
        {
            var param = CasinoDetailsHandler.CreateCasinoDetailsParam(message);
            var betlistData = casinoDetailsService.GetRNGCasinoDetails(param);

            return await BuildExcelViewModel(betlistData, message, param);
        }

        private async Task<ExcelFileViewModel> BuildExcelViewModel(PagedData<ITicket> betlistData, CasinoDetailsExcelQuery message, CasinoDetailsParam param)
        {
            await AssignCountry(betlistData.Tickets, message.DisplayVNIP);

            var viewModel = new CasinoDetailsViewModel(message, param, betlistData, null, TicketHelper);

            workbook = workbook.UseTemplate(GetTemplateInfo(message.ViewerLevel));
            RtfHelper = message.RtfHelper ?? InitRTFHelper((workbook as NpoiWorkbook<TicketDto>).Workbook);

            var fileContent = workbook
                .Header(Resources.ResourceManager.GetString)
                .Title(viewModel.Title)
                .Body(betlistData.Tickets as IEnumerable<TicketDto>, GetBodyRowMapper(message.ViewerLevel, message.DisplayVNIP))
                .ExportToBytes();

            return new ExcelFileViewModel(viewModel.Title, fileContent);
        }

        internal Mapper<TicketDto> GetBodyRowMapper(UserLevel viewerLevel, bool displayVNIP)
        {
            var mapper = new Mapper<TicketDto>()
                .Map(A, _ => currentRowIndex++)
                .Map(B, ticket => BuildTrans(ticket))
                .Map(C, ticket => BuildChoice(ticket))
                .Map(D, ticket => BuildOdds(ticket))
                .Map(E, ticket => BuildStake(ticket))
                .Map(F, ticket => BuildWinLoss(ticket))
                .Map(G, ticket => BuildStatus(ticket, displayVNIP));

            mapper.Map(H, ticket => BuildPtCommmission(ticket, null, viewerLevel));

            if (viewerLevel.IsMasterLevel())
            {
                mapper.Map(I, ticket => BuildPtCommmission(ticket, null, UserLevel.Agent));
            }

            if (viewerLevel.IsSuperLevel())
            {
                mapper
                    .Map(I, ticket => BuildPtCommmission(ticket, null, UserLevel.Master))
                    .Map(J, ticket => BuildPtCommmission(ticket, null, UserLevel.Agent));
            }

            AddPlatform(mapper, viewerLevel);

            return mapper;
        }

        private IRichTextString BuildChoice(ITicket ticket)
            => ExcelBuilder
                .CreateChoiceBuilder(ticket.BetTypeId)
                .RenderRTF(ticket, TicketHelper, null, false, RtfHelper)
                .FirstOrDefault();

        private IRichTextString BuildOdds(ITicket ticket)
           => ExcelBuilder
               .CreateOddsBuilder(ticket.BetTypeId)
               .RenderRTF(ticket, null, GetOddsTypeName, RtfHelper);

        private IRichTextString BuildStatus(ITicket ticket, bool displayVNIP)
        {
            RtfHelper.RTFRenderer.AddText(ExcelBuilder
                .CreateStatusBuilder(ticket.BetTypeId)
                .RenderRTF(ticket, TicketHelper, null, displayVNIP, RtfHelper)
                .ToString());

            var ticketDto = (TicketDto)ticket;
            if (!string.IsNullOrWhiteSpace(ticketDto.Country))
            {
                RtfHelper.RTFRenderer.AddText("\n" + ticketDto.Country);
            }

            var rtfText = RtfHelper.RTFRenderer.Render();
            RtfHelper.RTFRenderer.Clear();

            return rtfText;
        }
    }
}
