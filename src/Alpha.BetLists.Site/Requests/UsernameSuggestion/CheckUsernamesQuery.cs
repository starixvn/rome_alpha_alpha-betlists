namespace Alpha.BetLists.Site.Requests.UsernameSuggestion
{
    using System.Collections.Generic;
    using Alpha.BetLists.Models;
    using MediatR;

    public class CheckUsernamesQuery : BaseQuery, IRequest<IEnumerable<UsernameErrCode>>
    {
        public CheckUsernamesQuery(IUser viewer) : base(viewer)
        {
        }

        public string Usernames { get; set; }

        public bool FromSuggestion { get; set; }
    }
}