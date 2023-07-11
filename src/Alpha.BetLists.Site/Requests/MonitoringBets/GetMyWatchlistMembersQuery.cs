namespace Alpha.BetLists.Site.Requests.MonitoringBets
{
    using System.Collections.Generic;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Models.MonitoringBets;
    using MediatR;

    public class GetMyWatchlistMembersQuery : BaseQuery, IRequest<IEnumerable<MyWatchlistMember>>
    {
        public GetMyWatchlistMembersQuery(IUser viewer) : base(viewer)
        {
        }
    }
}