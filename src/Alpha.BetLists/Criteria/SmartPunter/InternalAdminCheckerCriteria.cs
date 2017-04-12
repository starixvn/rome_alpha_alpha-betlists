namespace Alpha.BetLists.Criteria.SmartPunter
{
    using Alpha.SmartPunter.Criteria;
    using Fanex.Data.Repository;

    public class InternalAdminCheckerCommand : NonQueryCommand
    {
        public InternalAdminCheckerCommand(InternalAdminChecker commandData)
        {
            CustId = commandData.CustId;
        }

        public int CustId { get; }

        public bool IsInternalAdmin { get; set; }

        public override string GetSettingKey() => "Acc_CustomerIsInternalAdmin";

        public override bool IsValid() => CustId > 0;
    }
}