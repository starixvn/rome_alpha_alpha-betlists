namespace Alpha.BetLists.Site.Requests.WinLoss
{
    using System;
    using System.Collections.Generic;
    using Alpha.BetLists.Extensions;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Site.ViewModels.WinLoss;
    using Alpha.BetLists.Utils;
    using MediatR;

    public class WinLossBetListWeekSelectionQuery : WinLossQuery, IRequest<WinLossBetListWeekSelectionViewModel>
    {
        public WinLossBetListWeekSelectionQuery(IUser viewer) : base(viewer)
        {
            var today = SystemTime.Today;
            MinDate = today.AddYears(NegativeThree)
               .FirstDayOfYear()
               .GetGreaterDate(LimitedDateToShowInfo);
        }

        public override DateTime MinDate { get; }

        private IDictionary<int, bool> userSelectedProductIds;
        public IDictionary<int, bool> UserSelectedProductIds
        {
            get
            {
                userSelectedProductIds = userSelectedProductIds ?? ProductIds.ToIntDictionary();

                return userSelectedProductIds;
            }
        }

        private IDictionary<int, bool> userSelectedResourceIds;
        public IDictionary<int, bool> UserSelectedResourceIds
        {
            get

            {
                userSelectedResourceIds = userSelectedResourceIds ?? ResourceIds.ToIntDictionary();

                return userSelectedResourceIds;
            }
        }

        public string BreadcrumbLevelJson { get; set; }

        public virtual bool IsValid() => MinDate <= FromDate && FromDate <= ToDate;
    }
}