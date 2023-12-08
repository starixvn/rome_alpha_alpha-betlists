namespace Alpha.BetLists.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.BetLists.Criteria.RiskAnalysisIPMatching;
    using Alpha.BetLists.DataAccesses;
    using AlphaPlus.PartialUM.Services;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.Repository;
    using Models;
    using Params;

    public interface RiskAnalysisIPMatchingService
    {
        Task<BetListData> GetBetList(RiskAnalysisIPMatchingParam param);
    }

    public class RiskAnalysisIPMatchingServiceImpl : RiskAnalysisIPMatchingService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly IPartialUMService partialUMService;
        private readonly IBetListRepository betlistRepository;

        public RiskAnalysisIPMatchingServiceImpl(
            IDynamicRepository dynamicRepository,
            IPartialUMService partialUMService,
            IBetListRepository betlistRepository)
        {
            this.dynamicRepository = dynamicRepository;
            this.partialUMService = partialUMService;
            this.betlistRepository = betlistRepository;
        }

        public async Task<BetListData> GetBetList(RiskAnalysisIPMatchingParam param)
        {
            var criteria = new GetRiskAnalysisIPMatchingCriteria(
                    param.CustId,
                    param.IP,
                    param.Date,
                    param.MoveBetsDay);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            //var betListDataTuple = await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(criteria);
            var betListDataTuple = betlistRepository.FetchMultiple(criteria);
            return new BetListData(
                betListDataTuple.Item1.OrderByDescending(ticket => ticket.TransDate), 
                betListDataTuple.Item2.ToList<ITicketData>());
        }
    }
}