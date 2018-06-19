namespace Alpha.BetLists.Criteria.LastBets
{
    using Fanex.Data.Repository;

    public class GetMatchInfoByIdCriteria : CriteriaBase
    {
        public GetMatchInfoByIdCriteria(int matchId)
        {
            MatchId = matchId;
        }

        public int MatchId { get; }

        public override string GetSettingKey() => "Age_Monitor_MatchSelByID";

        public override bool IsValid() => true;
    }
}
