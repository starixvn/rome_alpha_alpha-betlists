namespace Alpha.BetLists.Criteria.SmartPunter
{
    using Fanex.Data.Repository;

    public class NAPUserPermissionCriteria : CriteriaBase
    {
        public NAPUserPermissionCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public override string GetSettingKey() => "Acc_Con_FileNameSelByCustID";

        public override bool IsValid() => CustId > 0;
    }
}