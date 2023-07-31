namespace Alpha.BetLists.Params.MonitoringBets
{
    using System;

    [Serializable]
    public class DeleteFilterParam
    {
        public DeleteFilterParam(int custId, int subAccId, int filterId, string ip)
        {
            CustId = custId;
            SubAccId = subAccId;
            FilterId = filterId;
            IP = ip;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public int FilterId { get; }

        public string IP { get; }
    }
}
