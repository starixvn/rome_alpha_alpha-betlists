using Alpha.BetLists.Models;

namespace Alpha.BetLists.Params
{
    public class RunningBetListByBetTypeParam
    {
        public RunningBetListByBetTypeParam(
            int custId,
            int betType,
            string language,
            UserLevel custLevel)
        {
            CustId = custId;
            BetType = betType;
            Language = language;
            CustLevel = custLevel;
        }

        public int CustId { get; }

        public int BetType { get; }

        public string Language { get; }

        public UserLevel CustLevel { get; }
    }
}
