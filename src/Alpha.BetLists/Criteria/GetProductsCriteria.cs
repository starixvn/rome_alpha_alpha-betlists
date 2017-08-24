using Fanex.Data.Repository;

namespace Alpha.BetLists.Criteria
{
    public class GetProductsCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Age_Rpt_GetProductList";

        public override bool IsValid() => true;
    }
}