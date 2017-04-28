using Alpha.BetLists.Models;
using Alpha.BetLists.Site.Requests.WinLossByMatch;
using Alpha.BetLists.Site.ViewModels.WinLoss;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace Alpha.BetLists.Site.ViewModels.ParlayWinLossByMatch
{
    public class ParlayWinLossByMatchViewModel : WinLossViewModel
    {
        public ParlayWinLossByMatchViewModel(
            ParlayWinLossByMatchQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper)
            : base(query, betListData, builder, ticketHelper, showMemberName: true)
        {
            BetType = query.BetType;
            CustLevelId = query.CustLevelId;
        }

        public int BetType { get; set; }

        public byte CustLevelId { get; set; }
    }
}