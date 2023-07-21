namespace Alpha.BetLists.Site.Handlers.MonitoringBets
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.BetLists.Constants;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Models.MonitoringBets;
    using Alpha.BetLists.Params.MonitoringBets;
    using Alpha.BetLists.Services;
    using Alpha.BetLists.Site.Constants;
    using Alpha.BetLists.Site.Requests.MonitoringBets;
    using Alpha.BetLists.Site.ViewModels.MonitoringBets;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using Alpha.IPInfo.Services;
    using Alpha.SmartPunter.Services;
    using Alpha.Utilities.IP;
    using Alpha.Web.Models;
    using System;

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
        IAsyncRequestHandler<GetMyWatchlistMembersQuery, IEnumerable<MyWatchlistMember>>,
        IAsyncRequestHandler<GetLastFilteringBetsQuery, FilteringBetDataViewModel>
    {
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
            var filters = (await monitoringBetService.GetSavedFilters(message.ViewerId, message.SubAccId)).ToList();
            var tempFilter = userSession.GetProperty<SavedFilter>(SessionKey.LastBetsMonitoringDefault);
            if (tempFilter != null)
            {
                filters.Insert(0, tempFilter);
            }

            var filterId = 0;
            BetListData betListData = null;
            long latestTransId = 0;
            if (filters.Any())
            {
                var defaultFilter = filters.FirstOrDefault(f => f.IsDefault) ?? filters.First();
                filterId = defaultFilter.FilterId;

                var result = await GetMemberList(defaultFilter.Members, message.ViewerId, defaultFilter.FilterByMembers);

                var memberList = result.Item1;
                var memberInWatchlistIds = result.Item2;

                var param = new GetLastFilteringBetsParams(message.ViewerId,
                    0,
                    memberList,
                    defaultFilter.SportTypes,
                    defaultFilter.Leagues,
                    defaultFilter.BetTypes,
                    defaultFilter.EventStatus,
                    defaultFilter.Stake,
                    limitRowCount: 100);

                var betData = await GetBetData(param, memberInWatchlistIds, message.DisplayVNIP, latestTransId, message.CustId);

                betListData = betData.Item1;
                latestTransId = betData.Item2;
            }

            return new MonitoringBetsViewModel(latestTransId, message, filters, betListData, Builder, TicketHelper,
                encryptionService.CfsEncode($"LastTransId_{message.CustId}_{filterId}", numberOfEncodeResult: 6),
                encryptionService.CfsEncode($"LatestTransId_{message.CustId}_{filterId}", numberOfEncodeResult: 6));
        }

        private async Task<Tuple<string, List<int>>> GetMemberList(string memberList, int viewerId, int filterByMembers)
        {
            var memberInWatchlistIds = (await monitoringBetService.GetMyWatchlistMembers(viewerId)).Select(member => member.CustId).ToList();
            if (filterByMembers == FilterByMembersConstants.AllWatchlistMembers)
            {
                memberList = $"{memberList},{string.Join(",", memberInWatchlistIds)}";
            }
            return Tuple.Create(memberList, memberInWatchlistIds);
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
            var savedFilter = monitoringBetService.BuildSavedFilter(new AddFilterParam(
                message.ViewerId,
                message.SubAccId,
                message.FilterName,
                message.IsDefault,
                message.BetType,
                message.Stake,
                message.Sports,
                message.LiveStatus,
                message.InputList,
                message.MyWatchlist,
                message.FilterByMembers,
                message.LanguageKey,
                message.IP));
            userSession.SetProperty(SessionKey.LastBetsMonitoringDefault, savedFilter);

            return Task.FromResult(new Result());
        }

        private void UpdateInWatchlistState(List<int> memberInWatchlistIds, IEnumerable<TicketDto> tickets)
        {
            if (memberInWatchlistIds.Any())
            {
                var memberInWatchlistDictionary = memberInWatchlistIds.ToDictionary(id => id, id => true);
                foreach (var ticket in tickets)
                {
                    if (memberInWatchlistDictionary.ContainsKey((int)ticket.CustId))
                    {
                        ticket.IsInWatchlist = memberInWatchlistDictionary[(int)ticket.CustId];
                    }
                }
            }
        }

        private async Task<Tuple<BetListData, long>> GetBetData(GetLastFilteringBetsParams param, List<int> memberInWatchlistIds, bool displayVNIP, long latestTransId, int custId)
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
                WriteLog(custId, newTickets);
            }

            latestTransId = newTickets.FirstOrDefault()?.TransId ?? latestTransId;

            return Tuple.Create(betListData, latestTransId);
        }

        public async Task<FilteringBetDataViewModel> Handle(GetLastFilteringBetsQuery message)
        {
            var result = await GetMemberList(message.MemberList, message.ViewerId, message.FilterByMembers);

            var memberList = result.Item1;
            var memberInWatchlistIds = result.Item2;

            var param = new GetLastFilteringBetsParams(message.ViewerId,
                message.TransId == 0 ? 0 : message.TransId - 1,
                memberList,
                message.SportTypeList,
                message.LeagueList,
                message.BetTypeList,
                message.EvenStatus,
                message.MinStake,
                message.LimitRowCount);

            var betData = await GetBetData(param, memberInWatchlistIds, message.DisplayVNIP, message.LatestTransId, message.CustId);

            var betListData = betData.Item1;
            var latestTransId = betData.Item2;

            return new FilteringBetDataViewModel(latestTransId,
                encryptionService.CfsEncode($"LastTransId_{message.CustId}_{message.FilterId}", numberOfEncodeResult: 6),
                encryptionService.CfsEncode($"LatestTransId_{message.CustId}_{message.FilterId}", numberOfEncodeResult: 6),
                message, betListData, Builder, TicketHelper);
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
                message.LiveStatus,
                message.InputList,
                message.MyWatchlist,
                message.FilterByMembers,
                message.LanguageKey,
                message.IP));
    }
}