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
            string ip,
            bool isNotify) : base(custId, subAccId, filterId, languageKey, ip)
        {
            FilterName = filterName;
            IsDefault = isDefault;
            IsNotify = isNotify;
        }

        public string FilterName { get; }

        public bool IsDefault { get; }

        public bool IsNotify { get; }
    }
}
