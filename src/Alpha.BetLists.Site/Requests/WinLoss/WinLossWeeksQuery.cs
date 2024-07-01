namespace Alpha.BetLists.Site.Requests.WinLoss
{
    using System.Collections.Generic;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Site.ViewModels.Components;
    using MediatR;

    public class WinLossWeeksQuery : WinLossBetListWeekSelectionQuery, IRequest<IEnumerable<WeekSelectionViewModel>>
    {
        public WinLossWeeksQuery(IUser viewer) : base(viewer)
        {
        }
    }
}