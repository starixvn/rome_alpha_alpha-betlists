namespace Alpha.BetLists.Site.Requests.Parlay
{
    using System;
    using BetLists.Models;
    using MediatR;
    using ViewModels.Parlay;

    public class ComboParlayDetailQuery : BaseQuery, IRequest<ComboParlayDetailViewModel>
    {
        public ComboParlayDetailQuery(IUser viewer) : base(viewer)
        {
        }

        public long RefNo { get; set; }

        public DateTime WinLostDate { get; set; }

        public long TransId { get; set; }

        public int BetId { get; set; }
    }
}