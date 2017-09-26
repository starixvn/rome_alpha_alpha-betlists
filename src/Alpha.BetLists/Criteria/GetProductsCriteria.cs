namespace Alpha.BetLists.Criteria
{
    using Fanex.Data.Repository;

    public class GetProductsCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Age_Rpt_GetProductList";

        public override bool IsValid() => true;
    }
}