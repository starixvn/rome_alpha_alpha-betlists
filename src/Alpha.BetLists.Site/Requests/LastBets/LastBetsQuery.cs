using Alpha.BetLists.Models;
using Alpha.BetLists.Site.ViewModels.LastBets;
using MediatR;

namespace Alpha.BetLists.Site.Requests.LastBets
{
    public class LastBetsQuery : BaseQuery, IRequest<LastBetsViewModel>
    {
        public LastBetsQuery(IUser viewer) : base(viewer)
        {
        }

        public int BetType { get; set; } = -1;

        public int PageSize { get; set; } = 10;

        public bool IsAutoRefresh { get; set; } = true;
    }
}