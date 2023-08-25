namespace Alpha.BetLists.Criteria.SmartPunter
{
    using AlphaPlus.SmartPunter.Criteria;
    using Fanex.Data.Repository;

    public class SmartPunterSMACheckerCommand : NonQueryCommand
    {
        public SmartPunterSMACheckerCommand(SmartPunterSMAChecker commandData)
        {
            CustId = commandData.CustId;
            Username = commandData.Username;
        }

        public int? CustId { get; }

        public string Username { get; }

        public bool IsSmartPunter { get; set; }

        public bool HasDownlineSmartPunter { get; set; }

        public override string GetSettingKey() => "Age_CustomerSelCheckSmartPunterSMA";

        public override bool IsValid() => CustId.HasValue || Username != null;
    }
}