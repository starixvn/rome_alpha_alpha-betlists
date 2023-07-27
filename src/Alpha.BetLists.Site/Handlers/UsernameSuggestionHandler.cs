namespace Alpha.BetLists.Site.Handlers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Services;
    using Alpha.BetLists.Site.Requests;
    using MediatR;

    public class UsernameSuggestionHandler : IAsyncRequestHandler<UsernameSuggestionQuery, IEnumerable<UsernameSuggestion>>
    {
        private readonly UsernameSuggestionService usernameSuggestionService;

        public UsernameSuggestionHandler(UsernameSuggestionService usernameSuggestionService)
        {
            this.usernameSuggestionService = usernameSuggestionService;
        }

        public Task<IEnumerable<UsernameSuggestion>> Handle(UsernameSuggestionQuery message)
            => usernameSuggestionService.GetUsernamesSuggestion(
                message.ViewerId,
                message.SuggestionQuery,
                message.IsInternal,
                message.DirectDownlines,
                message.IsAccInfo);
    }
}