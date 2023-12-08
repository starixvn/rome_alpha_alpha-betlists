namespace Alpha.BetLists.Services
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.BetLists.DataAccesses;
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
        private readonly IBetListRepository betlistRepository;

        public RunningBetListByBetTypeAndMatchServiceImpl(
            IDynamicRepository dynamicRepository, 
            IBetListRepository betlistRepository)
        {
            this.dynamicRepository = dynamicRepository;
            this.betlistRepository = betlistRepository;
        }

        public async Task<BetListData> GetBetList(RunningBetListByBetTypeAndMatchParam param)
        {
            var betListDataTuple = await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(new GetBetListRunningByBetTypeAndMatchCriteria(
                    param.MatchId,
                    param.CustId,
                    param.BetType,
                    param.LiveIndicator,
                    param.BetTeam,
                    param.CustLevel));
            //var betListDataTuple = betlistRepository.FetchMultiple(
            //    new GetBetListRunningByBetTypeAndMatchCriteria(
            //        param.MatchId,
            //        param.CustId,
            //        param.BetType,
            //        param.LiveIndicator,
            //        param.BetTeam,
            //        param.CustLevel));

            return new BetListData(betListDataTuple.Item1, betListDataTuple.Item2.ToList<ITicketData>());
        }

    }
}
