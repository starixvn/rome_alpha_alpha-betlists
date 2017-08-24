using System.Collections.Generic;

namespace Alpha.BetLists.Params.LastBets
{
    public class CheckCanViewLastBetsParam
    {
        public CheckCanViewLastBetsParam(
            int custId,
            bool isInternal,
            bool isEnableLastBetsMonitoring,
            IEnumerable<int> usersAlwaysAllowAccessBetsListMonitoring)
        {
            CustId = custId;
            IsInternal = isInternal;
            IsEnableLastBetsMonitoring = isEnableLastBetsMonitoring;
            UsersAlwaysAllowAccessBetsListMonitoring = usersAlwaysAllowAccessBetsListMonitoring;
        }

        public int CustId { get; }

        public bool IsInternal { get; }

        public bool IsEnableLastBetsMonitoring { get; }

        public IEnumerable<int> UsersAlwaysAllowAccessBetsListMonitoring { get; }
    }
}
