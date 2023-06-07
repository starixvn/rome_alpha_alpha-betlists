namespace Alpha.BetLists.Criteria.IPInfo
{
    using Fanex.Data.Repository;

    public class GetIPV4InfoByTopCountryCriteria : CriteriaBase
    {
        public GetIPV4InfoByTopCountryCriteria(string countryCodes)
            => CountryCodes = countryCodes;

        public string CountryCodes { get; }

        public override string GetSettingKey()
            => "Age_IPRangesByCountrySel";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(CountryCodes);
    }
}
