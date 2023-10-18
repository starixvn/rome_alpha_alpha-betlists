namespace Alpha.BetLists.Services
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Criteria.RunningBetListByBetTypeAndMatch;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.Repository;
    using Models;
    using Params;

    public interface RunningBetListByBetTypeAndMatchService
    {
        Task<BetListData> GetBetList(RunningBetListByBetTypeAndMatchParam param);
    }

    public class RunningBetListByBetTypeAndMatchServiceImpl : RunningBetListByBetTypeAndMatchService
    {
        private readonly IDynamicRepository dynamicRepository;

        public RunningBetListByBetTypeAndMatchServiceImpl(IDynamicRepository dynamicRepository, ParlayService parlayService)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<BetListData> GetBetList(RunningBetListByBetTypeAndMatchParam param)
        {
            var betListDataTuple = await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(
                new GetBetListRunningByBetTypeAndMatchCriteria(
                    param.MatchId,
                    param.CustId,
                    param.BetType,
                    param.LiveIndicator,
                    param.BetTeam,
                    param.CustLevel));

            return new BetListData(betListDataTuple.Item1, betListDataTuple.Item2.ToList<ITicketData>());
        }

    }
}
