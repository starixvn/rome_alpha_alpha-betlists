namespace Alpha.BetLists.Site.Requests.WinLoss
{
    using System;
    using Alpha.BetLists.Extensions;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Site.ViewModels.WinLoss;
    using Alpha.BetLists.Utils;
    using MediatR;

    public class WinLossNewQuery : WinLossQuery, IRequest<WinLossNewViewModel>
    {
        public WinLossNewQuery(IUser viewer) : base(viewer)
        {
        }

        public string BreadcrumbLevelJson { get; set; }

        public override DateTime MinDate
            => SystemTime.Today.AddMonths(NegativeThree)
            .FirstDayOfMonth()
            .GetGreaterDate(LimitedDateToShowInfo);

        public DateTime HistoricMinDate
            => SystemTime.Today.AddYears(NegativeThree)
            .FirstDayOfYear()
            .GetGreaterDate(LimitedDateToShowInfo);
    }
}