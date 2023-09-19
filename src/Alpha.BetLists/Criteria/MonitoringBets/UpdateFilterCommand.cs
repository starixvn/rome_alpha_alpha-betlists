namespace Alpha.BetLists.Criteria.MonitoringBets
{
    using Alpha.BetLists.Params.MonitoringBets;

    public class UpdateFilterCommand : DeleteFilterCommand
    {
        public UpdateFilterCommand(UpdateFilterParam param) : base(param)
        {
            FilterName = param.FilterName;
            IsDefault = param.IsDefault;
        }

        public string FilterName { get; }

        public bool IsDefault { get; }

        public override string GetSettingKey()
            => "Age_CustLastBetFilter_Upd";

        public override bool IsValid()
            => base.IsValid() && !string.IsNullOrWhiteSpace(FilterName);
    }
}
