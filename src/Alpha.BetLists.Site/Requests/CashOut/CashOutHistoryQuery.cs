namespace Alpha.BetLists.Site.Requests.CasinoDetails
{
    using System;
    using Alpha.BetLists.Site.ViewModels.CashOutHistory;
    using BetLists.Models;
    using MediatR;

    public class CashOutHistoryQuery : BaseQuery, IRequest<CashOutHistoryViewModel>
    {
        public CashOutHistoryQuery(IUser viewer) : base(viewer)
        {
        }

        public long TransId { get; set; }

        public DateTime WinLostDate { get; set; }
    }
}
