using Fanex.Data.Repository;

namespace Alpha.BetLists.Criteria.CancelledBet
{
    public class GetUsersHasCancelledBetByAgentCriteria : CriteriaBase
    {
        public GetUsersHasCancelledBetByAgentCriteria(int agentId)
        {
            AgentId = agentId;
        }

        public int AgentId { get; }

        public override string GetSettingKey() => "Age_BetList_SelDeleted_UserList_Ag";

        public override bool IsValid() => AgentId > 0;
    }
}
