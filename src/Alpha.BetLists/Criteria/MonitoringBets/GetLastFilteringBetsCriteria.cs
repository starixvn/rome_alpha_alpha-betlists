namespace Alpha.BetLists.Criteria.MonitoringBets
{
    using Alpha.BetLists.Models;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetLastFilteringBetsCriteria : CriteriaBase
    {
        public GetLastFilteringBetsCriteria(
            int custId,
            UserLevel userLevel,
            long transId, 
            string memberList,
            string sportTypeList, 
            string leagueList,
            string betTypeList,
            byte liveIndicator,
            decimal minStake,
            int limitRowCount)
        {
            CustId = custId;
            UserLevel = userLevel;
            TransId = transId;
            MemberList = memberList;
            BetTypeList = betTypeList;
            SportTypeList = sportTypeList ?? string.Empty;
            LeagueList = leagueList ?? string.Empty;
            LiveIndicator = liveIndicator;
            MinStake = minStake;
            LimitRowCount = limitRowCount;
        }

        public int CustId { get; }

        [SpParam(Ignored = true)]
        public UserLevel UserLevel { get; }

        public long TransId { get; }
        
        public string MemberList { get; }
        
        public string BetTypeList { get; }
        
        public string SportTypeList { get; }
        
        public string LeagueList { get; }

        public byte LiveIndicator { get; }

        public decimal MinStake { get; }
        
        public int LimitRowCount { get; }

        public override string GetSettingKey()
        {
            if (UserLevel.IsSuperLevel())
            {
                return "Age_BetList_LastBets_SAg";
            }

            if (UserLevel.IsMasterLevel())
            {
                return "Age_BetList_LastBets_MAg";
            }

            return "Age_BetList_LastBets_Ag";
        }

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(BetTypeList)
            && MinStake >= 0
            && LimitRowCount > 0;
    }
}
