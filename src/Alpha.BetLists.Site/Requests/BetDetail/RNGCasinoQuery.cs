namespace Alpha.BetLists.Site.Requests.BetDetail
{
    using BetLists.Models;
    using MediatR;
    using ViewModels.BetDetail;

    public class RNGCasinoQuery : BaseQuery, IRequest<BetDetailViewModel>
    {
        public RNGCasinoQuery(IUser viewer) : base(viewer)
        {
            PageIndex = 1;
            PageSize = 100;
        }

        public long RefNo { get; set; }

        public byte TransType { get; set; }

        public string SportName { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
