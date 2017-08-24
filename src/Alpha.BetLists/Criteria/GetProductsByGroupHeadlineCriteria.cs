using Fanex.Data.Repository;

namespace Alpha.BetLists.Criteria
{
    public class GetProductsByGroupHeadlineCriteria : CriteriaBase
    {
        public GetProductsByGroupHeadlineCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public override string GetSettingKey()
            => "Age_Rpt_ConsolidatedGroupProductSel";

        public override bool IsValid()
            => CustId > 0;
    }
}
