namespace Alpha.BetLists.Site.Requests.MonitoringBets
{
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Site.ViewModels.MonitoringBets;
    using MediatR;

    public class ManageFiltersQuery : BaseQuery, IRequest<ManageFiltersViewModel>
    {
        public ManageFiltersQuery(IUser viewer) : base(viewer)
        {
            SubAccId = viewer.SubAccId;
        }

        public string FilterName { get; set; }

        public int LimitRowCount { get; set; }

        public bool AutoStart { get; set; }

        public int SubAccId { get; }
    }
}