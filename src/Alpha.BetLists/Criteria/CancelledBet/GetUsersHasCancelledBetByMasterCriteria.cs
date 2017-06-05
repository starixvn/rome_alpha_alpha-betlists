using Fanex.Data.Repository;

namespace Alpha.BetLists.Criteria.CancelledBet
{
    public class GetUsersHasCancelledBetByMasterCriteria : CriteriaBase
    {
        public GetUsersHasCancelledBetByMasterCriteria(int masterId)
        {
            MasterId = masterId;
        }

        public int MasterId { get; }

        public override string GetSettingKey() => "Age_BetList_SelDeleted_UserList_MAg";

        public override bool IsValid() => MasterId > 0;
    }
}
