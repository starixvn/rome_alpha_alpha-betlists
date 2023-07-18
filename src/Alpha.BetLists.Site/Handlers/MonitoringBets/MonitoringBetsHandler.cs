namespace Alpha.BetLists.Site.Handlers.MonitoringBets
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AppResources;
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
    using Alpha.BetLists.Params.LastBets;
    using Alpha.IPInfo.Services;
    using Alpha.SmartPunter.Services;
    using Alpha.Utilities.IP;
    using Alpha.Web.Models;

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
        IRequestHandler<CheckCanViewLastFilteringBetsQuery, Result>,
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
            if (filters.Any())
            {
                var defaultFilter = filters.FirstOrDefault(f => f.IsDefault) ?? filters.First();
                filterId = defaultFilter.FilterId;
                var memberList = defaultFilter.Members;

                if (defaultFilter.FilterByMembers == FilterByMembersConstants.AllWatchlistMembers)
                {
                    var custIds = (await monitoringBetService.GetMyWatchlistMembers(message.ViewerId)).Select(member => member.CustId);
                    memberList = $"{defaultFilter.Members},{string.Join(",", custIds)}";
                }

                var param = new GetLastFilteringBetsParams(message.ViewerId,
                    0,
                    memberList,
                    defaultFilter.SportTypes,
                    defaultFilter.Leagues,
                    defaultFilter.BetTypes,
                    defaultFilter.EventStatus,
                    defaultFilter.Stake,
                    limitRowCount: 10);
                var tickets = await monitoringBetService.GetLastFilteringBets(param);

                await AssignCountry(tickets, message.DisplayVNIP);
                betListData = new BetListData(tickets, null);
            }

            return new MonitoringBetsViewModel(message, filters, betListData, Builder, TicketHelper,
                encryptionService.CfsEncode($"LastTransId_{message.CustId}_{filterId}", numberOfEncodeResult: 6),
                encryptionService.CfsEncode($"LatestTransId_{message.CustId}_{filterId}", numberOfEncodeResult: 6));
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

        public async Task<FilteringBetDataViewModel> Handle(GetLastFilteringBetsQuery message)
        {
            IEnumerable<long> memberInWatchIds = (await monitoringBetService.GetMyWatchlistMembers(message.ViewerId)).Select(member => member.CustId);
            if (message.FilterByMembers == FilterByMembersConstants.AllWatchlistMembers)
            {
                message.MemberList = $"{message.MemberList},{string.Join(",", memberInWatchIds)}";
            }

            var param = new GetLastFilteringBetsParams(message.ViewerId,
                message.TransId == 0 ? 0 : message.TransId - 1,
                message.MemberList,
                message.SportTypeList,
                message.LeagueList,
                message.BetTypeList,
                message.EvenStatus,
                message.MinStake,
                message.LimitRowCount);

            var tickets = await monitoringBetService.GetLastFilteringBets(param);

            if (memberInWatchIds.Any())
            {
                foreach (var ticket in tickets)
                {
                    foreach (var memberId in memberInWatchIds)
                    {
                        if (ticket.CustId == memberId)
                        {
                            ticket.IsInWatchlist = true;
                        }
                    }

                }
            }

            await AssignCountry(tickets, message.DisplayVNIP);
            var betListData = new BetListData(tickets, null);

            var newTickets = tickets.Where(t => t.TransId > message.LatestTransId);

            if (newTickets.Any())
            {
                WriteLog(message.CustId, newTickets);
            }

            var latestTransId = newTickets.Any() ? newTickets.First().TransId : message.LatestTransId;

            return new FilteringBetDataViewModel(latestTransId,
                encryptionService.CfsEncode($"LastTransId_{message.CustId}_{message.FilterId}", numberOfEncodeResult: 6),
                encryptionService.CfsEncode($"LatestTransId_{message.CustId}_{message.FilterId}", numberOfEncodeResult: 6),
                message, betListData, Builder, TicketHelper);
        }

        public Result Handle(CheckCanViewLastFilteringBetsQuery message)
        {
            var canViewLastBets = lastBetsService.CanViewLastBets(
                new CheckCanViewLastBetsParam(
                    message.CustId,
                    message.IsInternal,
                    message.IsEnableLastBetsMonitoring,
                    message.UsersAlwaysAllowAccessBetsListMonitoring));

            var errorCode = canViewLastBets ? ErrorCodeConstants.None : ErrorCodeConstants.FunctionTurnOff;
            var errorMessage = canViewLastBets ? string.Empty : Resources.FunctionTurnOffError;

            return new Result(errorCode, errorMessage);
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