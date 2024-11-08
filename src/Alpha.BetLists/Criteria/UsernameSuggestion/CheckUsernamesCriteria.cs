namespace Alpha.BetLists.Criteria.UsernameSuggestion
{
    using Fanex.Data.Repository;

    public class CheckUsernamesCriteria : CriteriaBase
    {
        public CheckUsernamesCriteria(int agentId, byte roleId, string usernames)
        {
            AgentId = agentId;
            RoleId = roleId;
            Usernames = usernames;
        }

        public int AgentId { get; }

        public byte RoleId { get; }

        public string Usernames { get; }

        public override string GetSettingKey()
            => "Age_CustLastBetFilter_Check";

        public override bool IsValid()
            => AgentId > 0 && !string.IsNullOrWhiteSpace(Usernames);
    }
}
