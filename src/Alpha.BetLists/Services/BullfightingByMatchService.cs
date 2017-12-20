namespace Alpha.BetLists.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Criteria.BullfightingByMatch;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.Repository;
    using Models;
    using Params;
    using SmartPunter.Services;

    public interface BullfightingByMatchService
    {
        Task<IEnumerable<TicketDto>> GetBetList(BullfightingByMatchParam param);
    }

    public class BullfightingByMatchServiceImpl : BullfightingByMatchService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly ISmartPunterBetListMonitor monitorService;

        public BullfightingByMatchServiceImpl(IDynamicRepository dynamicRepository, ISmartPunterBetListMonitor monitorService)
        {
            this.dynamicRepository = dynamicRepository;
            this.monitorService = monitorService;
        }

        public async Task<IEnumerable<TicketDto>> GetBetList(BullfightingByMatchParam param)
        {
            var tickets = await dynamicRepository.FetchAsync<TicketDto>(
                new GetBetListBullfightingByMatchCriteria(
                    param.CustId,
                    param.MatchId,
                    param.CustLevel));

            monitorService.WriteLog(param.CustId, tickets.ToList<ITicket>());

            return tickets;
        }
    }
}
