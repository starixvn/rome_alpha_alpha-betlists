namespace Alpha.BetLists.Site.Requests.MonitoringBets
{
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Site.ViewModels.MonitoringBets;
    using MediatR;

    public class MonitoringBetsQuery : BaseQuery, IRequest<MonitoringBetsViewModel>
    {
        public MonitoringBetsQuery(IUser viewer) : base(viewer)
        {
            IsInternal = viewer.IsInternal;
        }

        public string FilterName { get; set; }

        public int LimitRowCount { get; set; } = 100;

        public bool AutoStart { get; set; } = true;

        public bool IsInternal { get; }
    }
}