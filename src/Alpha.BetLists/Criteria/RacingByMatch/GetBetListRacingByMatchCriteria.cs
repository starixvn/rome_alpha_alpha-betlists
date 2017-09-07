namespace Alpha.BetLists.Criteria.RacingByMatch
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models;

    public class GetBetListRacingByMatchCriteria : CriteriaBase
    {
        public GetBetListRacingByMatchCriteria(
            int custId,
            int betType,
            int matchId,
            string language,
            UserLevel custLevel)
        {
            CustId = custId;
            BetType = betType;
            MatchId = matchId;
            Language = language;
            CustLevel = custLevel;
        }

        public int CustId { get; }

        public int BetType { get; }

        public int MatchId { get; }

        public string Language { get; }

        [SpParam(Ignored = true)]
        public UserLevel CustLevel { get; }

        public override string GetSettingKey()
            => CustLevel.IsSuperLevel()
            ? "Age_BetList_SelAllByDateHR_SAg"
            : CustLevel.IsMasterLevel()
            ? "Age_BetList_SelAllByDateHR_MAg"
            : "Age_BetList_SelAllByDateHR_Ag";

        public override bool IsValid()
            => CustId > 0 && BetType > 0;
    }
}
