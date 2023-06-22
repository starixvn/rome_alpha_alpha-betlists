namespace Alpha.BetLists.Criteria.MonitoringBets
{
    using Alpha.BetLists.Params.MonitoringBets;

    public class UpdateFilterCommand : DeleteFilterCommand
    {
        public UpdateFilterCommand(UpdateFilterParam param)
            : base(param.CustId, param.SubAccId, param.FilterId, param.IP)
        {
            FilterName = param.FilterName;
            IsDefault = param.IsDefault;
            Language = param.LanguageKey;
        }

        public string FilterName { get; }

        public bool IsDefault { get; }

        public string Language { get; }

        public override string GetSettingKey()
            => "Age_CustLastBetFilter_Upd";

        public override bool IsValid()
            => base.IsValid() && !string.IsNullOrWhiteSpace(FilterName);
    }
}
