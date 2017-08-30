namespace Alpha.BetLists.Site.Handlers.BetListByBetType
{
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using ONEbook.SpreadSheets;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels;
    using static ONEbook.SpreadSheets.Column;

    public class BetListByBetTypeExcelBaseHandler : BetListExcelBaseHandler
    {
        private int currentRowIndex;

        public BetListByBetTypeExcelBaseHandler(
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService)
            : base(builder, excelBuilder, ticketHelper, monitorService, ipService)
        {
            currentRowIndex = 1;
        }

        protected internal override Mapper<TicketDto> GetBodyRowMapper(UserLevel viewerLevel, BetListBaseViewModel viewModel)
        {
            var ticketData = viewModel.BetListData.TicketData;

            var mapper = new Mapper<TicketDto>()
                .Map(A, _ => currentRowIndex++)
                .Map(B, ticket => ticket.UserName)
                .Map(C, ticket => BuildTrans(ticket))
                .Map(D, ticket => BuildChoice(ticket, viewModel))
                .Map(E, ticket => BuildOdds(ticket, viewModel))
                .Map(F, ticket => BuildStake(ticket))
                .Map(G, ticket => BuildStatus(ticket, viewModel));

            mapper.Map(H, ticket => BuildPtCommmission(ticket, ticketData, UserLevel.Agent));

            if (viewerLevel.IsMasterLevel())
            {
                mapper.Map(I, ticket => BuildPtCommmission(ticket, ticketData, UserLevel.Master));
            }

            if (viewerLevel.IsSuperLevel())
            {
                mapper
                    .Map(I, ticket => BuildPtCommmission(ticket, ticketData, UserLevel.Master))
                    .Map(J, ticket => BuildPtCommmission(ticket, ticketData, UserLevel.Super));
            }

            return mapper;
        }
    }
}
