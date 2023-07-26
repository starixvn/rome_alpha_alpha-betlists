namespace Alpha.BetLists.Site.Requests.FindTickets
{
    using System;
    using Alpha.BetLists.Services;
    using Alpha.BetLists.Site.ViewModels.FindTickets;
    using BetLists.Models;
    using MediatR;

    public class FindTicketsQuery : BaseQuery, IRequest<FindTicketsViewModel>
    {
        public FindTicketsQuery(IUser viewer) : base(viewer)
        {
            MinDate = ReportDateServiceImpl.Instance.GetMoveBetsDay().AddDays(1);
        }

        public long RefNo { get; set; }
        public DateTime MinDate { get; }
    }
}
