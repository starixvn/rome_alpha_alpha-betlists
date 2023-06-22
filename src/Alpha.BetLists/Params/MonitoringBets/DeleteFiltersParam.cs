namespace Alpha.BetLists.Params.MonitoringBets
{
    public class DeleteFiltersParam
    {
        public DeleteFiltersParam(int custId, int subAccId, int[] filterIds, string ip)
        {
            CustId = custId;
            SubAccId = subAccId;
            FilterIds = filterIds;
            IP = ip;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public int[] FilterIds { get; }

        public string IP { get; }
    }
}
