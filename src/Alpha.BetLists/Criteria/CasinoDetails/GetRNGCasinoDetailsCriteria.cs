namespace Alpha.BetLists.Criteria.CasinoDetails
{
    using System;

    public class GetRNGCasinoDetailsCriteria : GetCasinoDetailsCriteria
    {
        public GetRNGCasinoDetailsCriteria(long refNo, int custId, byte transType, int productId, int pageIndex, int pageSize, DateTime moveBetsDate, DateTime balanceUpDate)
            : base(refNo, custId, transType, productId, pageIndex, pageSize, moveBetsDate, balanceUpDate)
        {
        }

        public override string GetSettingKey()
            => "Acc_BetList_RNG_Detail";
    }
}
