using System.Collections.Generic;
using System.Linq;
using Alpha.BetLists.Models;
using Alpha.BetLists.Site.Requests.LastBets;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace Alpha.BetLists.Site.ViewModels.LastBets
{
    public class BetDataViewModel : BetListBaseViewModel
    {
        public BetDataViewModel(
            GetLastBetsQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            IEnumerable<int> supportScoreMapBettypeIds)
            : base(query, betListData, builder, ticketHelper, showMemberName: true)
        {
            TotalColumn = 7;
            SupportScoreMapBettypeIds = supportScoreMapBettypeIds;
        }

        public IEnumerable<int> SupportScoreMapBettypeIds { get; }

        public long LastTransId
            => BetListData.Tickets.Any()
            ? BetListData.Tickets.ElementAt(BetListData.Total - 1).TransId
            : 0;

        public override bool CanViewScoreMap(ITicket ticket) => SupportScoreMapBettypeIds.Contains(ticket.BetTypeId);
    }
}