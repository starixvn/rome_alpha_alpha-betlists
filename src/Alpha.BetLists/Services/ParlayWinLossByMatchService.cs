namespace Alpha.BetLists.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Alpha.PartialUM.Services;
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

        public ParlayWinLossByMatchServiceImpl(
            ReportDateService reportDateService,
            IDynamicRepository dynamicRepository,
            IPartialUMService partialUMService)
        {
            this.reportDateService = reportDateService;
            this.dynamicRepository = dynamicRepository;
            this.partialUMService = partialUMService;
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

            return tickets;
        }
    }
}
