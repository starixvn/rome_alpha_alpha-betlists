namespace Alpha.BetLists.Criteria.RunningBetListByBetType
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models;

    public class GetBetListRunningByBetTypeCriteria : CriteriaBase
    {
        public GetBetListRunningByBetTypeCriteria(
            int custId,
            int betType,
            UserLevel custLevel)
        {
            CustId = custId;
            BetType = betType;
            CustLevel = custLevel;
        }

        public int CustId { get; }

        public int BetType { get; }

        [SpParam(Ignored = true)]
        public UserLevel CustLevel { get; }

        public override string GetSettingKey()
            => CustLevel.IsAgentLevel()
            ? "Age_BetList_SelRunningByBettype_Ag"
            : CustLevel.IsMasterLevel()
            ? "Age_BetList_SelRunningByBettype_MAg"
            : "Age_BetList_SelRunningByBettype_SAg";

        public override bool IsValid() => CustId > 0 && BetType > 0;
    }
}
