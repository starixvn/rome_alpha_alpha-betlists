namespace Alpha.BetLists.Criteria.ProgressiveJackpot
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetEnableJackpotProductCriteria : CriteriaBase
    {
        public GetEnableJackpotProductCriteria(int custId, bool includeTestData)
        {
            CustId = custId;
            IncludeTestData = includeTestData;
        }

        public int CustId { get; }

        [SpParam(SqlParamName = "IsTest")]
        public bool IncludeTestData { get; }

        public override string GetSettingKey() => "Acc_GameEnableJackpotSel";

        public override bool IsValid() => CustId > 0;
    }
}