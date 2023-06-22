namespace Alpha.BetLists.Site.Requests.MonitoringBets
{
    using System.Collections.Generic;
    using Alpha.BetLists.Models;
    using Alpha.Web.Models;
    using MediatR;

    public class DeleleFiltersCommand : BaseQuery, IRequest<IEnumerable<Result>>
    {
        public DeleleFiltersCommand(IUser viewer) : base(viewer)
        {
            IsInternal = viewer.IsInternal;
            SubAccId = viewer.SubAccId;
            IP = viewer.IP;
        }

        public int[] Ids { get; set; }

        public bool IsInternal { get; }

        public int SubAccId { get; }

        public string IP { get; }
    }
}