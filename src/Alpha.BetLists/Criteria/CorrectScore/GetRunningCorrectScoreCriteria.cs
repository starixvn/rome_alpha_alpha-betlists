namespace Alpha.BetLists.Criteria.CorrectScore
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models;

    public class GetRunningCorrectScoreCriteria : CriteriaBase
    {
        public GetRunningCorrectScoreCriteria(
            int custId,
            string language,
            UserLevel custLevel)
        {
            CustId = custId;
            Language = language;
            CustLevel = custLevel;
        }

        public int CustId { get; }

        public string Language { get; }

        [SpParam(Ignored = true)]
        public UserLevel CustLevel { get; }

        public override string GetSettingKey()
            => CustLevel.IsSuperLevel()
            ? "Age_TotalBets_Sel430_SAg"
            : CustLevel.IsMasterLevel()
            ? "Age_TotalBets_Sel430_MAg"
            : "Age_TotalBets_Sel430_Ag";

        public override bool IsValid() => CustId > 0;
    }
}
