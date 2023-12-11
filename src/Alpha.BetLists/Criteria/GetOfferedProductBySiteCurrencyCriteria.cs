namespace Alpha.BetLists.Criteria
{
    using Alpha.BetLists.Configurations;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetOfferedProductBySiteCurrencyCriteria : CriteriaBase
    {
        public string Site { get; set; }

        [SpParam(SqlParamName = "Currency")]
        public int CurrencyId { get; set; }
        public byte SettingStatus => AppSettings.SettingStatus;

        public override string GetSettingKey() => "Age_GetOfferedProductBySiteCurrency";
        public override bool IsValid() => CurrencyId > 0 && !string.IsNullOrWhiteSpace(Site);
    }
}
