namespace Alpha.BetLists.Criteria.RiskAnalysisIPMatching
{
    using System;
    using Fanex.Data.Repository;

    public class GetRiskAnalysisIPMatchingCriteria : CriteriaBase
    {
        public GetRiskAnalysisIPMatchingCriteria(
            int custId,
            string ip,
            DateTime date,
            DateTime moveBetsDay)
        {
            CustId = custId;
            Date = date;
            MoveBetsDay = moveBetsDay;
            IP = ip;
        }

        public int CustId { get; }

        public DateTime Date { get; }

        public DateTime MoveBetsDay { get; }

        public string IP { get; }

        public override string GetSettingKey() => "Age_BetList_IPMatching";

        public override bool IsValid() =>
            CustId > 0
            && !string.IsNullOrWhiteSpace(IP)
            && Date > DateTime.MinValue;
    }
}