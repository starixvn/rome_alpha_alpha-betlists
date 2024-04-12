namespace Alpha.BetLists.Services
{
    using Alpha.BetLists.Criteria.CancelledBet;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Params;
    using AlphaPlus.PartialUM.Services;
    using Fanex.Data.Repository;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface BookedBetService
    {
        Task<IEnumerable<TicketDto>> Get(BookedBetParam parameter);
    }

    public class BookedBetServiceImpl : BookedBetService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly IPartialUMService partialUMService;

        public BookedBetServiceImpl(
            IDynamicRepository dynamicRepository,
            IPartialUMService partialUMService)
        {
            this.dynamicRepository = dynamicRepository;
            this.partialUMService = partialUMService;
        }

        public async Task<IEnumerable<TicketDto>> Get(BookedBetParam parameter)
        {
            var criteria = new GetBookedBetCriteria(
                parameter.CustId,
                parameter.FromDate,
                parameter.ToDate,                
                parameter.RoleId,
                parameter.StatusIds,
                parameter.PageSize,
                parameter.PageIndex);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            return await dynamicRepository.FetchAsync<TicketDto>(criteria);
        }
    }
}
