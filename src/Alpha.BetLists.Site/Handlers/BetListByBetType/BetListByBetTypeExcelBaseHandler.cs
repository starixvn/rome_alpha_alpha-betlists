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
        public BetListByBetTypeExcelBaseHandler(
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService)
            : base(builder, excelBuilder, ticketHelper, monitorService, ipService)
        {
        }

        protected internal override Mapper<TicketDto> GetBodyRowMapper(UserLevel viewerLevel, BetListBaseViewModel viewModel)
        {
            var ticketData = viewModel.BetListData.TicketData;

            var mapper = GetTicketDtoRowMapperIncludeUsername(viewModel);

            mapper.Map(H, ticket => BuildPtCommmission(ticket, ticketData, viewerLevel));

            if (viewerLevel.IsMasterLevel())
            {
                mapper.Map(I, ticket => BuildPtCommmission(ticket, ticketData, UserLevel.Agent));
            }

            if (viewerLevel.IsSuperLevel())
            {
                mapper
                    .Map(I, ticket => BuildPtCommmission(ticket, ticketData, UserLevel.Master))
                    .Map(J, ticket => BuildPtCommmission(ticket, ticketData, UserLevel.Agent));
            }

            return mapper;
        }
    }
}
