namespace Alpha.BetLists.Site.Requests.MonitoringBets
{
    using System.Collections.Generic;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Models.MonitoringBets;
    using MediatR;

    public class GetMonitoredWatchlistMembersQuery : BaseQuery, IRequest<IEnumerable<MonitoredWatchlistMember>>
    {
        public GetMonitoredWatchlistMembersQuery(IUser viewer) : base(viewer)
        {
        }
    }
}