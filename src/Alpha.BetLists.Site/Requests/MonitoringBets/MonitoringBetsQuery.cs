namespace Alpha.BetLists.Site.Requests.MonitoringBets
{
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Site.ViewModels.MonitoringBets;
    using MediatR;

    public class MonitoringBetsQuery : BaseQuery, IRequest<MonitoringBetsViewModel>
    {
        public MonitoringBetsQuery(IUser viewer) : base(viewer)
        {
            SubAccId = viewer.SubAccId;
        }

        public int SubAccId { get; }
    }
}