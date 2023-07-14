namespace Alpha.BetLists.Site.Requests.MonitoringBets
{
    using System.Collections.Generic;
    using BetLists.Models;
    using Configurations;
    using MediatR;
    using Web.Models;

    public class CheckCanViewLastFilteringBetsQuery : IRequest<Result>
    {
        public CheckCanViewLastFilteringBetsQuery(IUser viewer)
        {
            CustId = viewer.Id;
            IsInternal = viewer.IsInternal;
        }

        public int CustId { get; }

        public bool IsInternal { get; }

        public bool IsEnableLastBetsMonitoring { get; } = AppSettings.EnableBetsListMonitoring;

        public IEnumerable<int> UsersAlwaysAllowAccessBetsListMonitoring { get; } = AppSettings.UsersAlwaysAllowAccessBetsListMonitoring;
    }
}