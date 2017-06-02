using Alpha.BetLists.Models;
using Alpha.BetLists.Site.ViewModels.LastBets;
using MediatR;

namespace Alpha.BetLists.Site.Requests.LastBets
{
    public class GetLastBetsQuery : BaseQuery, IRequest<BetDataViewModel>
    {
        public GetLastBetsQuery(IUser viewer) : base(viewer)
        {
        }

        public int BetType { get; set; }

        public long TransId { get; set; }

        public int PageSize { get; set; }

        public bool IsAutoLoad { get; set; }
    }
}