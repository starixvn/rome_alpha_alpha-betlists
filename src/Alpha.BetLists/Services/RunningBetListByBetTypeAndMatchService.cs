namespace Alpha.BetLists.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Criteria.RunningBetListByBetTypeAndMatch;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.Repository;
    using Helpers;
    using Models;
    using Params;

    public interface RunningBetListByBetTypeAndMatchService
    {
        Task<BetListData> GetBetList(RunningBetListByBetTypeAndMatchParam param);
    }

    public class RunningBetListByBetTypeAndMatchServiceImpl : RunningBetListByBetTypeAndMatchService
    {
        private readonly IDynamicRepository dynamicRepository;

        public RunningBetListByBetTypeAndMatchServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<BetListData> GetBetList(RunningBetListByBetTypeAndMatchParam param)
        {
            Tuple<IEnumerable<TicketDto>, IEnumerable<TicketData>> betListDataTuple;

            betListDataTuple = BetTypeHelper.IsHorseRacing(param.BetType)
                ? await GetRacingByMatch(param)
                : await GetRunningByBetTypeAndMatch(param);

            return new BetListData(betListDataTuple.Item1, betListDataTuple.Item2.ToList<ITicketData>());
        }

        private async Task<Tuple<IEnumerable<TicketDto>, IEnumerable<TicketData>>> GetRacingByMatch(RunningBetListByBetTypeAndMatchParam param)
            => await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(
                new GetBetListRacingByMatchCriteria(
                    param.CustId,
                    param.BetType,
                    param.MatchId,
                    param.Language,
                    param.IsShowRacing,
                    param.CustLevel));

        private async Task<Tuple<IEnumerable<TicketDto>, IEnumerable<TicketData>>> GetRunningByBetTypeAndMatch(RunningBetListByBetTypeAndMatchParam param)
            => await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(
                new GetBetListRunningByBetTypeAndMatchCriteria(
                    param.MatchId,
                    param.CustId,
                    param.BetType,
                    param.Language,
                    param.LiveIndicator,
                    param.BetTeam,
                    param.CustLevel));
    }
}
