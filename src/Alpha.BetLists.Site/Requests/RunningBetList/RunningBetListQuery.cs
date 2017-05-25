namespace Alpha.BetLists.Site.Requests.RunningBetList
{
    using BetLists.Models;
    using Extensions;
    using MediatR;
    using System;
    using Utils;
    using ViewModels.RunningBetList;

    public class RunningBetListQuery : BaseQuery, IRequest<RunningBetListViewModel>
    {
        public RunningBetListQuery(IUser viewer) : base(viewer)
        {
            MinDate = SystemTime.Today.AddMonths(-3).FirstDayOfMonth();
            FromDate = SystemTime.Today.AddDays(-7);
            ToDate = SystemTime.Today;
        }

        public bool GetAll { get; set; }

        public bool IsFilterVisible { get; set; } = true;

        public int UserSelectedDateRangeOption { get; set; }

        public DateTime MinDate { get; }
    }
}