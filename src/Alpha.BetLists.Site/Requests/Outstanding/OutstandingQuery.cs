namespace Alpha.BetLists.Site.Requests.Outstanding
{
    using BetLists.Models;
    using Extensions;
    using MediatR;
    using System;
    using Utils;
    using ViewModels.Outstanding;

    public class OutstandingQuery : BaseQuery, IRequest<OutstandingViewModel>
    {
        public OutstandingQuery(IUser viewer) : base(viewer)
        {
            MinDate = SystemTime.Today.AddMonths(-3).FirstDayOfMonth();
        }

        public bool GetAll { get; set; }

        public DateTime MinDate { get; }

        public int UserSelectedDateRangeOption { get; set; }
    }
}