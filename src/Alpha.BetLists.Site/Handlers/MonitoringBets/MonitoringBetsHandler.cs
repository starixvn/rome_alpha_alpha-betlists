namespace Alpha.BetLists.Site.Handlers.MonitoringBets
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Models.MonitoringBets;
    using Alpha.BetLists.Params.MonitoringBets;
    using Alpha.BetLists.Services;
    using Alpha.BetLists.Site.Constants;
    using Alpha.BetLists.Site.Requests.MonitoringBets;
    using Alpha.BetLists.Site.ViewModels.MonitoringBets;
    using Alpha.IPInfo.Services;
    using Alpha.SmartPunter.Services;
    using Alpha.Utilities.IP;
    using Alpha.Web.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Sunplus.Agent.Model.Entities;

    public class MonitoringBetsHandler :
        BetListBaseHandler,
        IAsyncRequestHandler<MonitoringBetsQuery, MonitoringBetsViewModel>,
        IAsyncRequestHandler<ManageFiltersQuery, IEnumerable<FilterConfiguration>>,
        IAsyncRequestHandler<ApplyFilterCommand, Result>,
        IAsyncRequestHandler<AddFilterCommand, Result>,
        IAsyncRequestHandler<UpdateFilterCommand, Result>,
        IAsyncRequestHandler<DeleleFiltersCommand, IEnumerable<Result>>,
        IAsyncRequestHandler<FilterBySportsQuery, IEnumerable<Sport>>,
        IAsyncRequestHandler<CheckUsernamesQuery, IEnumerable<MemberErrCode>>,
        IAsyncRequestHandler<GetMyWatchlistMembersQuery, IEnumerable<MyWatchlistMember>>
    {
        private readonly IUserSession userSession;
        private readonly MonitoringBetsService monitoringBetService;

        public MonitoringBetsHandler(
            IUserSession userSession,
            MonitoringBetsService monitoringBetService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            IIPInfoService ipInfoService) : base(builder, ticketHelper, monitorService, ipService, ipInfoService)
        {
            this.userSession = userSession;
            this.monitoringBetService = monitoringBetService;
        }

        public async Task<MonitoringBetsViewModel> Handle(MonitoringBetsQuery message)
        {
            var filters = (await monitoringBetService.GetSavedFilters(message.ViewerId, message.SubAccId)).ToList();

            var tempFilter = userSession.GetProperty<SavedFilter>(SessionKey.LastBetsMonitoringDefault);
            if (tempFilter != null)
            {
                filters.Insert(0, tempFilter);
            }

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
            => monitoringBetService.GetSports(message.GroupSport);

        public Task<IEnumerable<MemberErrCode>> Handle(CheckUsernamesQuery message)
            => monitoringBetService.CheckUsernames(message.ViewerId, message.Usernames);

        public Task<IEnumerable<MyWatchlistMember>> Handle(GetMyWatchlistMembersQuery message)
            => monitoringBetService.GetMyWatchlistMembers(message.ViewerId);

        public Task<Result> Handle(ApplyFilterCommand message)
        {
            var savedFilter = new SavedFilter(
                0,
                message.FilterName,
                true,
                message.BetType,
                message.Stake,
                null);
            userSession.SetProperty(SessionKey.LastBetsMonitoringDefault, savedFilter);

            return Task.FromResult(new Result());
        }

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
                message.MyWatchlist,
                message.FilterByMembers,
                message.LanguageKey,
                message.IP));
    }
}