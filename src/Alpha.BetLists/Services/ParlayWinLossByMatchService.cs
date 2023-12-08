namespace Alpha.BetLists.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Alpha.BetLists.DataAccesses;
    using AlphaPlus.PartialUM.Services;
    using Criteria.ParlayWinLossByMatch;
    using Extensions;
    using Fanex.Data.Repository;
    using Models;
    using Params;

    public interface ParlayWinLossByMatchService
    {
        Task<IEnumerable<TicketDto>> GetBetList(ParlayWinLossByMatchParam param);
    }

    public class ParlayWinLossByMatchServiceImpl : ParlayWinLossByMatchService
    {
        private readonly ReportDateService reportDateService;
        private readonly IDynamicRepository dynamicRepository;
        private readonly IPartialUMService partialUMService;
        private readonly IBetListRepository betlistRepository;

        public ParlayWinLossByMatchServiceImpl(
            ReportDateService reportDateService,
            IDynamicRepository dynamicRepository,
            IPartialUMService partialUMService,
            IBetListRepository betlistRepository)
        {
            this.reportDateService = reportDateService;
            this.dynamicRepository = dynamicRepository;
            this.partialUMService = partialUMService;
            this.betlistRepository = betlistRepository;
        }

        public async Task<IEnumerable<TicketDto>> GetBetList(ParlayWinLossByMatchParam param)
        {
            var criteria = new GetParlayWinLossByMatchCriteria(
                param.CustId,
                param.BetType,
                param.FromDate.GetGreaterDate(param.LimitedDate),
                param.ToDate,
                reportDateService.GetMoveBetsDay(),
                param.CustLevel);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            var tickets = await dynamicRepository.FetchAsync<TicketDto>(criteria);
            //var tickets = betlistRepository.Fetch(criteria);

            return tickets;
        }
    }
}
