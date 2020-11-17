namespace Alpha.BetLists.Site.Requests.RunningBetList
{
    using System;
    using Alpha.BetLists.Services;
    using BetLists.Models;
    using Constants;
    using Extensions;
    using MediatR;
    using Utils;
    using ViewModels.RunningBetList;

    public class RunningBetListQuery : BaseQuery, IRequest<RunningBetListViewModel>
    {
        public RunningBetListQuery(IUser viewer) : base(viewer)
        {
            MinDate = ReportDateServiceImpl.Instance.GetMoveBetsDay().AddDays(1);
            FromDate = SystemTime.Today.AddDays(-7);
            ToDate = SystemTime.Today;
        }

        public bool GetAll { get; set; }

        public bool IsFilterVisible { get; set; } = true;

        public int UserSelectedDateRangeOption { get; set; } = UserSelectedDateRangeOptionConstant.SpecificDates;

        public DateTime MinDate { get; }
    }
}