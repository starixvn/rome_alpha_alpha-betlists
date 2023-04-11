namespace Alpha.BetLists.Site.Requests.WinLoss
{
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Site.ViewModels.WinLoss;
    using MediatR;

    public class WinLossIndexQuery : WinLossBetListWeekSelectionQuery, IRequest<WinLossIndexViewModel>
    {
        public WinLossIndexQuery(IUser viewer) : base(viewer)
        {
        }

        public override bool IsValid()
            => base.IsValid() && IsValidWeekRange();

        private bool IsValidWeekRange()
        {
            if (!WeekFromDate.HasValue && !WeekToDate.HasValue)
            {
                return true;
            }

            return WeekFromDate.HasValue
                && WeekToDate.HasValue
                && WeekFromDate.Value >= FromDate
                && WeekToDate.Value <= ToDate;
        }
    }
}