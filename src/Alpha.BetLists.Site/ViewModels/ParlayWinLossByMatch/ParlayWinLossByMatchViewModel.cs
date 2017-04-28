using Alpha.BetLists.AppResources;
using Alpha.BetLists.Models;
using Alpha.BetLists.Site.Requests;
using Alpha.BetLists.Site.Requests.WinLossByMatch;
using Alpha.BetLists.Site.ViewModels.WinLoss;
using Alpha.RemotingRef;
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
            Title = HasDateParam(query)
                ? $"{CacheRefFactory.CacheRef.GetBetTypeName(BetType)} {query.FromDate.ToShortDateString()} --> {query.ToDate.ToShortDateString()}"
                : Resources.memberBetList;
        }

        public int BetType { get; set; }

        public byte CustLevelId { get; set; }
    }
}