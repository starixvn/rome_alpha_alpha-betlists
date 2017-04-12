namespace Alpha.BetLists.Criteria
{
    using Fanex.Data.Repository;

    public class GetIPVNCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "GetIPVN";

        public override bool IsValid() => true;
    }
}