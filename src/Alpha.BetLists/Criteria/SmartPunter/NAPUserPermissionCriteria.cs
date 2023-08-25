namespace Alpha.BetLists.Criteria.SmartPunter
{
    using AlphaPlus.SmartPunter.Criteria;
    using Fanex.Data.Repository;

    public class NAPUserPermissionCriteria : CriteriaBase
    {
        public NAPUserPermissionCriteria(NAPUserPermission criteriaData)
        {
            CustId = criteriaData.CustId;
        }

        public int CustId { get; }

        public override string GetSettingKey() => "Acc_Con_FileNameSelByCustID";

        public override bool IsValid() => CustId > 0;
    }
}