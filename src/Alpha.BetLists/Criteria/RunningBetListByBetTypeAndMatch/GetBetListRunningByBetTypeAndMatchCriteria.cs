namespace Alpha.BetLists.Criteria.RunningBetListByBetTypeAndMatch
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models;

    public class GetBetListRunningByBetTypeAndMatchCriteria : CriteriaBase
    {
        public GetBetListRunningByBetTypeAndMatchCriteria(
            int matchId,
            int custId,
            int betType,
            string language,
            int liveIndicator,
            string betTeam,
            UserLevel custLevel)
        {
            MatchId = matchId;
            CustId = custId;
            BetType = betType;
            Language = language;
            LiveIndicator = liveIndicator;
            BetTeam = betTeam;
            CustLevel = custLevel;
        }

        public int MatchId { get; }

        public int CustId { get; }

        public int BetType { get; }

        public string Language { get; }

        public int LiveIndicator { get; }

        public string BetTeam { get; }

        [SpParam(Ignored = true)]
        public UserLevel CustLevel { get; }

        public override string GetSettingKey()
            => CustLevel.IsAgentLevel()
            ? "Age_BetList_SelRunningByBettypeMatch_Ag"
            : CustLevel.IsMasterLevel()
            ? "Age_BetList_SelRunningByBettypeMatch_MAg"
            : "Age_BetList_SelRunningByBettypeMatch_SAg";

        public override bool IsValid() => CustId > 0 && BetType > 0;
    }
}
