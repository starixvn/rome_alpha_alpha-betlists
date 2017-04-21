namespace Alpha.BetLists.Site.Handlers.WinLoss
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AppResources;
    using BetLists.Models;
    using Builder.ExcelBuilders;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using NPOI.SS.UserModel;
    using ONEbook.SpreadSheets;
    using Requests.WinLoss;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels;
    using ViewModels.Blocks;
    using ViewModels.WinLoss;
    using static ONEbook.SpreadSheets.Column;

    public class WinLossExcelHandler : WinLossBaseHandler, IAsyncRequestHandler<WinLossExcelQuery, ExcelFileViewModel>
    {
        private readonly Workbook<ITicket> workbook;

        public WinLossExcelHandler(
            WinLossService winLossService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            Workbook<ITicket> workbook)
            : base(winLossService, builder, excelBuilder, ticketHelper, monitorService, ipService)
        {
            this.workbook = workbook;
        }

        public async Task<ExcelFileViewModel> Handle(WinLossExcelQuery message)
        {
            var winLossParam = CreateWinLossParam(message);
            var betlistData = await WinLossService.GetBetList(winLossParam);

            var winLossViewModel = new WinLossViewModel(message, betlistData, null, TicketHelper);

            var fileContent = workbook
                    .UseTemplate(GetTemplateInfo(message))
                    .Header(Resources.ResourceManager.GetString)
                    .Title(winLossViewModel.Title)
                    .Body(betlistData.Tickets, GetBodyRowMapper(message, winLossViewModel))
                    .Footer(new Ticket(), GetFooterRowMapper(winLossViewModel))
                    .Sheet(Resources.BetList)
                    .ExportToBytes();

            return new ExcelFileViewModel(winLossViewModel.Title, fileContent);
        }

        private TemplateInfo GetTemplateInfo(WinLossExcelQuery message)
            => new TemplateInfo(
                   GetExcelTemplatePath(message.ViewerLevel.DisplayName),
                   noInfomationText: Resources.NoInformationIsAvailable,
                   bodyStartIndex: 3,
                   bodyEndIndex: 4,
                   footerEndIndex: 5);

        private Mapper<ITicket> GetBodyRowMapper(WinLossExcelQuery message, WinLossViewModel winlossViewModel)
        {
            var ticketData = winlossViewModel.BetListData.TicketData;
            var canViewScoreMap = winlossViewModel.CanViewScoreMap;

            var mapper = new Mapper<ITicket>()
                .Map(B, ticket => ExcelBuilder.CreateTransBuilder(ticket.BetTypeId).RenderRTF(ticket, TicketHelper))
                .Map(C, ticket => ExcelBuilder.CreateChoiceBuilder(ticket.BetTypeId).RenderRTF(ticket, TicketHelper, ticketData, canViewScoreMap, RtfHelper).FirstOrDefault())
                .Map(D, ticket => ExcelBuilder.CreateOddsBuilder(ticket.BetTypeId).RenderRTF(ticket, ticketData, GetOddsTypeName, RtfHelper))
                .Map(E, ticket => ExcelBuilder.CreateStakeBuilder(ticket.BetTypeId).RenderRTF(ticket, RtfHelper))
                .Map(F, ticket => BuildWinLoss(ticket))
                .Map(G, ticket => ExcelBuilder.CreateStatusBuilder(ticket.BetTypeId).RenderRTF(ticket, TicketHelper, ticketData, true, RtfHelper));

            if (message.ViewerLevel.IsAgentLevel())
            {
                mapper.Map(H, ticket => BuildPTCommmission(ticket, ticketData, UserLevel.Agent));
            }

            if (message.ViewerLevel.IsMasterLevel())
            {
                mapper
                    .Map(H, ticket => BuildPTCommmission(ticket, ticketData, UserLevel.Master))
                    .Map(I, ticket => BuildPTCommmission(ticket, ticketData, UserLevel.Agent));
            }

            if (message.ViewerLevel.IsSuperLevel())
            {
                mapper
                    .Map(H, ticket => BuildPTCommmission(ticket, ticketData, UserLevel.Super))
                    .Map(I, ticket => BuildPTCommmission(ticket, ticketData, UserLevel.Master))
                    .Map(J, ticket => BuildPTCommmission(ticket, ticketData, UserLevel.Agent));
            }

            return mapper;
        }

        private Mapper<ITicket> GetFooterRowMapper(WinLossViewModel winlossViewModel)
        {
            var mapper = new Mapper<ITicket>()
                .Map(A, _ => BetListExcelBuilderExt.BuildFooterHeader(RtfHelper))
                .Map(F, _ => BuildFooter(winlossViewModel));

            return mapper;
        }

        private IRichTextString BuildWinLoss(ITicket ticket)
        {
            var winLossBlockViewModel = new WinLostBlockViewModel(ticket);

            return BetListExcelBuilderExt.BuildWinLoss(
                winLossBlockViewModel.WinLost,
                winLossBlockViewModel.CommVisibility ? winLossBlockViewModel.Comm : string.Empty,
                RtfHelper);
        }

        private IRichTextString BuildPTCommmission(ITicket ticket, List<ITicketData> ticketData, UserLevel userLevel)
            => BetListExcelBuilderExt.BuildPTCommmission(ticket, ticketData, userLevel, RtfHelper);

        private IRichTextString BuildFooter(WinLossViewModel winlossViewModel)
            => BetListExcelBuilderExt.BuildFooter(
                winlossViewModel.FooterSubTotalWinLoss,
                winlossViewModel.FooterSubTotalCommission,
                winlossViewModel.FooterTotal,
                RtfHelper);
    }
}