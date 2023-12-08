namespace Alpha.BetLists.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.BetLists.DataAccesses;
    using AlphaPlus.PartialUM.Services;
    using Criteria.SearchTickets;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.Repository;
    using Models;
    using Params;

    public interface SearchTicketsService
    {
        Task<BetListData> GetBetList(SearchTicketsParam param);
    }

    public class SearchTicketsServiceImpl : SearchTicketsService
    {
        private readonly ReportDateService reportDateService;
        private readonly IDynamicRepository dynamicRepository;
        private readonly IPartialUMService partialUMService;
        private readonly IBetListRepository betlistRepository;

        public SearchTicketsServiceImpl(
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

        public async Task<BetListData> GetBetList(SearchTicketsParam param)
        {
            var criteria = new SearchTicketsCriteria(param.RefNo, param.UplineId, param.CustLevel, reportDateService.GetMoveBetsDay(), reportDateService.GetBalanceUpDay());

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            //var betListDataTuple = await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(criteria);
            var betListDataTuple = betlistRepository.FetchMultiple(criteria);

            return new BetListData(betListDataTuple.Item1, betListDataTuple.Item2.ToList<ITicketData>());
        }

    }
}
