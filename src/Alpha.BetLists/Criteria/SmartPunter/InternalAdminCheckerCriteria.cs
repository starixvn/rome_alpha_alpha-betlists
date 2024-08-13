namespace Alpha.BetLists.Criteria.SmartPunter
{
    using Fanex.Data.Repository;

    public class InternalAdminCheckerCommand : NonQueryCommand
    {
        public InternalAdminCheckerCommand(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public bool IsInternalAdmin { get; set; }

        public override string GetSettingKey() => "Acc_CustomerIsInternalAdmin";

        public override bool IsValid() => CustId > 0;
    }
}