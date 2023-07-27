namespace Alpha.BetLists.Criteria.UsernameSuggestion
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetUsernameSuggestionCriteria : CriteriaBase
    {
        public GetUsernameSuggestionCriteria(
            int actorId,
            string suggestionQuery,
            bool isInternal,
            bool directDownline,
            bool isAccInfo)
        {
            ActorId = actorId;
            SuggestionQuery = suggestionQuery;
            IsInternal = isInternal;
            DirectDownline = directDownline;
            IsAccInfo = isAccInfo;
        }

        public int UserId => ActorId;

        [SpParam(SqlParamName = "CustId")]
        public int ActorId { get; }

        [SpParam(SqlParamName = "username")]
        public string SuggestionQuery { get; }

        [SpParam(SqlParamName = "isInternalAgent")]
        public bool IsInternal { get; }

        public bool DirectDownline { get; }

        public bool IsAccInfo { get; }

        public override string GetSettingKey() => "Age_CustomerSelUserSearch";

        public override bool IsValid() => ActorId > 0 && !string.IsNullOrEmpty(SuggestionQuery);
    }
}
