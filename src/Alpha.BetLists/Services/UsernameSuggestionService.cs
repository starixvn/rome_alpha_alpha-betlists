namespace Alpha.BetLists.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.BetLists.Criteria.UsernameSuggestion;
    using Alpha.BetLists.Models;
    using Fanex.Data.Repository;

    public interface UsernameSuggestionService
    {
        Task<IEnumerable<UsernameSuggestion>> GetUsernamesSuggestion(
            int actorId,
            string suggestionQuery,
            bool isInternal,
            bool directDownline,
            bool isAccInfo);

        Task UpdateUserSearch(int userId, string search, string firstName, string lastName, string nickname);

        Task<IEnumerable<UsernameErrCode>> CheckUsernames(int agentId, byte roleId, string usernames);
    }

    public class UsernameSuggestionServiceImpl : UsernameSuggestionService
    {
        private readonly IDynamicRepository dynamicRepository;

        public UsernameSuggestionServiceImpl(IDynamicRepository dynamicRepository)
            => this.dynamicRepository = dynamicRepository;

        public async Task<IEnumerable<UsernameSuggestion>> GetUsernamesSuggestion(
               int actorId,
               string suggestionQuery,
               bool isInternal,
               bool directDownline,
               bool isAccInfo)
               => (await dynamicRepository.FetchAsync<UsernameSuggestion>(new GetUsernameSuggestionCriteria(
                   actorId,
                   suggestionQuery,
                   isInternal,
                   directDownline,
                   isAccInfo))).OrderBy(item => item.Username);

        public Task UpdateUserSearch(int userId, string search, string firstName, string lastName, string nickname)
            => dynamicRepository.ExecuteAsync(new UpdateUserSearchCriteria(userId, search, firstName, lastName, nickname));

        public Task<IEnumerable<UsernameErrCode>> CheckUsernames(int agentId, byte roleId, string usernames)
            => dynamicRepository.FetchAsync<UsernameErrCode>(new CheckUsernamesCriteria(agentId, roleId, usernames));
    }
}
