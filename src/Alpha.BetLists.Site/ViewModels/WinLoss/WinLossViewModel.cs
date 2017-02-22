namespace Alpha.BetLists.Site.ViewModels.WinLoss
{
    using BetLists.Models;
    using Requests.WinLoss;

    public class WinLossViewModel : BetListBaseViewModel
    {
        public WinLossViewModel(WinLossQuery query, BetListData betListData)
            : base(query, betListData)
        {
        }

        public bool AllowToViewSuperPT => baseQuery.ViewerLevel.IsSuperLevel();

        public bool AllowToViewMasterPT => baseQuery.ViewerLevel.IsMasterOrSuperLevel();
    }
}