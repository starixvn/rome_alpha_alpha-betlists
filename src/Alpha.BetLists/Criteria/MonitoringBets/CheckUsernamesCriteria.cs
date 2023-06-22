namespace Alpha.BetLists.Criteria.MonitoringBets
{
    using Fanex.Data.Repository;

    public class CheckUsernamesCriteria : CriteriaBase
    {
        public CheckUsernamesCriteria(int agentId, string usernames)
        {
            AgentId = agentId;
            Usernames = usernames;
        }

        public int AgentId { get; }

        public string Usernames { get; }

        public override string GetSettingKey()
            => "Age_CustLastBetFilter_Check";

        public override bool IsValid()
            => AgentId > 0 && !string.IsNullOrWhiteSpace(Usernames);
    }
}
