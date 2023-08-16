namespace Alpha.BetLists.Site.Requests.SearchTickets
{
    using System;
    using Alpha.BetLists.Services;
    using Alpha.BetLists.Site.ViewModels.SearchTickets;
    using BetLists.Models;
    using MediatR;

    public class SearchTicketsQuery : BaseQuery, IRequest<SearchTicketsViewModel>
    {
        public SearchTicketsQuery(IUser viewer) : base(viewer)
        {
            MinDate = ReportDateServiceImpl.Instance.GetBalanceUpDay();
        }

        public long RefNo { get; set; }
        public DateTime MinDate { get; }
    }
}
