namespace Alpha.BetLists.Site.ViewModels.CorrectScore
{
    using System.Collections.Generic;
    using BetListByBetType;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests.CorrectScore;

    public class CorrectScoreViewModel : BetListByBetTypeViewModel
    {
        public CorrectScoreViewModel(
            CorrectScoreQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            IEnumerable<int> supportScoreMapBetTypeIds = null)
            : base(query, betListData, builder, ticketHelper, supportScoreMapBetTypeIds)
        {
        }
    }
}
