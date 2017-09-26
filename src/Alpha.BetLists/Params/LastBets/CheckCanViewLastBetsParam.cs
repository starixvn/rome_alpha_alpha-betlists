namespace Alpha.BetLists.Params.LastBets
{
    using System.Collections.Generic;

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
