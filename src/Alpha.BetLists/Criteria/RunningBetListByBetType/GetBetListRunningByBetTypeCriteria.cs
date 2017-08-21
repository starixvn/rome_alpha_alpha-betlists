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
            string language,
            UserLevel custLevel)
        {
            CustId = custId;
            BetType = betType;
            Language = language;
            CustLevel = custLevel;
        }

        public int CustId { get; }

        public int BetType { get; }

        public string Language { get; }

        [SpParam(Ignored = true)]
        public UserLevel CustLevel { get; }

        public override string GetSettingKey()
            => CustLevel.IsSuperLevel()
            ? "Age_BetList_SelRunningByBettype_SAg"
            : CustLevel.IsMasterLevel()
            ? "Age_BetList_SelRunningByBettype_MAg"
            : "Age_BetList_SelRunningByBettype_Ag";

        public override bool IsValid() => CustId > 0 && BetType > 0;
    }
}
