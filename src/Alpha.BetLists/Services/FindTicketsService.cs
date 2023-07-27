namespace Alpha.BetLists.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.PartialUM.Services;
    using Criteria.FindTickets;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.Repository;
    using Helpers;
    using Models;
    using Params;

    public interface FindTicketsService
    {
        Task<BetListData> GetBetList(FindTicketsParam param);
    }

    public class FindTicketsServiceImpl : FindTicketsService
    {
        private readonly ReportDateService reportDateService;
        private readonly IDynamicRepository dynamicRepository;
        private readonly IPartialUMService partialUMService;

        public FindTicketsServiceImpl(
            ReportDateService reportDateService,
            IDynamicRepository dynamicRepository,
            IPartialUMService partialUMService)
        {
            this.reportDateService = reportDateService;
            this.dynamicRepository = dynamicRepository;
            this.partialUMService = partialUMService;
        }

        public async Task<BetListData> GetBetList(FindTicketsParam param)
        {
            var criteria = new FindTicketsCriteria(param.RefNo, param.UplineId, param.CustLevel, reportDateService.GetMoveBetsDay());

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            var betListDataTuple = await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(criteria);

            return new BetListData(betListDataTuple.Item1, betListDataTuple.Item2.ToList<ITicketData>());
        }

    }
}
