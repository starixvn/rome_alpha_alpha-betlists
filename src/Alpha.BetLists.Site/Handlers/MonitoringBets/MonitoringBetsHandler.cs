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
        IAsyncRequestHandler<ManageFiltersQuery, ManageFiltersViewModel>,
        IAsyncRequestHandler<ApplyFilterCommand, Result>,
        IAsyncRequestHandler<AddUnsavedFilterCommand, Result>,
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
        private readonly EncryptionService encryptionService;

        public MonitoringBetsHandler(
            IUserSession userSession,
            MonitoringBetsService monitoringBetService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            EncryptionService encryptionService,
            IIPInfoService ipInfoService) : base(builder, ticketHelper, monitorService, ipService, ipInfoService)
        {
            this.userSession = userSession;
            this.monitoringBetService = monitoringBetService;
            this.encryptionService = encryptionService;
        }

        public async Task<MonitoringBetsViewModel> Handle(MonitoringBetsQuery message)
        {
            var filters = await monitoringBetService.GetSavedFilters(
                message.ViewerId,
                message.SubAccId,
                GetUnsavedFilterParamSession(message.ViewerId, message.SubAccId));

            var filterId = -1;
            var isShowFilterPlaceHolder = true;
            long latestTransId = 0;
            BetListData betListData = null;

            if (filters.Any())
            {
                var defaultFilter = GetDefaultFilter(filters, message.FilterName);
                if (defaultFilter != null)
                {
                    filterId = defaultFilter.FilterId;
                    isShowFilterPlaceHolder = false;

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
                        message.LimitRowCount);

                    var betData = await GetBetData(param, memberInWatchlistIds, message.DisplayVNIP, latestTransId);

                    betListData = betData.Item1;
                    latestTransId = betData.Item2;
                }
            }

            return new MonitoringBetsViewModel(
                filterId,
                filters,
                isShowFilterPlaceHolder,
                latestTransId,
                LastTransIdCacheKeyByFilter(message.ViewerId, filterId),
                LatestTransIdCacheKeyByFilter(message.ViewerId, filterId),
                message,
                betListData,
                Builder,
                TicketHelper);
        }

        private static SavedFilter GetDefaultFilter(IEnumerable<SavedFilter> filters, string filterName)
        {
            var defaultFilter = string.IsNullOrWhiteSpace(filterName)
                ? null
                : filters.FirstOrDefault(f => f.FilterName.Equals(filterName, StringComparison.OrdinalIgnoreCase));

            return defaultFilter ?? filters.FirstOrDefault(f => f.IsDefault);
        }

        public async Task<ManageFiltersViewModel> Handle(ManageFiltersQuery message)
            => new ManageFiltersViewModel(
                await monitoringBetService.GetFilters(
                    message.ViewerId,
                    message.SubAccId,
                    GetUnsavedFilterParamSession(message.ViewerId, message.SubAccId)),
                message);

        public Task<Result> Handle(UpdateFilterCommand message)
            => monitoringBetService.UpdateFilter(new UpdateFilterParam(
                message.ViewerId,
                message.SubAccId,
                message.FilterId,
                message.FilterName,
                message.IsDefault,
                message.LanguageKey,
                message.IP));

        public async Task<IEnumerable<Result>> Handle(DeleteFiltersCommand message)
        {
            var results = new List<Result>();
            var filterIds = message.Ids.ToListInt();

            foreach (var id in filterIds)
            {
                if (id == 0)
                {
                    SetUnsavedFilterParamSession(message.ViewerId, message.SubAccId, null);
                    results.Add(new Result());
                }
                else
                {
                    results.Add(await monitoringBetService.DeleteFilter(
                        new DeleteFilterParam(message.ViewerId, message.SubAccId, id, message.LanguageKey, message.IP)));
                }
            }

            return results;
        }

        public Task<IEnumerable<Sport>> Handle(FilterBySportsQuery message)
            => monitoringBetService.GetSports(message.GroupSport);

        public Task<IEnumerable<MyWatchlistMember>> Handle(GetMyWatchlistMembersQuery message)
            => monitoringBetService.GetMyWatchlistMembers(message.ViewerId);

        public Task<Result> Handle(ApplyFilterCommand message)
        {
            SetUnsavedFilterParamSession(message.ViewerId, message.SubAccId, BuildAddFilterParam(message));
            return Task.FromResult(new Result());
        }

        public async Task<Result> Handle(AddUnsavedFilterCommand message)
        {
            var unsavedFilterParam = GetUnsavedFilterParamSession(message.ViewerId, message.SubAccId);

            if (unsavedFilterParam == null)
            {
                return new Result();
            }

            var result = await monitoringBetService.AddFilter(new AddFilterParam(
                message.ViewerId,
                message.SubAccId,
                message.FilterName,
                message.IsDefault,
                unsavedFilterParam.BetType,
                unsavedFilterParam.Stake,
                unsavedFilterParam.Sports,
                unsavedFilterParam.LiveStatus,
                unsavedFilterParam.InputList,
                unsavedFilterParam.InputListMembers,
                unsavedFilterParam.MyWatchlist,
                unsavedFilterParam.MyWatchlistMembers,
                unsavedFilterParam.FilterByMembers,
                message.LanguageKey,
                unsavedFilterParam.IP
            ));

            if (result.ErrCode == 0)
            {
                SetUnsavedFilterParamSession(message.ViewerId, message.SubAccId, null);
            }

            return result;
        }

        public Task<Result> Handle(AddFilterCommand message)
            => monitoringBetService.AddFilter(BuildAddFilterParam(message));

        private static AddFilterParam BuildAddFilterParam(AddFilterCommand message)
            => new AddFilterParam(
                message.ViewerId,
                message.SubAccId,
                message.FilterName,
                message.IsDefault,
                CheckIdList(message.BetType),
                message.Stake,
                message.Sports,
                message.LiveStatus,
                CheckIdList(message.InputList),
                message.InputListMembers,
                CheckIdList(message.MyWatchlist),
                message.MyWatchlistMembers,
                message.FilterByMembers,
                message.LanguageKey,
                message.IP);

        private static string CheckIdList(string idList)
            => string.IsNullOrWhiteSpace(idList) || idList == "0"
            ? idList
            : idList.RemoveDuplicate();

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
                LastTransIdCacheKeyByFilter(message.ViewerId, message.FilterId),
                LatestTransIdCacheKeyByFilter(message.ViewerId, message.FilterId),
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
            AssignBetTypeNameAndMyWatchlistIcon(memberInWatchlistIds, tickets);

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

        private void AssignBetTypeNameAndMyWatchlistIcon(IEnumerable<int> memberInWatchlistIds, IEnumerable<TicketDto> tickets)
        {
            var memberInWatchlistHashSet = new HashSet<int>(memberInWatchlistIds ?? Enumerable.Empty<int>());

            foreach (var ticket in tickets)
            {
                ticket.BetType = TicketHelper.GetBetTypeNameById(ticket.BetTypeId);
                ticket.IsInWatchlist = memberInWatchlistHashSet.Contains((int)ticket.CustId);
            }
        }

        private string LastTransIdCacheKeyByFilter(int viewerId, int filterId)
            => encryptionService.CfsEncode($"LastTransId_{viewerId}_{filterId}", NumberOfEncodeResult);

        private string LatestTransIdCacheKeyByFilter(int viewerId, int filterId)
            => encryptionService.CfsEncode($"LatestTransId_{viewerId}_{filterId}", NumberOfEncodeResult);

        private AddFilterParam GetUnsavedFilterParamSession(int custId, int subAccId)
            => userSession.GetProperty<AddFilterParam>(UnsavedFilterSessionKey(custId, subAccId));

        private void SetUnsavedFilterParamSession(int custId, int subAccId, AddFilterParam value)
            => userSession.SetProperty(UnsavedFilterSessionKey(custId, subAccId), value);

        private static string UnsavedFilterSessionKey(int custId, int subAccId)
            => string.Format(SessionKey.LastBetsMonitoringUnsavedFilter, custId, subAccId);
    }
}