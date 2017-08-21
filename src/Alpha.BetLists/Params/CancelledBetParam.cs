namespace Alpha.BetLists.Params
{
    using Alpha.BetLists.Models;

    public class CancelledBetParam
    {
        public CancelledBetParam(
            UserLevel level,
            int currentUserId,
            int masterId,
            int agentId,
            int memberId,
            string language)
        {
            Level = level;
            CurrentUserId = currentUserId;
            MasterId = masterId;
            AgentId = agentId;
            MemberId = memberId;
            Language = language;
        }

        public UserLevel Level { get; }

        public int CurrentUserId { get; }

        public int MasterId { get; }

        public int AgentId { get; }

        public int MemberId { get; }

        public string Language { get; }
    }
}
