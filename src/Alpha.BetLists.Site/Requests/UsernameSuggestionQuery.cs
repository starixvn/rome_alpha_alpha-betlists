namespace Alpha.BetLists.Site.Requests
{
    using System.Collections.Generic;
    using Alpha.BetLists.Models;
    using MediatR;

    public class UsernameSuggestionQuery : BaseQuery, IRequest<IEnumerable<UsernameSuggestion>>
    {
        public UsernameSuggestionQuery(IUser viewer) : base(viewer)
        {
            IsInternal = viewer.IsInternal;
        }

        public string SuggestionQuery { get; set; }

        public bool DirectDownlines { get; set; }

        public bool IsAccInfo { get; set; } = true;

        public bool IsInternal { get; }

        public bool IsValid() => !string.IsNullOrWhiteSpace(SuggestionQuery);
    }
}