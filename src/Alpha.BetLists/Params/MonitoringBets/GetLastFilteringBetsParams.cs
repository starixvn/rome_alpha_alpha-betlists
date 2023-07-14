using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alpha.BetLists.Params.MonitoringBets
{
    public class GetLastFilteringBetsParams
    {

        public GetLastFilteringBetsParams(int custId, long transId, 
            string memberList, string sportTypeList, 
            string leagueList, string betTypeList,
            string liveIndicator,
            decimal minStake, int limitRowCount)
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
        
        public string LiveIndicator { get; }
        
        public decimal MinStake { get; }
        
        public int LimitRowCount { get; }
    }
}
