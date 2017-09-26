namespace Alpha.BetLists.Criteria
{
    using Fanex.Data.Repository;

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
