namespace Alpha.BetLists.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Alpha.Web.Helpers;
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
        private readonly PartialUMService partialUMService;

        public ParlayWinLossByMatchServiceImpl(
            ReportDateService reportDateService,
            IDynamicRepository dynamicRepository,
            PartialUMService partialUMService)
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
                param.Language,
                param.FromDate.GetGreaterDate(param.LimitedDate),
                param.ToDate,
                reportDateService.GetMoveBetsDay(),
                param.CustLevel);

            partialUMService.CheckPartialUM(criteria.GetSettingKey());

            var tickets = await dynamicRepository.FetchAsync<TicketDto>(criteria);

            return tickets;
        }
    }
}
