namespace Alpha.BetLists.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.BetLists.Criteria;
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
    }
}
