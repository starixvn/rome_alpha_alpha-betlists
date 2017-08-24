using System.Collections.Generic;
using Alpha.BetLists.Configurations;
using Alpha.BetLists.Models;
using Alpha.Web.Models;
using MediatR;

namespace Alpha.BetLists.Site.Requests.LastBets
{
    public class CheckCanViewLastBetsQuery : IRequest<Result>
    {
        public CheckCanViewLastBetsQuery(IUser viewer)
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