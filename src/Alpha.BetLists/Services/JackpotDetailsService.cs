namespace Alpha.BetLists.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Alpha.BetLists.DataAccesses;
    using Criteria.ProgressiveJackpot;
    using Fanex.Data.Repository;
    using Models;
    using Params;

    public interface JackpotDetailsService
    {
        Task<IEnumerable<TicketDto>> GetJackpotDetails(JackpotDetailsParam param);
    }

    public class JackpotDetailsServiceImpl : JackpotDetailsService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly ReportDateService reportDateService;
        private readonly IBetListRepository betlistRepository;

        public JackpotDetailsServiceImpl(
            IDynamicRepository dynamicRepository, 
            ReportDateService reportDateService,
            IBetListRepository betlistRepository)
        {
            this.dynamicRepository = dynamicRepository;
            this.reportDateService = reportDateService;
            this.betlistRepository = betlistRepository;
        }

        public async Task<IEnumerable<TicketDto>> GetJackpotDetails(JackpotDetailsParam param)
             => betlistRepository.Fetch(
                new GetJackpotDetailsCriteria(param.TransId,
                    param.CustId,
                    param.BetType,
                    reportDateService.GetMoveBetsDay(),
                    reportDateService.GetBalanceUpDay()));
        //=> await dynamicRepository.FetchAsync<TicketDto>(
        //    new GetJackpotDetailsCriteria(param.TransId,
        //        param.CustId,
        //        param.BetType,
        //        reportDateService.GetMoveBetsDay(),
        //        reportDateService.GetBalanceUpDay()));
    }
}