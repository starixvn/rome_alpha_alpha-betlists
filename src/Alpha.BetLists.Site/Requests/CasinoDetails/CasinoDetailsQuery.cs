namespace Alpha.BetLists.Site.Requests.CasioDetails
{
    using BetLists.Models;
    using Fanex.BetDetails.Casino.ViewModels;
    using MediatR;

    public class CasinoDetailsQuery : BaseQuery, IRequest<CasinoDetailsViewModel>
    {
        private const int DefaultPageIndex = 1;
        private const int DefaultPageSize = 100;

        public CasinoDetailsQuery(IUser viewer) : base(viewer)
        {
            PageIndex = DefaultPageIndex;
            PageSize = DefaultPageSize;
        }

        public long RefNo { get; set; }

        public byte TransType { get; set; }

        public string SportName { get; set; }

        public int ProductId { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
