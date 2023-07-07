namespace Alpha.BetLists.Site.Handlers.MonitoringBets
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Models.MonitoringBets;
    using Alpha.BetLists.Params.MonitoringBets;
    using Alpha.BetLists.Services;
    using Alpha.BetLists.Site.Requests.MonitoringBets;
    using Alpha.BetLists.Site.ViewModels.MonitoringBets;
    using Alpha.IPInfo.Services;
    using Alpha.SmartPunter.Services;
    using Alpha.Utilities.IP;
    using Alpha.Web.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;

    public class MonitoringBetsHandler :
        BetListBaseHandler,
        IAsyncRequestHandler<MonitoringBetsQuery, MonitoringBetsViewModel>,
        IAsyncRequestHandler<ManageFiltersQuery, IEnumerable<FilterConfiguration>>,
        IAsyncRequestHandler<AddFilterCommand, Result>,
        IAsyncRequestHandler<UpdateFilterCommand, Result>,
        IAsyncRequestHandler<DeleleFiltersCommand, IEnumerable<Result>>,
        IAsyncRequestHandler<FilterBySportsQuery, IEnumerable<Sport>>,
        IAsyncRequestHandler<CheckUsernamesQuery, IEnumerable<MemberErrCode>>,
        IAsyncRequestHandler<GetMonitoredWatchlistMembersQuery, IEnumerable<MonitoredWatchlistMember>>
    {
        private readonly MonitoringBetsService monitoringBetService;

        public MonitoringBetsHandler(
            MonitoringBetsService monitoringBetService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            IIPInfoService ipInfoService) : base(builder, ticketHelper, monitorService, ipService, ipInfoService)
        {
            this.monitoringBetService = monitoringBetService;
        }

        public async Task<MonitoringBetsViewModel> Handle(MonitoringBetsQuery message)
        {
            var filters = await monitoringBetService.GetFilters(message.ViewerId, message.SubAccId);

            return new MonitoringBetsViewModel(message, filters, null, Builder, TicketHelper);
        }

        public Task<IEnumerable<FilterConfiguration>> Handle(ManageFiltersQuery message)
            => monitoringBetService.GetFilters(message.ViewerId, message.SubAccId);

        public Task<Result> Handle(UpdateFilterCommand message)
            => monitoringBetService.UpdateFilter(new UpdateFilterParam(
                message.ViewerId,
                message.SubAccId,
                message.FilterId,
                message.FilterName,
                message.IsDefault,
                message.LanguageKey,
                message.IP));

        public Task<IEnumerable<Result>> Handle(DeleleFiltersCommand message)
            => monitoringBetService.DeleteFilters(
                new DeleteFiltersParam(message.ViewerId, message.SubAccId, message.Ids, message.IP));

        public Task<IEnumerable<Sport>> Handle(FilterBySportsQuery message)
            => monitoringBetService.GetSports(message.IsLive, message.GroupSport);

        public Task<IEnumerable<MemberErrCode>> Handle(CheckUsernamesQuery message)
            => monitoringBetService.CheckUsernames(message.ViewerId, message.Usernames);

        public Task<IEnumerable<MonitoredWatchlistMember>> Handle(GetMonitoredWatchlistMembersQuery message)
            => monitoringBetService.GetMonitoredWatchlistMembers(message.ViewerId);

        public Task<Result> Handle(AddFilterCommand message)
            => monitoringBetService.AddFilter(new AddFilterParam(
                message.ViewerId,
                message.SubAccId,
                message.FilterName,
                message.IsDefault,
                message.BetType,
                message.Stake,
                message.Sports,
                message.IsLive,
                message.InputList,
                message.WatchList,
                message.IsWatchList,
                message.LanguageKey,
                message.IP));
    }
}