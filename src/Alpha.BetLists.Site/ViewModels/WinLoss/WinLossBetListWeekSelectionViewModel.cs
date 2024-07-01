namespace Alpha.BetLists.Site.ViewModels.WinLoss
{
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Site.Requests.WinLoss;
    using Alpha.BetLists.Site.ViewModels.Components;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Newtonsoft.Json;

    public class WinLossBetListWeekSelectionViewModel : WinLossViewModel
    {
        public WinLossBetListWeekSelectionViewModel(
            WinLossBetListWeekSelectionQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            bool showContributionAmountRoundNote,
            bool isGroupHeadline = false,
            bool showMemberName = false,
            string[] excludingTicketStatus = null,
            BetlistOutputParams betlistOutputParams = null)
            : base(
                  query,
                  betListData,
                  builder,
                  ticketHelper,
                  showContributionAmountRoundNote,
                  isGroupHeadline,
                  showMemberName,
                  excludingTicketStatus,
                  betlistOutputParams)
        {
            BreadcrumbLevels = string.IsNullOrWhiteSpace(query.BreadcrumbLevelJson)
                ? Enumerable.Empty<BreadcrumbLevelViewModel>()
                : JsonConvert.DeserializeObject<IEnumerable<BreadcrumbLevelViewModel>>(query.BreadcrumbLevelJson);
        }

        public IEnumerable<BreadcrumbLevelViewModel> BreadcrumbLevels { get; set; }
    }
}