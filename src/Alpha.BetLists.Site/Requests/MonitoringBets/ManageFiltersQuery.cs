namespace Alpha.BetLists.Site.Requests.MonitoringBets
{
    using System.Collections.Generic;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Models.MonitoringBets;
    using MediatR;

    public class ManageFiltersQuery : BaseQuery, IRequest<IEnumerable<FilterConfiguration>>
    {
        public ManageFiltersQuery(IUser viewer) : base(viewer)
        {
            SubAccId = viewer.SubAccId;
        }

        public int SubAccId { get; }
    }
}