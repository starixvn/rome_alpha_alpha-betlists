namespace Alpha.BetLists.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.BetLists.Criteria.CancelledBet;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Models.CancelledBet;
    using Alpha.BetLists.Params;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.Repository;

    public interface CancelledBetService
    {
        Task<CancelledBetModel> GetCancelledBet(CancelledBetParam parameter);
    }

    public class CancelledBetServiceImpl : CancelledBetService
    {
        private readonly IDynamicRepository dynamicRepository;

        public CancelledBetServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<CancelledBetModel> GetCancelledBet(CancelledBetParam parameter)
        {
            var filter = await GetFilterCancelledBet(parameter);

            var betList = await GetCancelledBetList(filter, parameter.Language);

            return new CancelledBetModel(filter, betList);
        }

        private static GetCancelledBetCriteria CreateCancelledBetListCriteria(FilterCancelledBetModel parameter, string language)
        {
            var id = parameter.MemberId;
            var userLevel = UserLevel.Member;
            if (id == 0)
            {
                if (parameter.AgentId > 0)
                {
                    id = parameter.AgentId;
                    userLevel = UserLevel.Agent;
                }
                else
                {
                    id = parameter.MasterId;
                    userLevel = UserLevel.Master;
                }
            }

            return new GetCancelledBetCriteria(id, userLevel.Value, language);
        }

        private async Task<IEnumerable<UserBetCount>> GetUserHasCancelledBet(int custId, UserLevel userLevel)
        {
            var criteria = new GetUsersHasCancelledBetCriteria(custId, userLevel);

            return await dynamicRepository.FetchAsync<UserBetCount>(criteria);
        }

        private async Task<FilterCancelledBetModel> GetFilterCancelledBet(CancelledBetParam parameter)
        {
            if (parameter.Level.IsSuperLevel())
            {
                return await GetFilterCancelledBetBySuper(parameter);
            }

            if (parameter.Level.IsMasterLevel())
            {
                return await GetFilterCancelledBetByMaster(parameter);
            }

            return await GetFilterCancelledBetByAgent(parameter);
        }

        private async Task<BetListData> GetCancelledBetList(FilterCancelledBetModel parameter, string language)
        {
            var criteria = CreateCancelledBetListCriteria(parameter, language);
            var ticketData = await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(criteria);

            return new BetListData(ticketData.Item1, ticketData.Item2.ToList<ITicketData>());
        }

        private async Task<FilterCancelledBetModel> GetFilterCancelledBetBySuper(CancelledBetParam parameter)
        {
            var filter = new FilterCancelledBetModel();

            filter.Masters = await GetUserHasCancelledBet(parameter.CurrentUserId, parameter.Level);
            filter.MasterId = parameter.MasterId == 0 && filter.Masters.Any() ? filter.Masters.First().CustId : parameter.MasterId;

            filter.Agents = await GetUserHasCancelledBet(filter.MasterId, parameter.Level);
            filter.AgentId = filter.Agents.Any(i => i.CustId == parameter.AgentId) ? parameter.AgentId : 0;

            filter.Members = parameter.AgentId > 0 ? await GetUserHasCancelledBet(filter.AgentId, parameter.Level) : filter.Members;
            filter.MemberId = filter.Members.Any(i => i.CustId == parameter.MemberId) ? parameter.MemberId : 0;

            return filter;
        }

        private async Task<FilterCancelledBetModel> GetFilterCancelledBetByMaster(CancelledBetParam parameter)
        {
            var filter = new FilterCancelledBetModel();
            filter.Agents = await GetUserHasCancelledBet(parameter.CurrentUserId, parameter.Level);
            filter.AgentId = parameter.AgentId == 0 && filter.Agents.Any() ? filter.Agents.First().CustId : parameter.AgentId;

            filter.Members = await GetUserHasCancelledBet(filter.AgentId, parameter.Level);
            filter.MemberId = filter.Members.Any(i => i.CustId == parameter.MemberId) ? parameter.MemberId : 0;

            return filter;
        }

        private async Task<FilterCancelledBetModel> GetFilterCancelledBetByAgent(CancelledBetParam parameter)
        {
            var filter = new FilterCancelledBetModel();

            filter.Members = await GetUserHasCancelledBet(parameter.CurrentUserId, parameter.Level);
            filter.MemberId = parameter.MemberId == 0 && filter.Members.Any() ? filter.Members.First().CustId : parameter.MemberId;

            return filter;
        }
    }
}
