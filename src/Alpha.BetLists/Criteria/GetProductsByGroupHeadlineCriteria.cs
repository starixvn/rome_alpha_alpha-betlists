namespace Alpha.BetLists.Criteria
{
    using Alpha.BetLists.Configurations;
    using Fanex.Data.Repository;

    public class GetProductsByGroupHeadlineCriteria : CriteriaBase
    {
        public GetProductsByGroupHeadlineCriteria(int custId)
        {
            CustId = custId;
            SettingStatus = AppSettings.SettingStatus;
        }

        public int CustId { get; }

        public byte SettingStatus { get; }

        public override string GetSettingKey()
            => "Age_Rpt_ConsolidatedGroupProductSel";

        public override bool IsValid()
            => CustId > 0;
    }
}
