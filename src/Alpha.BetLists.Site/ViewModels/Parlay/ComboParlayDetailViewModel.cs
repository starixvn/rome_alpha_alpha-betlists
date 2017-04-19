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
            ITicketHelper ticketHelper)
        {
            ComboParlayDetailWrappers = new List<ComboParlayDetailWrapper>(comboParlayDetailData.ComboParlayDetails
                    .Select(d => new ComboParlayDetailWrapper(d, comboParlayDetailData.MatchDetails, ticketHelper)));
            ComboParlayDetailWrapperGroups = ComboParlayDetailWrappers.GroupBy(w => w.WinLostDate);

            OutStanding = ConvertByBetType.Stake(comboParlayDetailData.Outstanding);
            Title = Resources.lblSystemParlay;
            TotalWinLost = ConvertByBetType.Winloss(comboParlayDetailData.ComboParlayDetails.Sum(d => d.WinLost + d.PlayerComm));
            IsDataTooHuge = comboParlayDetailData.ComboParlayDetails.Count() > 5000;
            HugeDataLabel = string.Format(Resources.CombinationResultTooHuge, BuildComboParlayDetailExcelUrl(query));
        }

        public IEnumerable<ComboParlayDetailWrapper> ComboParlayDetailWrappers { get; }

        public IEnumerable<IGrouping<string, ComboParlayDetailWrapper>> ComboParlayDetailWrapperGroups { get; }

        public string TotalWinLost { get; }

        public string OutStanding { get; }

        public string Title { get; }

        public bool IsDataTooHuge { get; }

        public string HugeDataLabel { get; }

        public string SubTotalWinLost(IEnumerable<ComboParlayDetailWrapper> group)
            => ConvertByBetType.Winloss(group.Sum(item => item.ComboParlayDetail.WinLost + item.ComboParlayDetail.PlayerComm));

        private string BuildComboParlayDetailExcelUrl(ComboParlayDetailQuery query)
        {
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

            return urlHelper.Action("ComboParlayDetailExcel", "Parlay", query);
        }
    }
}