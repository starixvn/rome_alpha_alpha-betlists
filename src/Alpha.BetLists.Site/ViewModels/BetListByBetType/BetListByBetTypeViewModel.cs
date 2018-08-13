namespace Alpha.BetLists.Site.ViewModels.BetListByBetType
{
    using System.Collections.Generic;
    using System.Linq;
    using BetLists.Helpers;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests.BetListByBetType;

    public class BetListByBetTypeViewModel : BetListBaseViewModel
    {
        public BetListByBetTypeViewModel(
            BetListByBetTypeQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            IEnumerable<int> supportScoreMapBetTypeIds = null)
            : base(query, betListData, builder, ticketHelper, true)
        {
            SupportScoreMapBetTypeIds = supportScoreMapBetTypeIds;
            Title = CacheRefServiceHelper.GetBetTypeName(query.BetType).ToUpperInvariant();
            BetType = query.BetType;
        }

        public int BetType { get; }

        public IEnumerable<int> SupportScoreMapBetTypeIds { get; }

        public bool AllowToViewSuperPT => baseQuery.ViewerLevel.IsSuperLevel();

        public bool AllowToViewMasterPT => baseQuery.ViewerLevel.IsMasterOrSuperLevel();

        public override bool CanViewScoreMap(ITicket ticket)
            => SupportScoreMapBetTypeIds != null
            && SupportScoreMapBetTypeIds.Contains(ticket.BetTypeId);
    }
}