namespace Alpha.BetLists.Params.MonitoringBets
{
    using System;

    [Serializable]
    public class UpdateFilterParam : DeleteFilterParam
    {
        public UpdateFilterParam(
            int custId,
            int subAccId,
            int filterId,
            string filterName,
            bool isDefault,
            string languageKey,
            string ip) : base(custId, subAccId, filterId, ip)
        {
            FilterName = filterName;
            IsDefault = isDefault;
            LanguageKey = languageKey;
        }


        public string FilterName { get; }

        public bool IsDefault { get; }

        public string LanguageKey { get; }
    }
}
