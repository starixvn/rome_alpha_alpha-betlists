using Alpha.BetLists.Criteria.CancelledBet;
using Alpha.BetLists.Models;
using Alpha.BetLists.Models.CancelledBet;
using Alpha.BetLists.Params;
using Fanex.BetList.Core.Entities;
using Fanex.Data.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alpha.BetLists.Services
{
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

        protected async Task<IEnumerable<UserBetCount>> GetUserHasCancelledBetBySuper(int superId)
        {
            IEnumerable<UserBetCount> masters = new List<UserBetCount>();
            var criteria = new GetUsersHasCancelledBetBySuperCriteria(superId);
            if (criteria.IsValid())
            {
                masters = await dynamicRepository.FetchAsync<UserBetCount>(criteria);
            }

            return masters;
        }

        protected async Task<IEnumerable<UserBetCount>> GetUserHasCancelledBetByMaster(int masterId)
        {
            IEnumerable<UserBetCount> masters = new List<UserBetCount>();
            var criteria = new GetUsersHasCancelledBetByMasterCriteria(masterId);
            if (criteria.IsValid())
            {
                masters = await dynamicRepository.FetchAsync<UserBetCount>(criteria);
            }

            return masters;
        }

        protected async Task<IEnumerable<UserBetCount>> GetMembersHasCancelledBetByAgent(int agentId)
        {
            IEnumerable<UserBetCount> users = new List<UserBetCount>();
            var criteria = new GetUsersHasCancelledBetByAgentCriteria(agentId);
            if (criteria.IsValid())
            {
                users = await dynamicRepository.FetchAsync<UserBetCount>(criteria);
            }

            return users;
        }

        protected async Task<FilterCancelledBetModel> GetFilterCancelledBet(CancelledBetParam parameter)
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

        protected async Task<BetListData> GetCancelledBetList(FilterCancelledBetModel parameter, string language)
        {
            var betData = new BetListData(new List<TicketDto>(), new List<ITicketData>());
            var criteria = CreateCancelledBetListCriteria(parameter, language);
            if (criteria.IsValid())
            {
                var ticketData = await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(criteria);

                betData = new BetListData(ticketData.Item1, ticketData.Item2.ToList<ITicketData>());
            }

            return betData;
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

            return new GetCancelledBetCriteria(id, userLevel, language);
        }

        private async Task<FilterCancelledBetModel> GetFilterCancelledBetBySuper(CancelledBetParam parameter)
        {
            var filter = new FilterCancelledBetModel();

            filter.Masters = await GetUserHasCancelledBetBySuper(parameter.CurrentUserId);
            if (parameter.MasterId == 0 && filter.Masters.Any())
            {
                filter.MasterId = filter.Masters.First().CustId;
            }

            filter.Agents = await GetUserHasCancelledBetByMaster(filter.MasterId);
            filter.AgentId = filter.Agents.Any(i => i.CustId == parameter.AgentId) ? parameter.AgentId : 0;

            filter.Members = parameter.AgentId > 0 ? await GetMembersHasCancelledBetByAgent(filter.AgentId) : filter.Members;
            filter.MemberId = filter.Members.Any(i => i.CustId == parameter.MemberId) ? parameter.MemberId : 0;

            return filter;
        }

        private async Task<FilterCancelledBetModel> GetFilterCancelledBetByMaster(CancelledBetParam parameter)
        {
            var filter = new FilterCancelledBetModel();
            filter.Agents = await GetUserHasCancelledBetByMaster(parameter.CurrentUserId);
            if (parameter.AgentId == 0 && filter.Agents.Any())
            {
                filter.AgentId = filter.Agents.First().CustId;
            }

            filter.Members = await GetMembersHasCancelledBetByAgent(filter.AgentId);
            filter.MemberId = filter.Members.Any(i => i.CustId == parameter.MemberId) ? parameter.MemberId : 0;

            return filter;
        }

        private async Task<FilterCancelledBetModel> GetFilterCancelledBetByAgent(CancelledBetParam parameter)
        {
            var filter = new FilterCancelledBetModel();

            filter.Members = await GetMembersHasCancelledBetByAgent(parameter.CurrentUserId);
            if (parameter.MemberId == 0 && filter.Members.Any())
            {
                filter.MemberId = filter.Members.First().CustId;
            }

            return filter;
        }
    }
}
