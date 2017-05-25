namespace Alpha.BetLists.Site.Requests.MemberBetList
{
    using BetLists.Models;
    using Extensions;
    using MediatR;
    using System;
    using Utils;
    using ViewModels.MemberBetList;

    public class MemberBetListQuery : BaseQuery, IRequest<MemberBetListViewModel>
    {
        public MemberBetListQuery(IUser viewer) : base(viewer)
        {
            MinDate = SystemTime.Today.AddMonths(-3).FirstDayOfMonth();
            FromDate = SystemTime.Today;
            ToDate = SystemTime.Today.AddDays(7);
        }

        public bool IsFilterVisible { get; set; } = true;

        public int UserSelectedDateRangeOption { get; set; }

        public DateTime MinDate { get; }
    }
}