namespace Alpha.BetLists.Site.Requests.MemberBetList
{
    using BetLists.Models;
    using Extensions;
    using MediatR;
    using System;
    using ViewModels.MemberBetList;

    public class MemberBetListQuery : BaseQuery, IRequest<MemberBeListViewModel>
    {
        public MemberBetListQuery(IUser viewer) : base(viewer)
        {
            MinDate = DateTime.Today.AddMonths(-3).FirstDayOfMonth();
        }

        public bool IsFilterVisible { get; set; } = true;

        public int UserSelectedDateRangeOption { get; set; }

        public DateTime MinDate { get; }
    }
}