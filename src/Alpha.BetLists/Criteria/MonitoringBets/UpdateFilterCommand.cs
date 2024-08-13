namespace Alpha.BetLists.Criteria.MonitoringBets
{
    public class UpdateFilterCommand : DeleteFilterCommand
    {
        public UpdateFilterCommand(
            int custId,
            int subAccId,
            int filterId,
            string filterName,
            bool isDefault,
            string language,
            string ip,
            bool isNotify) : base(custId, subAccId, filterId, language, ip)
        {
            FilterName = filterName;
            IsDefault = isDefault;
            IsNotify = isNotify;
        }

        public string FilterName { get; }

        public bool IsDefault { get; }

        public bool IsNotify { get; }

        public override string GetSettingKey()
            => "Age_CustLastBetFilter_Upd";

        public override bool IsValid()
            => base.IsValid() && !string.IsNullOrWhiteSpace(FilterName);
    }
}
