namespace Alpha.BetLists.Params.MonitoringBets
{
    using System;

    [Serializable]
    public class UpdateFilterParam
    {
        public UpdateFilterParam(
            int custId,
            int subAccId,
            int filterId,
            string filterName,
            bool isDefault,
            string languageKey,
            string ip)
        {
            CustId = custId;
            SubAccId = subAccId;
            FilterId = filterId;
            FilterName = filterName;
            IsDefault = isDefault;
            LanguageKey = languageKey;
            IP = ip;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public int FilterId { get; }

        public string FilterName { get; }

        public bool IsDefault { get; }

        public string LanguageKey { get; }

        public string IP { get; }
    }
}
