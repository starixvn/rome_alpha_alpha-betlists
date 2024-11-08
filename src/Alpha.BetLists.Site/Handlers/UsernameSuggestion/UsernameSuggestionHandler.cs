namespace Alpha.BetLists.Site.Handlers.UsernameSuggestion
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Services;
    using Alpha.BetLists.Site.Requests.UsernameSuggestion;
    using MediatR;

    public class UsernameSuggestionHandler :
        IAsyncRequestHandler<UsernameSuggestionQuery, IEnumerable<UsernameSuggestion>>,
        IAsyncRequestHandler<CheckUsernamesQuery, IEnumerable<UsernameErrCode>>
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

        public async Task<IEnumerable<UsernameErrCode>> Handle(CheckUsernamesQuery message)
        {
            var usernames = await usernameSuggestionService.CheckUsernames(message.ViewerId, message.ViewerLevelId, message.Usernames);

            if (message.FromSuggestion)
            {
                var validUsernames = usernames.Where(u => u.ErrCode == 0);
                foreach (var item in validUsernames)
                {
                    await usernameSuggestionService.UpdateUserSearch(
                        message.ViewerId,
                        item.Username,
                        item.FirstName,
                        item.LastName,
                        item.Username2);
                }
            }

            return usernames;
        }
    }
}