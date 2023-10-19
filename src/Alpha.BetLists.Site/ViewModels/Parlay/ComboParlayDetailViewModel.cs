namespace Alpha.BetLists.Site.ViewModels.Parlay
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using AppResources;
    using BetLists.Models.Parlay;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Requests.Parlay;

    public class ComboParlayDetailViewModel
    {
        private readonly Dictionary<string, int> statusOrder = new Dictionary<string, int>
        {
            { string.Empty, 0 },
            { BetStatus.Won, 1 },
            { BetStatus.Lose, 2 },
            { BetStatus.Draw, 3 },
            { BetStatus.Void, 4 },
            { BetStatus.Reject, 5 },
            { BetStatus.Refund, 6 }
        };

        public ComboParlayDetailViewModel(
            ComboParlayDetailQuery query,
            ComboParlayDetailData comboParlayDetailData,
            ITicketHelper ticketHelper,
            bool isMobile,
            bool isExportingExcel = false)
        {
            IsDataTooHuge = comboParlayDetailData.ComboParlayDetails.Count() > 5000;
            Query = query;

            Title = query.BetTeam == "1"
                ? Resources.lblMixParlay
                : Resources.lblSystemParlay;

            var allowInitializingData = isExportingExcel || !IsDataTooHuge;

            if (allowInitializingData)
            {
                ComboParlayDetailWrappers = new List<ComboParlayDetailWrapper>(comboParlayDetailData.ComboParlayDetails
                        .OrderBy(i => statusOrder[i.Status?.ToLowerInvariant() ?? string.Empty])
                        .Select(d => new ComboParlayDetailWrapper(d, comboParlayDetailData.MatchDetails, ticketHelper)));
                ComboParlayDetailWrapperGroups = ComboParlayDetailWrappers.GroupBy(w => w.WinLostDate).OrderBy(i => i.Key);

                OutStanding = ConvertByBetType.Stake(comboParlayDetailData.Outstanding);
                IsParlayBoost = comboParlayDetailData.IsParlayBoost;
                TotalWinLost = ConvertByBetType.Winloss(comboParlayDetailData.ComboParlayDetails.Sum(d => d.WinLost + d.PlayerComm));
                IsRunningTicket = !comboParlayDetailData.ComboParlayDetails.Any();
            }

            IsMobile = isMobile;
        }

        public bool IsMobile { get; }

        public ComboParlayDetailQuery Query { get; }

        public IEnumerable<ComboParlayDetailWrapper> ComboParlayDetailWrappers { get; }

        public IEnumerable<IGrouping<string, ComboParlayDetailWrapper>> ComboParlayDetailWrapperGroups { get; }

        public string TotalWinLost { get; }

        public string OutStanding { get; }

        public bool IsParlayBoost { get; }

        public string Title { get; }

        public bool IsDataTooHuge { get; }

        public bool IsRunningTicket { get; }

        public string HugeDataLabel => string.Format(Resources.CombinationResultTooHuge, BuildComboParlayDetailExcelPath(Query));

        public string SubTotalWinLost(IEnumerable<ComboParlayDetailWrapper> group)
            => ConvertByBetType.Winloss(group.Sum(item => item.ComboParlayDetail.WinLost + item.ComboParlayDetail.PlayerComm));

        private static string BuildComboParlayDetailExcelPath(ComboParlayDetailQuery query)
        {
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

            return urlHelper.Action("ComboParlayDetailExcel", "Parlay", query);
        }
    }
}
