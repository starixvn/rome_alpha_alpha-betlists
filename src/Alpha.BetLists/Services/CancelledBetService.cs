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

            var betList = await GetCancelledBetList(parameter);

            return new CancelledBetModel(filter, betList);
        }

        protected async Task<IEnumerable<UserBetCount>> GetMasterUserHasCancelledBet(UserLevel level, int superId)
        {
            IEnumerable<UserBetCount> masters = new List<UserBetCount>();
            if (level.IsSuperLevel())
            {
                var criteria = new GetUsersHasCancelledBetBySuperCriteria(superId);
                if (criteria.IsValid())
                {
                    masters = await dynamicRepository.FetchAsync<UserBetCount>(criteria);
                }
            }

            return masters;
        }

        protected async Task<IEnumerable<UserBetCount>> GetAgentUserHasCancelledBet(UserLevel level, int masterId)
        {
            IEnumerable<UserBetCount> masters = new List<UserBetCount>();
            if (level.IsMasterOrSuperLevel())
            {
                var criteria = new GetUsersHasCancelledBetByMasterCriteria(masterId);
                if (criteria.IsValid())
                {
                    masters = await dynamicRepository.FetchAsync<UserBetCount>(criteria);
                }
            }

            return masters;
        }

        protected async Task<IEnumerable<UserBetCount>> GetUsersHasCancelledBetByAgent(int agentId)
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
            var filter = new FilterCancelledBetModel();
            if (parameter.Level.IsSuperLevel())
            {
                filter.Masters = await GetMasterUserHasCancelledBet(parameter.Level, parameter.CurrentUserId);
                filter.MasterId = parameter.MasterId == 0 ? filter.Masters.FirstOrDefault().CustId : parameter.MasterId;

                filter.Agents = await GetAgentUserHasCancelledBet(parameter.Level, filter.MasterId);
                filter.AgentId = parameter.AgentId;

                filter.Members = parameter.AgentId > 0 ? await GetUsersHasCancelledBetByAgent(filter.AgentId) : filter.Members;
                filter.MemberId = parameter.MemberId;
            }
            else if (parameter.Level.IsMasterLevel())
            {
                filter.Agents = await GetAgentUserHasCancelledBet(parameter.Level, parameter.CurrentUserId);
                filter.AgentId = parameter.AgentId == 0 ? filter.Agents.FirstOrDefault().CustId : parameter.AgentId;

                filter.Members = await GetUsersHasCancelledBetByAgent(filter.AgentId);
                filter.MemberId = parameter.MemberId;
            }
            else
            {
                filter.Members = await GetUsersHasCancelledBetByAgent(parameter.CurrentUserId);
                filter.MemberId = parameter.MemberId;
            }

            return filter;
        }

        protected async Task<BetListData> GetCancelledBetList(CancelledBetParam parameter)
        {
            BetListData betData = null;
            var id = parameter.MemberId;
            var userLevel = UserLevel.Member;

            if (id == 0 && parameter.AgentId > 0)
            {
                id = parameter.AgentId;
                userLevel = UserLevel.Agent;
            }
            if (id == 0 && parameter.MasterId > 0)
            {
                id = parameter.MasterId;
                userLevel = UserLevel.Master;
            }

            var criteria = new GetCancelledBetCriteria(id, userLevel, parameter.Language);
            if (criteria.IsValid())
            {
                var ticketData = await dynamicRepository.FetchMultipleAsync<Ticket, TicketData>(criteria);

                betData = new BetListData(ticketData.Item1, ticketData.Item2.ToList<ITicketData>());
            }

            return betData;
        }
    }
}
