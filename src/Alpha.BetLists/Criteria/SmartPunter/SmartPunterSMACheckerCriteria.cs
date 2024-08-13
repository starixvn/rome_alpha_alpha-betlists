namespace Alpha.BetLists.Criteria.SmartPunter
{
    using Fanex.Data.Repository;

    public class SmartPunterSMACheckerCommand : NonQueryCommand
    {
        public SmartPunterSMACheckerCommand(int? custId, string username)
        {
            CustId = custId;
            Username = username;
        }

        public int? CustId { get; }

        public string Username { get; }

        public bool IsSmartPunter { get; set; }

        public bool HasDownlineSmartPunter { get; set; }

        public override string GetSettingKey() => "Age_CustomerSelCheckSmartPunterSMA";

        public override bool IsValid() => CustId.HasValue || Username != null;
    }
}