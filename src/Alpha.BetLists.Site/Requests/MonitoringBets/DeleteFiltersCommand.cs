namespace Alpha.BetLists.Site.Requests.MonitoringBets
{
    using System.Collections.Generic;
    using Alpha.BetLists.Models;
    using Alpha.Web.Models;
    using MediatR;

    public class DeleteFiltersCommand : BaseQuery, IRequest<IEnumerable<Result>>
    {
        public DeleteFiltersCommand(IUser viewer) : base(viewer)
        {
            IsInternal = viewer.IsInternal;
            SubAccId = viewer.SubAccId;
            IP = viewer.IP;
        }

        public string Ids { get; set; }

        public bool IsInternal { get; }

        public int SubAccId { get; }

        public string IP { get; }

        public bool IsValid() => !string.IsNullOrWhiteSpace(Ids);
    }
}