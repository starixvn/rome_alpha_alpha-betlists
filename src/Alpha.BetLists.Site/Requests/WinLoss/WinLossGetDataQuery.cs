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
        private const int HistoryMaxDateRange = 31;

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

        public DateTime? WeekFromDate { get; set; }

        public DateTime? WeekToDate { get; set; }

        public bool IsValid()
        {
            if (FromDate <= ToDate && MinDate <= FromDate)
            {
                var historicPivotDate = HistoricPivotDate;

                var isValid = !(
                    ToDate < historicPivotDate && ToDate.Subtract(FromDate).TotalDays >= HistoryMaxDateRange
                    || FromDate < historicPivotDate && historicPivotDate <= ToDate && ToDate.Subtract(FromDate).TotalDays >= HistoryMaxDateRange
                );

                return isValid && IsValidWeekRange();
            }

            return false;
        }

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