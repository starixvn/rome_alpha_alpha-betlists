using Alpha.BetLists.Models;

namespace Alpha.BetLists.Params.MonitoringBets
{
    public class GetLastFilteringBetsParams
    {
        public GetLastFilteringBetsParams(
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
            SportTypeList = sportTypeList;
            LeagueList = leagueList;
            LiveIndicator = liveIndicator;
            MinStake = minStake;
            LimitRowCount = limitRowCount;
        }
        
        public int CustId { get; }

        public UserLevel UserLevel { get; }

        public long TransId { get; }
        
        public string MemberList { get; }
        
        public string BetTypeList { get; }
        
        public string SportTypeList { get; }
        
        public string LeagueList { get; }
        
        public byte LiveIndicator { get; }
        
        public decimal MinStake { get; }
        
        public int LimitRowCount { get; }
    }
}
