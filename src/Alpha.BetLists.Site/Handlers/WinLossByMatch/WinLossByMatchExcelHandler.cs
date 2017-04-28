namespace Alpha.BetLists.Site.Handlers.WinLossByMatch
{
    using System.Threading.Tasks;
    using AppResources;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Requests.WinLossByMatch;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels;
    using ViewModels.Blocks;
    using ViewModels.WinLossByMatch;
    using static ONEbook.SpreadSheets.Column;

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

            var viewModel = new WinLossByMatchViewModel(message, betlistData, null, TicketHelper);

            currentRowIndex = 1;

            var fileContent = workbook
                .UseTemplate(GetTemplateInfo(message.CustLevel))
                .Header(Resources.ResourceManager.GetString)
                .Title(viewModel.Title)
                .Body(betlistData.Tickets, GetBodyRowMapper(message.ViewerLevel, viewModel))
                .Footer(new Ticket(), GetFooterRowMapper(viewModel.Footer))
                .Sheet(Resources.BetList)
                .ExportToBytes();

            return new ExcelFileViewModel(viewModel.Title, fileContent);
        }

        public void SetCurrentRowIndex(int rowIndex)
        {
            currentRowIndex = rowIndex;
        }

        protected internal override string ExcelTemplatePath(UserLevel viewerLevel)
            => GetExcelTemplatePath($"WinLossByMatch\\{viewerLevel.DisplayName}");

        protected internal override Mapper<ITicket> GetBodyRowMapper(UserLevel viewerLevel, BetListBaseViewModel viewModel)
        {
            var ticketData = viewModel.BetListData.TicketData;
            var mapper = new Mapper<ITicket>()
                .Map(A, _ => currentRowIndex++)
                .Map(B, ticket => ticket.UserName)
                .Map(C, ticket => BuildTrans(ticket))
                .Map(D, ticket => BuildChoice(ticket, viewModel))
                .Map(E, ticket => BuildOdds(ticket, viewModel))
                .Map(F, ticket => BuildStake(ticket))
                .Map(G, ticket => BuildWinLoss(ticket))
                .Map(H, ticket => BuildStatus(ticket, viewModel));

            mapper.Map(I, ticket => BuildPTCommmission(ticket, ticketData, viewerLevel));

            if (viewerLevel.IsMasterLevel())
            {
                mapper.Map(J, ticket => BuildPTCommmission(ticket, ticketData, UserLevel.Agent));
            }

            if (viewerLevel.IsSuperLevel())
            {
                mapper
                    .Map(J, ticket => BuildPTCommmission(ticket, ticketData, UserLevel.Master))
                    .Map(K, ticket => BuildPTCommmission(ticket, ticketData, UserLevel.Agent));
            }

            return mapper;
        }

        protected internal override Mapper<ITicket> GetFooterRowMapper(Footer footer)
        {
            var mapper = base.GetFooterRowMapper(footer);

            mapper.Map(G, _ => BuildFooter(footer));

            return mapper;
        }
    }
}