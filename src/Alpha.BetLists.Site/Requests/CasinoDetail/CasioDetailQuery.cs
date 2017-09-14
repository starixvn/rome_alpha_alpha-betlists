namespace Alpha.BetLists.Site.Requests.CasioDetail
{
    using BetLists.Models;
    using MediatR;
    using ViewModels.CasinoDetail;

    public class CasioDetailQuery : BaseQuery, IRequest<CasinoDetailViewModel>
    {
        public CasioDetailQuery(IUser viewer) : base(viewer)
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
