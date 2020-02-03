namespace Alpha.BetLists.Params
{
    using Extensions;
    using System;

    public class RiskAnalysisIPMatchingParam
    {
        public RiskAnalysisIPMatchingParam(
            int custId,
            DateTime date,
            DateTime moveBetsDay,
            string ip)
        {
            CustId = custId;
            Date = date.ToStartOfDay();
            MoveBetsDay = moveBetsDay.ToStartOfDay();
            IP = ip;
        }

        public int CustId { get; }

        public DateTime Date { get; }

        public DateTime MoveBetsDay { get; }

        public string IP { get; }
    }
}