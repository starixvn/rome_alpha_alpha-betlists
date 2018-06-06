namespace Alpha.BetLists.Site.Requests.CasinoDetails
{
    using BetLists.Models;
    using MediatR;
    using ViewModels.CasinoDetails;

    public class CasinoDetailsQuery : BaseQuery, IRequest<CasinoDetailsViewModel>
    {
        private const int DefaultPageIndex = 1;
        private const int DefaultPageSize = 500;

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
