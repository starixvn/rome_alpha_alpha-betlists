namespace Alpha.BetLists.Criteria.MonitoringBets
{
    using Fanex.Data.Repository;

    public class GetFiltersCriteria : CriteriaBase
    {
        public GetFiltersCriteria(int custId, int subAccId)
        {
            CustId = custId;
            SubAccId = subAccId;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public override string GetSettingKey()
            => "Age_CustLastBetFilter_Sel";

        public override bool IsValid()
            => CustId > 0 && SubAccId >= 0;
    }
}
