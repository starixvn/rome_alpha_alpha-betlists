namespace Alpha.BetLists.Site.Handlers.BetListByBetType
{
    using Alpha.IPInfo.Services;
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
            IPService ipService,
            IIPInfoService ipInfoService)
            : base(builder, excelBuilder, ticketHelper, monitorService, ipService, ipInfoService)
        {
        }

        protected internal override Mapper<TicketDto> GetBodyRowMapper(UserLevel viewerLevel, BetListBaseViewModel viewModel)
        {
            var ticketData = viewModel.BetListData.TicketData;

            var mapper = GetTicketDtoRowMapperIncludeUsername(viewModel, showWinLoss: false);

            mapper.Map(H, ticket => BuildPtCommmission(ticket, ticketData, viewerLevel));

            if (viewerLevel.IsMasterLevel())
            {
                mapper.Map(I, ticket => BuildPtCommmission(ticket, ticketData, UserLevel.Agent));
            }

            AddPtCommissionForSuper(mapper, viewerLevel, ticketData);

            AddPlatform(mapper, viewerLevel);

            return mapper;
        }
    }
}
