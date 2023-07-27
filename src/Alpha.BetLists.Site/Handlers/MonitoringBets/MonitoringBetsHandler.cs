namespace Alpha.BetLists.Site.Handlers.MonitoringBets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.BetLists.Constants;
    using Alpha.BetLists.Extensions;
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
        IAsyncRequestHandler<DeleteFiltersCommand, IEnumerable<Result>>,
        IAsyncRequestHandler<FilterBySportsQuery, IEnumerable<Sport>>,
        IAsyncRequestHandler<GetMyWatchlistMembersQuery, IEnumerable<MyWatchlistMember>>,
        IAsyncRequestHandler<GetLastFilteringBetsQuery, FilteringBetDataViewModel>
    {
        private const int NumberOfEncodeResult = 6;

        private readonly IUserSession userSession;
        private readonly MonitoringBetsService monitoringBetService;
        private readonly LastBetsService lastBetsService;
        private readonly EncryptionService encryptionService;

        public MonitoringBetsHandler(
            IUserSession userSession,
            MonitoringBetsService monitoringBetService,
            LastBetsService lastBetsService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            EncryptionService encryptionService,
            IIPInfoService ipInfoService) : base(builder, ticketHelper, monitorService, ipService, ipInfoService)
        {
            this.userSession = userSession;
            this.monitoringBetService = monitoringBetService;
            this.lastBetsService = lastBetsService;
            this.encryptionService = encryptionService;
        }

        public async Task<MonitoringBetsViewModel> Handle(MonitoringBetsQuery message)
        {
            var filters = await GetSavedFilters(message.ViewerId, message.SubAccId);

            var filterId = 0;
            BetListData betListData = null;
            long latestTransId = 0;
            if (filters.Any())
            {
                var defaultFilter = GetDefaultFilter(filters, message.FilterName);
                filterId = defaultFilter.FilterId;

                var result = await GetMemberList(defaultFilter.Members, message.ViewerId, defaultFilter.FilterByMembers);
                var memberList = result.Item1;
                var memberInWatchlistIds = result.Item2;

                var param = new GetLastFilteringBetsParams(
                    message.ViewerId,
                    0,
                    memberList,
                    defaultFilter.SportTypes,
                    defaultFilter.Leagues,
                    defaultFilter.BetTypes,
                    defaultFilter.EventStatus,
                    defaultFilter.Stake,
                    limitRowCount: 100);

                var betData = await GetBetData(param, memberInWatchlistIds, message.DisplayVNIP, latestTransId);

                betListData = betData.Item1;
                latestTransId = betData.Item2;
            }

            return new MonitoringBetsViewModel(
                filterId,
                filters,
                latestTransId,
                encryptionService.CfsEncode($"LastTransId_{message.ViewerId}_{filterId}", NumberOfEncodeResult),
                encryptionService.CfsEncode($"LatestTransId_{message.ViewerId}_{filterId}", NumberOfEncodeResult),
                message,
                betListData,
                Builder,
                TicketHelper);
        }

        private async Task<IEnumerable<SavedFilter>> GetSavedFilters(int viewerId, int subAccId)
        {
            var filters = (await monitoringBetService.GetSavedFilters(viewerId, subAccId)).ToList();
            var customFilter = userSession.GetProperty<SavedFilter>(SessionKey.LastBetsMonitoringCustomFilter);

            if (customFilter != null)
            {
                customFilter.FilterName = AppResources.Resources.UnsavedFilter;
                filters.Insert(0, customFilter);
            }

            return filters;
        }

        private static SavedFilter GetDefaultFilter(IEnumerable<SavedFilter> filters, string filterName)
        {
            var defaultFilter = string.IsNullOrWhiteSpace(filterName)
                ? filters.FirstOrDefault(f => f.IsDefault)
                : filters.FirstOrDefault(f => f.FilterName.Equals(filterName, StringComparison.OrdinalIgnoreCase));

            return defaultFilter ?? filters.First();
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

        public Task<IEnumerable<Result>> Handle(DeleteFiltersCommand message)
            => monitoringBetService.DeleteFilters(
                new DeleteFiltersParam(message.ViewerId, message.SubAccId, message.Ids, message.IP));

        public Task<IEnumerable<Sport>> Handle(FilterBySportsQuery message)
            => monitoringBetService.GetSports(message.GroupSport);

        public Task<IEnumerable<MyWatchlistMember>> Handle(GetMyWatchlistMembersQuery message)
            => monitoringBetService.GetMyWatchlistMembers(message.ViewerId);

        public Task<Result> Handle(ApplyFilterCommand message)
        {
            var savedFilter = monitoringBetService.BuildSavedFilter(new AddFilterParam(
                message.ViewerId,
                message.SubAccId,
                message.FilterName,
                message.IsDefault,
                message.BetType.RemoveDuplicate(),
                message.Stake,
                message.Sports,
                message.LiveStatus,
                message.InputList.RemoveDuplicate(),
                message.MyWatchlist.RemoveDuplicate(),
                message.FilterByMembers,
                message.LanguageKey,
                message.IP));
            userSession.SetProperty(SessionKey.LastBetsMonitoringCustomFilter, savedFilter);

            return Task.FromResult(new Result());
        }

        public Task<Result> Handle(AddFilterCommand message)
            => monitoringBetService.AddFilter(new AddFilterParam(
                message.ViewerId,
                message.SubAccId,
                message.FilterName,
                message.IsDefault,
                message.BetType.RemoveDuplicate(),
                message.Stake,
                message.Sports,
                message.LiveStatus,
                message.InputList.RemoveDuplicate(),
                message.MyWatchlist.RemoveDuplicate(),
                message.FilterByMembers,
                message.LanguageKey,
                message.IP));

        public async Task<FilteringBetDataViewModel> Handle(GetLastFilteringBetsQuery message)
        {
            var result = await GetMemberList(message.MemberList, message.ViewerId, message.FilterByMembers);

            var memberList = result.Item1;
            var memberInWatchlistIds = result.Item2;

            var param = new GetLastFilteringBetsParams(
                message.ViewerId,
                message.TransId == 0 ? 0 : message.TransId - 1,
                memberList,
                message.SportTypeList,
                message.LeagueList,
                message.BetTypeList,
                message.EvenStatus,
                message.MinStake,
                message.LimitRowCount);

            var betData = await GetBetData(param, memberInWatchlistIds, message.DisplayVNIP, message.LatestTransId);

            var betListData = betData.Item1;
            var latestTransId = betData.Item2;

            return new FilteringBetDataViewModel(
                latestTransId,
                encryptionService.CfsEncode($"LastTransId_{message.ViewerId}_{message.FilterId}", NumberOfEncodeResult),
                encryptionService.CfsEncode($"LatestTransId_{message.ViewerId}_{message.FilterId}", NumberOfEncodeResult),
                message,
                betListData,
                Builder,
                TicketHelper);
        }

        private async Task<Tuple<string, IEnumerable<int>>> GetMemberList(string memberList, int agentId, byte filterByMembers)
        {
            var memberInWatchlistIds = (await monitoringBetService.GetMyWatchlistMembers(agentId)).Select(member => member.CustId).ToList();
            if (filterByMembers == FilterByMembersConstants.AllWatchlistMembers)
            {
                memberList = $"{memberList},{string.Join(",", memberInWatchlistIds)}".TrimStart(',');
            }

            return Tuple.Create<string, IEnumerable<int>>(memberList, memberInWatchlistIds);
        }

        private async Task<Tuple<BetListData, long>> GetBetData(
            GetLastFilteringBetsParams param,
            IEnumerable<int> memberInWatchlistIds,
            bool displayVNIP,
            long latestTransId)
        {
            var tickets = await monitoringBetService.GetLastFilteringBets(param);

            UpdateInWatchlistState(memberInWatchlistIds, tickets);

            await AssignCountry(tickets, displayVNIP);
            var betListData = new BetListData(tickets, null);

            var isTransIdReset = latestTransId == 0;

            var previousLatestTransId = isTransIdReset ? 0 : latestTransId;

            var newTickets = tickets.Where(t => t.TransId > previousLatestTransId);

            if (newTickets.Any())
            {
                if (!isTransIdReset)
                {
                    foreach (var ticket in newTickets)
                    {
                        ticket.IsHighlight = true;
                    }
                }
                WriteLog(param.CustId, newTickets);
            }

            latestTransId = newTickets.FirstOrDefault()?.TransId ?? latestTransId;

            return Tuple.Create(betListData, latestTransId);
        }

        private void UpdateInWatchlistState(IEnumerable<int> memberInWatchlistIds, IEnumerable<TicketDto> tickets)
        {
            if (memberInWatchlistIds.Any())
            {
                var memberInWatchlistHashSet = new HashSet<int>(memberInWatchlistIds);

                foreach (var ticket in tickets)
                {
                    ticket.IsInWatchlist = memberInWatchlistHashSet.Contains((int)ticket.CustId);
                }
            }
        }
    }
}