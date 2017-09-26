namespace Alpha.BetLists.Site.ViewModels.LastBets
{
    using BetLists.Constants;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests.LastBets;

    public class LastBetsViewModel : BetListBaseViewModel
    {
        public LastBetsViewModel(
            LastBetsQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            string lastTransIdCacheKey,
            string latestTransIdCacheKey)
            : base(query, betListData, builder, ticketHelper, showMemberName: true)
        {
            PageSize = query.PageSize;
            ResetLastTransId = query.BetType == -1;
            IsAutoRefresh = query.IsAutoRefresh;

            BetType = query.BetType == -1 ? BetTypeIdConstants.AllBetTypes : query.BetType;
            LastTransIdCacheKey = lastTransIdCacheKey;
            LatestTransIdCacheKey = latestTransIdCacheKey;
        }

        public int BetType { get; }

        public int PageSize { get; }

        public bool IsAutoRefresh { get; }

        public string LastTransIdCacheKey { get; }

        public string LatestTransIdCacheKey { get; }

        public bool ResetLastTransId { get; }

        public bool IsSelectedBetType(int betType) => betType == BetType;

        public bool IsSelectedPageSize(int pageSize) => pageSize == PageSize;
    }
}