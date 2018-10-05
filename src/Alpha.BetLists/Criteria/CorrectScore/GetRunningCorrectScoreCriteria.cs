namespace Alpha.BetLists.Criteria.CorrectScore
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models;

    public class GetRunningCorrectScoreCriteria : CriteriaBase
    {
        public GetRunningCorrectScoreCriteria(
            int custId,
            UserLevel custLevel)
        {
            CustId = custId;
            CustLevel = custLevel;
        }

        public int CustId { get; }

        [SpParam(Ignored = true)]
        public UserLevel CustLevel { get; }

        public override string GetSettingKey()
            => CustLevel.IsAgentLevel()
            ? "Age_TotalBets_Sel430_Ag"
            : CustLevel.IsMasterLevel()
            ? "Age_TotalBets_Sel430_MAg"
            : "Age_TotalBets_Sel430_SAg";

        public override bool IsValid() => CustId > 0;
    }
}
