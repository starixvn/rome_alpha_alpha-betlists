using Fanex.Data;
using Fanex.Data.Repository;

namespace Alpha.BetLists.Criteria.LastBets
{
    public class GetRunningOutrightMatchCriteria : CriteriaBase
    {
        public GetRunningOutrightMatchCriteria(bool isVirtualSportsRacing)
        {
            IsVirtualSportsRacing = isVirtualSportsRacing;
        }

        [SpParam(SqlParamName = "isvsracing")]
        public bool IsVirtualSportsRacing { get; }

        public override string GetSettingKey() => "Age_Monitor_OutrightSel";

        public override bool IsValid() => true;
    }
}
