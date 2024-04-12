namespace Alpha.BetLists.Site.Features.BookedBet
{
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Site.Requests;
    using Alpha.BetLists.Utils;
    using BetLists.Constants;
    using Extensions;
    using MediatR;
    using System;

    public class BookedBetBaseQuery : BaseQuery
    {
        protected BookedBetBaseQuery(IUser viewer) : base(viewer)
        {
            const int daysago = -30;
            var today = SystemTime.Today;
            MinDate = today.AddDays(daysago)
               .GetGreaterDate(LimitedDateToShowInfo);

            FromDate = FromDate == DateTime.MinValue ? today : FromDate;
            ToDate = ToDate == DateTime.MinValue ? today : ToDate;
            StatusIds = string.IsNullOrEmpty(StatusIds) ? BookedBetStatusConstants.Dropdownlist.JoinWithComma() : StatusIds;
        }

        public string StatusIds { get; set; }

        public DateTime MinDate { get; }

        public int UserSelectedDateRangeOption { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public bool IsFilterVisible { get; set; } = true;

    }
}