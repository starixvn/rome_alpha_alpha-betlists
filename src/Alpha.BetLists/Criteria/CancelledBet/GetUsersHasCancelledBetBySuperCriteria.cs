using Fanex.Data;
using Fanex.Data.Repository;

namespace Alpha.BetLists.Criteria.CancelledBet
{
    public class GetUsersHasCancelledBetBySuperCriteria : CriteriaBase
    {
        public GetUsersHasCancelledBetBySuperCriteria(int superId)
        {
            SuperId = superId;
        }

        [SpParam(SqlParamName = "CustId")]
        public int SuperId { get; set; }

        public override string GetSettingKey() => "Age_BetList_SelDeleted_UserList_SAg";

        public override bool IsValid() => SuperId > 0;
    }
}
