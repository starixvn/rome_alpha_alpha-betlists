namespace Alpha.BetLists.Site.ViewModels.Parlay
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Utils;
    using System.Linq;
    using AppResources;
    using BetLists.Models.Parlay;
    using Fanex.BetList.Core.Entities;
    using System.Web;
    using Requests.Parlay;
    using System.Web.Mvc;

    public class ComboParlayDetailViewModel
    {
        public ComboParlayDetailViewModel(
            ComboParlayDetailQuery query,
            ComboParlayDetailData comboParlayDetailData,
            ITicketHelper ticketHelper,
            bool isExportingExcel = false)
        {
            IsDataTooHuge = comboParlayDetailData.ComboParlayDetails.Count() > 5000;
            Query = query;
            Title = Resources.lblSystemParlay;
            var allowInitializingData = isExportingExcel || !IsDataTooHuge;

            if (allowInitializingData)
            {
                ComboParlayDetailWrappers = new List<ComboParlayDetailWrapper>(comboParlayDetailData.ComboParlayDetails
                        .Select(d => new ComboParlayDetailWrapper(d, comboParlayDetailData.MatchDetails, ticketHelper)));
                ComboParlayDetailWrapperGroups = ComboParlayDetailWrappers.GroupBy(w => w.WinLostDate);

                OutStanding = ConvertByBetType.Stake(comboParlayDetailData.Outstanding);
                TotalWinLost = ConvertByBetType.Winloss(comboParlayDetailData.ComboParlayDetails.Sum(d => d.WinLost + d.PlayerComm));
                IsRunningTicket = !comboParlayDetailData.ComboParlayDetails.Any();
            }
        }

        public ComboParlayDetailQuery Query { get; }

        public IEnumerable<ComboParlayDetailWrapper> ComboParlayDetailWrappers { get; }

        public IEnumerable<IGrouping<string, ComboParlayDetailWrapper>> ComboParlayDetailWrapperGroups { get; }

        public string TotalWinLost { get; }

        public string OutStanding { get; }

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