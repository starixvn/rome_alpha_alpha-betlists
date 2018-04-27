namespace Alpha.BetLists.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
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

        public ParlayWinLossByMatchServiceImpl(
            ReportDateService reportDateService,
            IDynamicRepository dynamicRepository)
        {
            this.reportDateService = reportDateService;
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IEnumerable<TicketDto>> GetBetList(ParlayWinLossByMatchParam param)
        {
            var criteria = new GetParlayWinLossByMatchCriteria(
                param.CustId,
                param.BetType,
                param.Language,
                param.FromDate.GetLaterDate(param.LimitedDate),
                param.ToDate,
                reportDateService.GetMoveBetsDay(),
                param.CustLevel);

            var tickets = await dynamicRepository.FetchAsync<TicketDto>(criteria);

            return tickets;
        }
    }
}
