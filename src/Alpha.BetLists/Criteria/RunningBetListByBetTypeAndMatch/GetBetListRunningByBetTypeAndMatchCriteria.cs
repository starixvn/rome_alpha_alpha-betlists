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
            int liveIndicator,
            string betTeam,
            UserLevel custLevel)
        {
            MatchId = matchId;
            CustId = custId;
            BetType = betType;
            LiveIndicator = liveIndicator;
            BetTeam = betTeam;
            CustLevel = custLevel;
        }

        public int MatchId { get; }

        public int CustId { get; }

        public int BetType { get; }

        public int LiveIndicator { get; }

        public string BetTeam { get; }

        [SpParam(Ignored = true)]
        public UserLevel CustLevel { get; }

        public int RoleId 
            => CustLevel.IsSuperLevel() ? 4 : CustLevel.IsMasterLevel() ? 3 : 2;

        public override string GetSettingKey()
            => "Ost_BetList_TotalBet";

        public override bool IsValid() => CustId > 0 && BetType > 0;
    }
}
