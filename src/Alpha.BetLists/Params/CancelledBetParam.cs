using Alpha.BetLists.Models;

namespace Alpha.BetLists.Params
{
    public class CancelledBetParam
    {
        public CancelledBetParam(
            UserLevel level,
            int currentUserId,
            int masterId,
            int agentId,
            int memberId,
            string language
            )
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

        public int MasterId { get; set; }

        public int AgentId { get; set; }

        public int MemberId { get; set; }

        public string Language { get; set; }
    }
}
