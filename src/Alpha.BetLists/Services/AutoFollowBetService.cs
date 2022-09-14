namespace Alpha.BetLists.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.BetLists.Criteria;
    using Alpha.BetLists.Models;
    using Fanex.Data.Repository;

    public interface AutoFollowBetService
    {
        Task<int> GetRealCustId(int agentId, int dummyCustId);
    }

    public class AutoFollowBetServiceImpl : AutoFollowBetService
    {
        private readonly IDynamicRepository repository;

        public AutoFollowBetServiceImpl(IDynamicRepository dynamicRepository)
            => repository = dynamicRepository;

        public async Task<int> GetRealCustId(int agentId, int dummyCustId)
        {
            var followBetAccounts = await repository.FetchAsync<FollowBetAccount>(new GetFollowBetAccountsCriteria(agentId));
            var followBetAccount = followBetAccounts.FirstOrDefault(account => account.DummyCustId == dummyCustId);

            return followBetAccount == null ? 0 : followBetAccount.CustId;
        }
    }
}
