using Alpha.BetLists.Models;
using Fanex.Data;
using Fanex.Data.Repository;

namespace Alpha.BetLists.Criteria.CancelledBet
{
    public class GetCancelledBetCriteria : CriteriaBase
    {
        public GetCancelledBetCriteria(int custId, UserLevel userLevel,string language)
        {
            CustId = custId;            
            Language = language;
            Level = userLevel;
        }

        public int CustId { get; }

        public int RoleId => Level.Value;

        [SpParam(Ignored = true)]
        private UserLevel Level { get; }

        public string Language { get; }

        public override string GetSettingKey() => "Age_BetList_SelDeleted";

        public override bool IsValid() => CustId > 0 && !Level.IsMemberLevel();

    }
}
