namespace Alpha.BetLists.Site.Requests.MonitoringBets
{
    using System.Collections.Generic;
    using Alpha.BetLists.Models;
    using MediatR;

    public class FilterBySportsQuery : BaseQuery, IRequest<IEnumerable<Sport>>
    {
        public FilterBySportsQuery(IUser viewer) : base(viewer)
        {
        }

        public byte GroupSport { get; set; }
    }
}