namespace Alpha.BetLists.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Alpha.BetLists.Criteria;
    using Criteria.Parlay;
    using Fanex.Data.Repository;
    using Models;
    using Models.Parlay;
    using Alpha.BetLists.Constants;
    using System.Linq;

    public interface ParlayService
    {
        Task UpdateOddsMixParlay(IEnumerable<TicketDto> tickets);

        Task<IEnumerable<OddsMixParlayModel>> GetOddsMixParlay(IEnumerable<long> transIds);

        Task<IEnumerable<TicketDto>> GetParlayBetList(long transId, int custId);

        Task<ComboParlayDetailData> GetComboParlayDetail(long refNo, int custId, string language);
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

        public async Task<IEnumerable<OddsMixParlayModel>> GetOddsMixParlay(IEnumerable<long> transIds)
            => await dynamicRepository.FetchAsync<OddsMixParlayModel>(new GetOddsOfMixParlayCriteria(transIds));

        public async Task<IEnumerable<TicketDto>> GetParlayBetList(long transId, int custId)
        {
            var tickets = await dynamicRepository.FetchAsync<TicketDto>(new GetParlayBetListCriteria(transId, custId));

            return tickets;
        }

        public async Task<ComboParlayDetailData> GetComboParlayDetail(long refNo, int custId, string language)
        {
            var criteria = new GetComboParlayDetailCriteria(
                    refNo,
                    custId,
                    reportDateService.GetBalanceUpDay(),
                    reportDateService.GetMoveBetsDay());

            var comboParlayDetailTuple = await dynamicRepository.FetchMultipleAsync<ComboParlayDetail, MatchDetail>(criteria);

            return new ComboParlayDetailData(comboParlayDetailTuple.Item1, comboParlayDetailTuple.Item2, criteria.Outstanding, criteria.IsParlayBoost, criteria.IsMixParlay);
        }

        public async Task UpdateOddsMixParlay(IEnumerable<TicketDto> tickets)
        {
            var ticketsMissingOdds = GetMixParlayMissingOdds(tickets);
            if (ticketsMissingOdds.Any())
            {
                var transIds = ticketsMissingOdds.Select(i => i.TransId);
                var odds = await GetOddsMixParlay(transIds);

                foreach (var ticket in tickets)
                {
                    if (transIds.Any(i => i == ticket.TransId))
                    {
                        var oddsItem = GetOdds(odds, ticket.TransId);

                        ticket.Odds = oddsItem.Odds;
                        ticket.MasterOdds = oddsItem.MasterOdds;
                    }
                }
            }
        }

        private static IEnumerable<TicketDto> GetMixParlayMissingOdds(IEnumerable<TicketDto> tickets)
            => tickets.Where(i => i.BetTypeId == BetTypeIdConstants.CombinationMixParlay && i.BetTeam == "1" && i.Odds == 0);

        private static OddsMixParlayModel GetOdds(IEnumerable<OddsMixParlayModel> oddsList, long transId)
        {
            foreach (var item in oddsList)
            {
                if (item.TransId == transId)
                {
                    return item;
                }
            }

            return new OddsMixParlayModel();
        }
    }
}