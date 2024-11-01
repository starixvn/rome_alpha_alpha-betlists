namespace Alpha.BetLists.Site.ViewModels.LastBets
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Alpha.BetLists.Services;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests.LastBets;

    public class BetDataViewModel : BetListBaseViewModel
    {
        private readonly LastBetsService lastBetsService;
        private readonly GetLastBetsQuery query;

        public BetDataViewModel(
            long latestTransId,
            GetLastBetsQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            IEnumerable<int> supportScoreMapBetTypeIds,
            LastBetsService lastBetsService)
            : base(query, betListData, builder, ticketHelper, showMemberName: true)
        {
            LatestTransId = latestTransId;
            TotalColumn = 10;
            SupportScoreMapBetTypeIds = supportScoreMapBetTypeIds;
            this.lastBetsService = lastBetsService ?? DependencyResolver.Current.GetService<LastBetsService>();
            this.query = query;
        }

        public IEnumerable<int> SupportScoreMapBetTypeIds { get; }

        public long LatestTransId { get; }

        public long LastTransId
            => BetListData.Tickets.Any()
            ? BetListData.Tickets.ElementAt(BetListData.Total - 1).TransId
            : 0;

        public override bool CanViewScoreMap(ITicket ticket)
        {
            var status = lastBetsService.GetMatchInfoById(ticket.MatchId, query.RunningMatchCacheTime)?.Status;
            return !string.IsNullOrWhiteSpace(status)
                && !status.Equals("completed", System.StringComparison.OrdinalIgnoreCase)
                && SupportScoreMapBetTypeIds.Contains(ticket.BetTypeId);
        }
    }
}