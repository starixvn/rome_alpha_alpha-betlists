namespace Alpha.BetLists.Site.ViewModels.BetListByBetType
{
    using System.Collections.Generic;
    using System.Linq;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using RemotingRef;
    using Requests.BetListByBetType;

    public class BetListByBetTypeViewModel : BetListBaseViewModel
    {
        public BetListByBetTypeViewModel(
            BetListByBetTypeQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            IEnumerable<int> supportScoreMapBettypeIds = null)
            : base(query, betListData, builder, ticketHelper, true)
        {
            SupportScoreMapBettypeIds = supportScoreMapBettypeIds;
            Title = CacheRefFactory.CacheRef.GetBetTypeName(query.BetType).ToUpperInvariant();
            BetType = query.BetType;
        }

        public int BetType { get; }

        public IEnumerable<int> SupportScoreMapBettypeIds { get; }

        public bool AllowToViewSuperPT => baseQuery.ViewerLevel.IsSuperLevel();

        public bool AllowToViewMasterPT => baseQuery.ViewerLevel.IsMasterOrSuperLevel();

        public override bool CanViewScoreMap(ITicket ticket)
            => SupportScoreMapBettypeIds != null
            && SupportScoreMapBettypeIds.Contains(ticket.BetTypeId);
    }
}
