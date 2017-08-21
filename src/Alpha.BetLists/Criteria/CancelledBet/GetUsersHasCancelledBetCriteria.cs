namespace Alpha.BetLists.Criteria.CancelledBet
{
    using Alpha.BetLists.Models;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetUsersHasCancelledBetCriteria : CriteriaBase
    {
        public GetUsersHasCancelledBetCriteria(int custId, UserLevel custLevel)
        {
            CustId = custId;
            CustLevel = custLevel;
        }

        [SpParam(SqlParamName = "CustId")]
        public int CustId { get; }

        [SpParam(Ignored = true)]
        public UserLevel CustLevel { get; }
        
        public override string GetSettingKey() =>
           CustLevel.IsSuperLevel()
           ? "Age_BetList_SelDeleted_UserList_SAg"
           : CustLevel.IsMasterLevel()
           ? "Age_BetList_SelDeleted_UserList_MAg"
           : "Age_BetList_SelDeleted_UserList_Ag";

        public override bool IsValid() => CustId > 0;
    }
}
