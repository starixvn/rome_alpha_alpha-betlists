namespace Alpha.BetLists.Site.Handlers.BetListByBetType
{
    using Alpha.IPInfo.Services;
    using AlphaPlus.SmartPunter.Services;
    using AlphaPlus.Utilities.IP;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using ONEbook.SpreadSheets;
    using ViewModels;
    using static ONEbook.SpreadSheets.Column;

    public class BetListByBetTypeExcelBaseHandler : BetListExcelBaseHandler
    {
        public BetListByBetTypeExcelBaseHandler(
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            IIPInfoService ipInfoService)
            : base(builder, excelBuilder, ticketHelper, monitorService, ipService, ipInfoService)
        {
        }

        protected internal override Mapper<TicketDto> GetBodyRowMapper(UserLevel viewerLevel, BetListBaseViewModel viewModel)
        {
            var ticketData = viewModel.BetListData.TicketData;

            var mapper = GetTicketDtoRowMapperExcludeUsername(viewModel);
            mapper.Map(Column.H, ticket => BuildPtCommmission(ticket, ticketData, viewerLevel));
            if (viewerLevel.IsSuperLevel())
            {
                AddPtCommissionForSuper(mapper, viewerLevel, ticketData);
                mapper.Map(Column.K, ticket => ticket.MemberWinText);
                mapper.Map(Column.L, ticket => viewModel.GetSMARiskAmount(ticket));
                mapper.Map(Column.M, ticket => BuildPlatform(ticket.Platform));
            }

            if (viewerLevel.IsMasterLevel())
            {
                mapper.Map(Column.I, ticket => BuildPtCommmission(ticket, ticketData, UserLevel.Agent));
                mapper.Map(Column.J, ticket => ticket.MemberWinText);
                mapper.Map(Column.K, ticket => viewModel.GetSMARiskAmount(ticket));
                mapper.Map(Column.L, ticket => BuildPlatform(ticket.Platform));
            }

            if (viewerLevel.IsAgentLevel())
            {
                mapper.Map(Column.I, ticket => ticket.MemberWinText);
                mapper.Map(Column.J, ticket => viewModel.GetSMARiskAmount(ticket));
                mapper.Map(Column.K, ticket => BuildPlatform(ticket.Platform));
            }

            return mapper;
        }
    }
}
