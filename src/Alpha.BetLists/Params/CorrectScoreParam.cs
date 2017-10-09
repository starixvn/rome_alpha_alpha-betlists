namespace Alpha.BetLists.Params
{
    using Models;

    public class CorrectScoreParam
    {
        public CorrectScoreParam(
            int custId,
            string language,
            UserLevel custLevel)
        {
            CustId = custId;
            Language = language;
            CustLevel = custLevel;
        }

        public int CustId { get; }

        public string Language { get; }

        public UserLevel CustLevel { get; }
    }
}
