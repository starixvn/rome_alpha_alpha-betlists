namespace Alpha.BetLists.Criteria.CancelledBet
{
    using Fanex.Data.Repository;

    public class GetCancelledBetCriteria : CriteriaBase
    {
        public GetCancelledBetCriteria(int custId, int roleId, string language)
        {
            CustId = custId;
            Language = language;
            RoleId = roleId;
        }

        public int CustId { get; }

        public int RoleId { get; }

        public string Language { get; }

        public override string GetSettingKey() => "Age_BetList_SelDeleted";

        public override bool IsValid() => CustId > 0;
    }
}
