namespace Alpha.BetLists.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Criteria.Parlay;
    using Fanex.Data.Repository;
    using Models;
    using Models.Parlay;

    public interface ParlayService
    {
        Task<IEnumerable<TicketDto>> GetParlayBetList(long transId, string language);

        Task<ComboParlayDetailData> GetComboParlayDetail(long transId, long refNo, long betId, int custId, string language);
    }

    public class ParlayServiceImpl : ParlayService
    {
        private readonly ReportDateService reportDateService;
        private readonly IDynamicRepository dynamicRepository;

        public ParlayServiceImpl(
            ReportDateService reportDateService,
            IDynamicRepository dynamicRepository)
        {
            this.reportDateService = reportDateService;
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IEnumerable<TicketDto>> GetParlayBetList(long transId, string language)
        {
            var tickets = await dynamicRepository.FetchAsync<TicketDto>(new GetParlayBetListCriteria(transId, language));

            return tickets;
        }

        public async Task<ComboParlayDetailData> GetComboParlayDetail(long transId, long refNo, long betId, int custId, string language)
        {
            var criteria = new GetComboParlayDetailCriteria(
                    transId,
                    refNo,
                    betId,
                    custId,
                    reportDateService.GetBalanceUpDay(),
                    reportDateService.GetMoveBetsDay(),
                    language);

            var comboParlayDetailTuple = await dynamicRepository.FetchMultipleAsync<ComboParlayDetail, MatchDetail>(criteria);

            return new ComboParlayDetailData(comboParlayDetailTuple.Item1, comboParlayDetailTuple.Item2, criteria.Outstanding);
        }
    }
}