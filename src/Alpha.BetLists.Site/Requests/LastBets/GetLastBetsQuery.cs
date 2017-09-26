namespace Alpha.BetLists.Site.Requests.LastBets
{
    using BetLists.Models;
    using MediatR;
    using ViewModels.LastBets;

    public class GetLastBetsQuery : BaseQuery, IRequest<BetDataViewModel>
    {
        public GetLastBetsQuery(IUser viewer) : base(viewer)
        {
        }

        public int BetType { get; set; }

        public long TransId { get; set; }

        public int PageSize { get; set; }

        public bool IsAutoLoad { get; set; }

        public long LatestTransId { get; set; }
    }
}