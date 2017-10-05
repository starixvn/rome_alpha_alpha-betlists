namespace Alpha.BetLists.Site.ViewModels.LastBets
{
    using System.Collections.Generic;
    using System.Linq;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests.LastBets;

    public class BetDataViewModel : BetListBaseViewModel
    {
        public BetDataViewModel(
            long latestTransId,
            GetLastBetsQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            IEnumerable<int> supportScoreMapBetTypeIds)
            : base(query, betListData, builder, ticketHelper, showMemberName: true)
        {
            LatestTransId = latestTransId;
            TotalColumn = 7;
            SupportScoreMapBetTypeIds = supportScoreMapBetTypeIds;
        }

        public IEnumerable<int> SupportScoreMapBetTypeIds { get; }

        public long LatestTransId { get; }

        public long LastTransId
            => BetListData.Tickets.Any()
            ? BetListData.Tickets.ElementAt(BetListData.Total - 1).TransId
            : 0;

        public override bool CanViewScoreMap(ITicket ticket) => SupportScoreMapBetTypeIds.Contains(ticket.BetTypeId);
    }
}