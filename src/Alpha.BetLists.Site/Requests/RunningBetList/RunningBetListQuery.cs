namespace Alpha.BetLists.Site.Requests.RunningBetList
{
    using BetLists.Models;
    using Extensions;
    using MediatR;
    using System;
    using ViewModels.RunningBetList;

    public class RunningBetListQuery : BaseQuery, IRequest<RunningBetListViewModel>
    {
        public RunningBetListQuery(IUser viewer) : base(viewer)
        {
            MinDate = DateTime.Today.AddMonths(-3).FirstDayOfMonth();
        }

        public bool GetAll { get; set; }

        public bool IsFilterVisible { get; set; } = true;

        public int UserSelectedDateRangeOption { get; set; }

        public DateTime MinDate { get; }
    }
}