using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.BetLists.AppResources;
using Alpha.BetLists.Builder.ExcelBuilders;
using Alpha.BetLists.Models;
using Alpha.BetLists.Services;
using Alpha.BetLists.Site.Requests.WinLossByMatch;
using Alpha.BetLists.Site.ViewModels;
using Alpha.BetLists.Site.ViewModels.Blocks;
using Alpha.BetLists.Site.ViewModels.WinLoss;
using Alpha.SmartPunter.Services;
using Alpha.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;
using NPOI.SS.UserModel;
using ONEbook.SpreadSheets;
using static ONEbook.SpreadSheets.Column;

namespace Alpha.BetLists.Site.Handlers.WinLossByMatch
{
    public class WinLossByMatchExcelHandler : WinLossByMatchBaseHandler, IAsyncRequestHandler<WinLossByMatchExcelQuery, ExcelFileViewModel>
    {
        private readonly Workbook<ITicket> workbook;
        private int currentRowIndex;

        public WinLossByMatchExcelHandler(
            WinLossByMatchService winLossService,
            ISmartPunterBetListMonitor monitorService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            IPService ipService,
            Workbook<ITicket> workbook)
            : base(winLossService, monitorService, builder, excelBuilder, ticketHelper, ipService)
        {
            this.workbook = workbook;
        }

        public async Task<ExcelFileViewModel> Handle(WinLossByMatchExcelQuery message)
        {
            var winLossParam = CreateWinLossParam(message);
            var betlistData = await WinLossService.GetBetList(winLossParam);

            var winLossViewModel = new WinLossViewModel(message, betlistData, null, TicketHelper);

            currentRowIndex = 1;
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

        private TemplateInfo GetTemplateInfo(WinLossByMatchExcelQuery message)
            => new TemplateInfo(
                GetExcelTemplatePath($"WinLossByMatch\\{message.ViewerLevel.DisplayName}"),
                noInfomationText: Resources.NoInformationIsAvailable,
                bodyStartIndex: 3,
                bodyEndIndex: 4,
                footerEndIndex: 5);

        private Mapper<ITicket> GetBodyRowMapper(WinLossByMatchExcelQuery message, WinLossViewModel winlossViewModel)
        {
            var ticketData = winlossViewModel.BetListData.TicketData;
            var canViewScoreMap = winlossViewModel.CanViewScoreMap;

            var mapper = new Mapper<ITicket>()
                .Map(A, _ => currentRowIndex++)
                .Map(B, ticket => ticket.UserName)
                .Map(C, ticket => ExcelBuilder.CreateTransBuilder(ticket.BetTypeId).RenderRTF(ticket, TicketHelper))
                .Map(D, ticket => ExcelBuilder.CreateChoiceBuilder(ticket.BetTypeId).RenderRTF(ticket, TicketHelper, ticketData, canViewScoreMap, RtfHelper).FirstOrDefault())
                .Map(E, ticket => ExcelBuilder.CreateOddsBuilder(ticket.BetTypeId).RenderRTF(ticket, ticketData, GetOddsTypeName, RtfHelper))
                .Map(F, ticket => ExcelBuilder.CreateStakeBuilder(ticket.BetTypeId).RenderRTF(ticket, RtfHelper))
                .Map(G, ticket => BuildWinLoss(ticket))
                .Map(H, ticket => ExcelBuilder.CreateStatusBuilder(ticket.BetTypeId).RenderRTF(ticket, TicketHelper, ticketData, true, RtfHelper));

            if (message.ViewerLevel.IsAgentLevel())
            {
                mapper.Map(I, ticket => BuildPTCommmission(ticket, ticketData, UserLevel.Agent));
            }

            if (message.ViewerLevel.IsMasterLevel())
            {
                mapper
                    .Map(I, ticket => BuildPTCommmission(ticket, ticketData, UserLevel.Master))
                    .Map(J, ticket => BuildPTCommmission(ticket, ticketData, UserLevel.Agent));
            }

            if (message.ViewerLevel.IsSuperLevel())
            {
                mapper
                    .Map(I, ticket => BuildPTCommmission(ticket, ticketData, UserLevel.Super))
                    .Map(J, ticket => BuildPTCommmission(ticket, ticketData, UserLevel.Master))
                    .Map(K, ticket => BuildPTCommmission(ticket, ticketData, UserLevel.Agent));
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