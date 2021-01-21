namespace Alpha.BetLists.Criteria
{
    using Alpha.BetLists.Configurations;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetOfferedProductsBySiteCriteria : CriteriaBase
    {
        public GetOfferedProductsBySiteCriteria(int custId, string site, int currencyId)
        {
            CustId = custId;
            Site = site;
            CurrencyId = currencyId;
            SettingStatus = AppSettings.SettingStatus;
        }

        public byte SettingStatus { get; }

        public int CustId { get; }

        public string Site { get; }

        [SpParam(SqlParamName = "Currency")]
        public int CurrencyId { get; }

        public override string GetSettingKey() => "Acc_CustSetting_CheckMacauEnable";

        public override bool IsValid() => !string.IsNullOrEmpty(Site) && CurrencyId > 0 && CustId > 0;
    }
}