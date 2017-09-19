namespace Alpha.BetLists.Site.Requests.CasioDetails
{
    using BetLists.Models;
    using Fanex.BetDetails.Casino.ViewModels;
    using MediatR;

    public class CasinoDetailsQuery : BaseQuery, IRequest<CasinoDetailsViewModel>
    {
        public CasinoDetailsQuery(IUser viewer) : base(viewer)
        {
            PageIndex = 1;
            PageSize = 100;
        }

        public long RefNo { get; set; }

        public byte TransType { get; set; }

        public string SportName { get; set; }

        public int ProductId { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
