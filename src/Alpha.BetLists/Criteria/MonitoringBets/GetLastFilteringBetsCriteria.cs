namespace Alpha.BetLists.Criteria.MonitoringBets
{
    using Fanex.Data.Repository;

    public class GetLastFilteringBetsCriteria : CriteriaBase
    {
        public GetLastFilteringBetsCriteria(
            int custId,
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
            TransId = transId;
            MemberList = memberList;
            BetTypeList = betTypeList;
            SportTypeList = sportTypeList;
            LeagueList = leagueList;
            LiveIndicator = liveIndicator;
            MinStake = minStake;
            LimitRowCount = limitRowCount;
        }

        public int CustId { get; }
        
        public long TransId { get; }
        
        public string MemberList { get; }
        
        public string BetTypeList { get; }
        
        public string SportTypeList { get; }
        
        public string LeagueList { get; }

        public byte LiveIndicator { get; }

        public decimal MinStake { get; }
        
        public int LimitRowCount { get; }

        public override string GetSettingKey()
            => "Age_BetList_LastBets_Ag";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(BetTypeList)
            && !string.IsNullOrWhiteSpace(SportTypeList)
            && !string.IsNullOrWhiteSpace(LeagueList)
            && MinStake >= 0
            && LimitRowCount > 0;
    }
}
