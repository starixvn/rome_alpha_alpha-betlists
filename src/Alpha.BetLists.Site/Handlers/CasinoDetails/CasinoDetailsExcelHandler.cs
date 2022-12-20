namespace Alpha.BetLists.Site.Handlers.CasinoDetails
{
    using System.Collections.Generic;
    using System.Linq;
    using AppResources;
    using BetLists.Models;
    using Fanex.BetDetails.Casino.Services;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using BetListNPOI.SS.UserModel;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.BetListNPOI;
    using Requests.CasinoDetails;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels;
    using ViewModels.CasinoDetails;
    using static ONEbook.SpreadSheets.Column;

    public class CasinoDetailsExcelHandler : BetListExcelBaseHandler, IRequestHandler<CasinoDetailsExcelQuery, ExcelFileViewModel>
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
            Workbook<TicketDto> workbook)
            : base(builder, excelBuilder, ticketHelper, monitorService, ipService)
        {
            this.casinoDetailsService = casinoDetailsService;
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(CasinoDetailsExcelQuery message)
        {
            var param = CasinoDetailsHandler.CreateCasinoDetailsParam(message);

            var detailData = casinoDetailsService.GetCasinoDetails(param);

            var viewModel = new CasinoDetailsViewModel(message, param, detailData, null, TicketHelper);

            workbook = workbook.UseTemplate(GetTemplateInfo(message.ViewerLevel));
            RtfHelper = message.RtfHelper ?? InitRTFHelper((workbook as NpoiWorkbook<TicketDto>).Workbook);

            var fileContent = workbook
                .Header(Resources.ResourceManager.GetString)
                .Title(viewModel.Title)
                .Body(detailData.Tickets as IEnumerable<TicketDto>, GetBodyRowMapper(message.ViewerLevel, message.DisplayVNIP))
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
            => ExcelBuilder
                .CreateStatusBuilder(ticket.BetTypeId)
                .RenderRTF(ticket, TicketHelper, null, displayVNIP, RtfHelper);
    }
}
