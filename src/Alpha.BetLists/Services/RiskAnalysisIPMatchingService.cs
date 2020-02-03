namespace Alpha.BetLists.Services
{
    using Alpha.BetLists.Criteria.RiskAnalysisIPMatching;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.Repository;
    using Models;
    using Params;
    using System.Linq;
    using System.Threading.Tasks;

    public interface RiskAnalysisIPMatchingService
    {
        Task<BetListData> GetBetList(RiskAnalysisIPMatchingParam param);
    }

    public class RiskAnalysisIPMatchingServiceImpl : RiskAnalysisIPMatchingService
    {
        private readonly IDynamicRepository dynamicRepository;

        public RiskAnalysisIPMatchingServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<BetListData> GetBetList(RiskAnalysisIPMatchingParam param)
        {
            var criteria = new GetRiskAnalysisIPMatchingCriteria(
                    param.CustId,
                    param.IP,
                    param.Date,
                    param.MoveBetsDay);

            var betListDataTuple = await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(criteria);

            return new BetListData(betListDataTuple.Item1, betListDataTuple.Item2.ToList<ITicketData>());
        }
    }
}