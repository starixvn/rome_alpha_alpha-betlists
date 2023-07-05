namespace Alpha.BetLists.Site.Requests.WinLoss
{
    using System;
    using System.Collections.Generic;
    using Alpha.BetLists.Extensions;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Site.ViewModels.WinLoss;
    using Alpha.BetLists.Utils;
    using MediatR;

    public class WinLossBetListQuery : WinLossQuery, IRequest<WinLossViewModel>
    {
        private const int HistoryMaxDateRange = 31;

        public WinLossBetListQuery(IUser viewer) : base(viewer)
        {
            var today = SystemTime.Today;
            MinDate = today.AddYears(NegativeThree)
               .FirstDayOfYear()
               .GetGreaterDate(LimitedDateToShowInfo);
            HistoricPivotDate = today.AddMonths(NegativeThree).FirstDayOfMonth();
        }

        public override DateTime MinDate { get; }

        public DateTime HistoricPivotDate { get; }

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

        public virtual bool IsValid()
        {
            if (FromDate <= ToDate && MinDate <= FromDate)
            {
                return !(
                    ToDate < HistoricPivotDate && ToDate.Subtract(FromDate).TotalDays >= HistoryMaxDateRange
                    || FromDate < HistoricPivotDate && HistoricPivotDate <= ToDate && ToDate.Subtract(FromDate).TotalDays >= HistoryMaxDateRange
                );
            }

            return false;
        }
    }
}