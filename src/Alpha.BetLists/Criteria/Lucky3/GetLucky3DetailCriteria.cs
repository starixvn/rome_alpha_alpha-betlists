namespace Alpha.BetLists.Criteria.Lucky3
{
    using Fanex.BetListDetail.Lucky3.Criteria;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetLucky3DetailCriteria : CriteriaBase
    {
        public GetLucky3DetailCriteria(IGetLucky3DetailParameter commandData)
        {
            criteria = commandData;
        }

        [SpParam(Ignored = true)]
        private IGetLucky3DetailParameter criteria { get; set; }

        public long CustId
        {
            get
            {
                return criteria.CustomerId;
            }
        }

        public long TransId
        {
            get
            {
                return criteria.TransactionId;
            }
        }

        public override string GetSettingKey() => "Acc_BetList_Lucky3_Detail";

        public override bool IsValid() => criteria.IsValid();
    }
}