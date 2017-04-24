using Alpha.BetLists.Models;
using Fanex.Data;
using Fanex.Data.Repository;

namespace Alpha.BetLists.Criteria.WinLossByMatch
{
    public class GetWinLossOthersCriteria : CriteriaBase
    {
        public GetWinLossOthersCriteria(
             int custId,
             int matchId,
             int betType,
             string language,
             string custName,
             bool liveIndicator,
             UserLevel custLevel)
        {
            CustId = custId;
            MatchId = matchId;
            BetType = betType;

            Language = language;
            CustName = custName;
            LiveIndicator = liveIndicator;
            CustLevel = custLevel;
        }

        public int CustId { get; }

        public int MatchId { get; }

        public int BetType { get; }

        public string Language { get; }

        [SpParam(SqlParamName = "Username")]
        public string CustName { get; }

        public bool LiveIndicator { get; }

        [SpParam(Ignored = true)]
        public UserLevel CustLevel { get; }

        public override string GetSettingKey() =>
            CustLevel.IsSuperLevel()
            ? "Age_BetList_SelWinlossByBettypeMatch_SAg"
            : CustLevel.IsMasterLevel()
            ? "Age_BetList_SelWinlossByBettypeMatch_MAg"
            : "Age_BetList_SelWinlossByBettypeMatch_Ag";

        public override bool IsValid() => CustId > 0 && !string.IsNullOrWhiteSpace(Language);
    }
}
