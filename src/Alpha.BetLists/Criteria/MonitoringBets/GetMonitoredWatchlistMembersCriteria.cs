namespace Alpha.BetLists.Criteria.MonitoringBets
{
    using Fanex.Data.Repository;

    public class GetMonitoredWatchlistMembersCriteria : CriteriaBase
    {
        public GetMonitoredWatchlistMembersCriteria(int agentId)
            => AgentId = agentId;

        public int AgentId { get; }

        public bool ExportAll { get; } = true;

        public override string GetSettingKey()
            => "Acc_WLMonitored_Player_WatchList_Sel";

        public override bool IsValid()
            => AgentId > 0;
    }
}
