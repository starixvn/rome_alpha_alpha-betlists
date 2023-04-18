namespace Alpha.BetLists.Site.Requests.WinLoss
{
    using System;
    using System.Collections.Generic;
    using Alpha.BetLists.Extensions;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Site.ViewModels.WinLoss;
    using Alpha.BetLists.Utils;
    using MediatR;

    public class WinLossGetDataQuery : WinLossQuery, IRequest<WinLossViewModel>
    {
        public WinLossGetDataQuery(IUser viewer) : base(viewer)
        {
        }

        public string BreadcrumbLevelJson { get; set; }

        public override DateTime MinDate
            => SystemTime.Today.AddYears(NegativeThree)
            .FirstDayOfYear()
            .GetGreaterDate(LimitedDateToShowInfo);

        public DateTime HistoricPivotDate
            => SystemTime.Today.AddMonths(NegativeThree).FirstDayOfMonth();

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

        public bool IsValid()
        {
            if (FromDate <= ToDate && MinDate <= FromDate)
            {
                var historicPivotDate = HistoricPivotDate;
                const int maxDaysInOneMonth = 31;

                return !(
                    ToDate < historicPivotDate && ToDate.Subtract(FromDate).TotalDays >= maxDaysInOneMonth
                    || FromDate < historicPivotDate && historicPivotDate <= ToDate
                );
            }

            return false;
        }
    }
}