namespace Alpha.BetLists.Site.Requests.MemberBetList
{
    using System;
    using Alpha.BetLists.Services;
    using BetLists.Models;
    using Extensions;
    using MediatR;
    using Utils;
    using ViewModels.MemberBetList;

    public class MemberBetListQuery : BaseQuery, IRequest<MemberBetListViewModel>
    {
        public MemberBetListQuery(IUser viewer) : base(viewer)
        {
            MinDate = ReportDateServiceImpl.Instance.GetMoveBetsDay().AddDays(1);
            FromDate = SystemTime.Today;
            ToDate = SystemTime.Today;
        }

        public bool IsFilterVisible { get; set; } = true;

        public int UserSelectedDateRangeOption { get; set; }

        public DateTime MinDate { get; }
    }
}