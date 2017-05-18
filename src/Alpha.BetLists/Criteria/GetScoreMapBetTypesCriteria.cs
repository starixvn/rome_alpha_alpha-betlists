namespace Alpha.BetLists.Criteria
{
    using Fanex.Data.Repository;

    public class GetScoreMapBetTypesCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Age_ScoreMap_SelBettype";

        public override bool IsValid() => true;
    }
}