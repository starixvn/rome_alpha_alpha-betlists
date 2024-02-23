namespace Alpha.BetLists.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.BetLists.AppResources;
    using Alpha.BetLists.Configurations;
    using Alpha.BetLists.Constants;
    using Alpha.BetLists.Criteria.MonitoringBets;
    using Alpha.BetLists.Extensions;
    using Alpha.BetLists.Helpers;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Models.MonitoringBets;
    using Alpha.BetLists.Params.MonitoringBets;
    using Alpha.Web.Models;
    using AlphaPlus.PartialUM.Services;
    using Fanex.Data.Repository;

    public interface MonitoringBetsService
    {
        Task<IEnumerable<SavedFilter>> GetSavedFilters(int custId, int subAccId, AddFilterParam unsavedFilter);

        Task<IEnumerable<FilterConfiguration>> GetFilters(int custId, int subAccId, AddFilterParam unsavedFilter);

        Task<Result> AddFilter(AddFilterParam param);

        Task<Result> UpdateFilter(UpdateFilterParam param);

        Task<Result> DeleteFilter(DeleteFilterParam param);

        Task<IEnumerable<Sport>> GetSports(byte sportGroup);

        Task<IEnumerable<MyWatchlistMember>> GetMyWatchlistMembers(int agentId);

        Task<IEnumerable<TicketDto>> GetLastFilteringBets(GetLastFilteringBetsParams param);
    }

    public class MonitoringBetsServiceImpl : MonitoringBetsService
    {
        private const string IgnoreSportTypeName = "upcoming";
        private static readonly HashSet<int> MajorSports = new HashSet<int>
        {
            SportTypeIdConstants.Soccer,
            SportTypeIdConstants.Basketball,
            SportTypeIdConstants.Badminton,
            SportTypeIdConstants.ESports,
            SportTypeIdConstants.Tennis
        };
        private static readonly HashSet<string> SpecialFilterNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Unsaved Filter",
            "未保存筛选",
            "未保存篩選",
            "ตัวกรองที่ไม่ได้บันทึก",
            "Bộ Lọc Chưa Lưu"
        };
        private readonly IPartialUMService partialUMService;
        private readonly IDynamicRepository dynamicRepository;
        private readonly LastBetsService lastBetsService;
        private readonly ParlayService parlayService;

        public MonitoringBetsServiceImpl(
            IPartialUMService partialUMService,
            LastBetsService lastBetsService,
            IDynamicRepository dynamicRepository,
            ParlayService parlayService)
        {
            this.partialUMService = partialUMService;
            this.lastBetsService = lastBetsService;
            this.dynamicRepository = dynamicRepository;
            this.parlayService = parlayService;
        }

        public async Task<IEnumerable<SavedFilter>> GetSavedFilters(int custId, int subAccId, AddFilterParam unsavedFilter)
        {
            var isEnableNotifications = await IsEnableChatBotNotifications(custId, subAccId);

            var filterConfiguration = (await GetFilterConfiguration(custId, subAccId))
                .Select(f =>
                {
                    var filterXml = f.FilterXml;
                    var (sportTypes, leagues) = BuildSportTypesAndLeagues(filterXml.Sports);

                    return new SavedFilter(
                        f.FilterId,
                        f.FilterName,
                        f.IsDefault,
                        isEnableNotifications && f.IsNotify,
                        filterXml.BetType,
                        filterXml.Stake,
                        filterXml.LiveStatus,
                        sportTypes,
                        leagues,
                        BuildMembers(filterXml.FilterByMembers, filterXml.Members, filterXml.MyWatchlistMembers),
                        filterXml.FilterByMembers);
                })
                .ToList();

            if (unsavedFilter != null)
            {
                filterConfiguration.Insert(0, BuildUnsavedFilter(unsavedFilter));
            }

            return filterConfiguration;
        }

        private static SavedFilter BuildUnsavedFilter(AddFilterParam customFilter)
        {
            var (sportTypes, leagues) = BuildSportTypesAndLeagues(customFilter.Sports);

            return new SavedFilter(
                0,
                Resources.UnsavedFilter,
                false,
                customFilter.IsNotify,
                customFilter.BetType,
                customFilter.Stake,
                customFilter.LiveStatus,
                sportTypes,
                leagues,
                BuildMembers(customFilter.FilterByMembers, customFilter.InputList, customFilter.MyWatchlist),
                customFilter.FilterByMembers);
        }

        private static Tuple<string, string> BuildSportTypesAndLeagues(IEnumerable<SportXml> sportXmlList)
        {
            if (sportXmlList.Any(
                s => s.Sport.Equals("0", StringComparison.OrdinalIgnoreCase)
                    && s.League.Equals("0", StringComparison.OrdinalIgnoreCase)))
            {
                return Tuple.Create("0", "0");
            }

            var sportTypes = new HashSet<string>();
            var leagues = new HashSet<string>();

            foreach (var item in sportXmlList)
            {
                foreach (var sport in item.Sport.SplitByCommaWithNoneEmptyEntries())
                {
                    if (item.League.Equals("0", StringComparison.OrdinalIgnoreCase))
                    {
                        sportTypes.Add(sport);
                    }
                    else
                    {
                        foreach (var league in item.League.SplitByCommaWithNoneEmptyEntries())
                        {
                            leagues.Add(league);
                        }
                    }
                }
            }

            return Tuple.Create(string.Join(",", sportTypes), string.Join(",", leagues));
        }

        private static string BuildMembers(byte filterByMembers, string members, string myWatchlistMembers)
        {
            if (filterByMembers == FilterByMembersConstants.AllMembers)
            {
                return members;
            }

            if (filterByMembers == FilterByMembersConstants.AllWatchlistMembers)
            {
                return members;
            }

            return $"{members},{myWatchlistMembers}".Trim(',');
        }

        public async Task<IEnumerable<FilterConfiguration>> GetFilters(int custId, int subAccId, AddFilterParam unsavedFilter)
        {
            var isEnableNotifications = await IsEnableChatBotNotifications(custId, subAccId);
            var filterConfiguration = (await GetFilterConfiguration(custId, subAccId))
                .Select(item =>
                {
                    var filterXml = item.FilterXml;
                    filterXml.BetTypeNames = BuildBetTypeNames(filterXml.BetType);
                    filterXml.FlatternSports = BuildSports(FlattenSports(filterXml.Sports)).ToList();

                    item.IsNotify = isEnableNotifications && item.IsNotify;

                    return item;
                })
                .ToList();

            if (unsavedFilter != null)
            {
                filterConfiguration.Insert(0, BuildUnsavedFilterConfiguration(unsavedFilter));
            }

            return filterConfiguration;
        }

        private async Task<IEnumerable<FilterConfiguration>> GetFilterConfiguration(int custId, int subAccId)
        {
            var criteria = new GetFiltersCriteria(custId, subAccId);
            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            return (await dynamicRepository.FetchAsync<FilterConfiguration>(criteria)).OrderByDescending(f => f.FilterId);
        }

        private static FilterConfiguration BuildUnsavedFilterConfiguration(AddFilterParam param)
        {
            var filterXml = new FilterXml
            {
                BetType = param.BetType,
                Stake = param.Stake,
                Members = param.InputList,
                MyWatchlistMembers = param.MyWatchlist,
                FilterByMembers = param.FilterByMembers,
                LiveStatus = param.LiveStatus,
                Sports = param.Sports.ToList()
            };

            filterXml.BetTypeNames = BuildBetTypeNames(filterXml.BetType);
            filterXml.FlatternSports = BuildSports(FlattenSports(filterXml.Sports)).ToList();

            return new FilterConfiguration(
                param.FilterId,
                Resources.UnsavedFilter,
                string.Empty,
                filterXml,
                param.IsDefault,
                param.IsNotify,
                DateTime.MinValue,
                param.MyWatchlistMembers,
                param.InputListMembers);
        }

        private static IEnumerable<string> BuildBetTypeNames(string betTypes)
            => betTypes.Equals("0", StringComparison.OrdinalIgnoreCase)
            ? Enumerable.Empty<string>()
            : betTypes.ToListInt().Select(t => BetTypeHelper.GetBetTypeName(t)).ToList();

        private static IEnumerable<League> FlattenSports(IEnumerable<SportXml> sportXmlList)
        {
            foreach (var item in sportXmlList)
            {
                if (!item.Sport.Equals("0", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var sport in item.Sport.SplitByCommaWithNoneEmptyEntries())
                    {
                        foreach (var league in item.League.SplitByCommaWithNoneEmptyEntries())
                        {
                            yield return new League(Convert.ToInt32(sport), Convert.ToInt32(league));
                        }
                    }
                }
            }
        }

        private static IEnumerable<Sport> BuildSports(IEnumerable<League> leagueList)
        {
            var groupBySportTypes = leagueList.GroupBy(l => l.SportType).OrderBy(l => l.Key);

            foreach (var group in groupBySportTypes)
            {
                var sportTypeName = CacheRefServiceHelper.GetSportName(group.Key);
                if (sportTypeName.IndexOf(IgnoreSportTypeName, StringComparison.OrdinalIgnoreCase) == -1)
                {
                    yield return new Sport(
                        group.Key,
                        sportTypeName,
                        BuildLeagues(group.Key, group).OrderBy(l => l.LeagueName).ToList());
                }
            }
        }

        private static IEnumerable<League> BuildLeagues(int sportType, IEnumerable<League> leagueList)
            => leagueList.GroupBy(
                l => l.LeagueId,
                (leagueId, matches) => new League(
                    sportType,
                    leagueId,
                    0,
                    leagueId == 0 ? Resources.AllLeagues :
                    CacheRefServiceHelper.GetLeagueName(leagueId, matches.FirstOrDefault(i => i.LeagueId == leagueId).LeagueGroupId)));

        public async Task<Result> AddFilter(AddFilterParam param)
        {
            if (SpecialFilterNames.Contains(param.FilterName))
            {
                const int AlreadyExistsErrorCode = 90020;
                return new Result(AlreadyExistsErrorCode, Resources.FilterNameAlreadyExists);
            }

            if (param.IsNotify && !(await IsEnableChatBotNotifications(param.CustId, param.SubAccId)))
            {
                const int EnableChatBotFunctionErrorCode = 90021;
                return new Result(EnableChatBotFunctionErrorCode, Resources.EnableChatBot_LastBets);
            }

            var command = new AddFilterCommand(param);
            partialUMService.ThrowIfUM(command.GetSettingKey());

            await dynamicRepository.ExecuteAsync(command);
            return new Result(
                command.ErrCode,
                command.ErrCode == 0 ? Resources.AddedSuccessfully : command.ErrMsg);
        }

        public async Task<Result> UpdateFilter(UpdateFilterParam param)
        {
            if (param.IsNotify && !(await IsEnableChatBotNotifications(param.CustId, param.SubAccId)))
            {
                const int EnableChatBotFunctionErrorCode = 90021;
                return new Result(EnableChatBotFunctionErrorCode, Resources.EnableChatBot_LastBets);
            }

            var command = new UpdateFilterCommand(param);
            partialUMService.ThrowIfUM(command.GetSettingKey());

            await dynamicRepository.ExecuteAsync(command);
            return new Result(
                command.ErrCode,
                command.ErrCode == 0 ? Resources.UpdateSuccessfully : command.ErrMsg);
        }

        public async Task<Result> DeleteFilter(DeleteFilterParam param)
        {
            var command = new DeleteFilterCommand(param);
            partialUMService.ThrowIfUM(command.GetSettingKey());

            await dynamicRepository.ExecuteAsync(command);

            return new Result(command.ErrCode, command.ErrMsg);
        }

        public async Task<IEnumerable<Sport>> GetSports(byte sportGroup)
        {
            var criteria = new GetLeaguesCriteria();
            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            var leagues = await dynamicRepository.FetchAsync<League>(criteria);
            leagues = SiteGroup777DontSupportPinGoal(leagues);
            if (sportGroup != SportGroupConstants.All)
            {
                leagues = sportGroup == SportGroupConstants.MajorSports
                    ? leagues.Where(m => MajorSports.Contains(m.SportType))
                    : leagues.Where(m => !MajorSports.Contains(m.SportType));
            }

            var sports = BuildSports(leagues.ToList()).ToList();
            return sports;
        }

        public Task<IEnumerable<MyWatchlistMember>> GetMyWatchlistMembers(int agentId)
            => dynamicRepository.FetchAsync<MyWatchlistMember>(new GetMonitoredWatchlistMembersCriteria(agentId));

        public async Task<IEnumerable<TicketDto>> GetLastFilteringBets(GetLastFilteringBetsParams param)
        {
            var tickets = await lastBetsService.GetLastBets(
                new GetLastFilteringBetsCriteria(
                    param.CustId,
                    param.TransId,
                    param.MemberList,
                    param.SportTypeList,
                    param.LeagueList,
                    param.BetTypeList,
                    param.LiveIndicator,
                    param.MinStake,
                    param.LimitRowCount),
                AppSettings.RunningMatchCacheTime);

            await parlayService.UpdateOddsMixParlay(tickets);

            return tickets;
        }

        private async Task<bool> IsEnableChatBotNotifications(int custId, int subAccId)
        {
            var command = new CheckEnableChatBotNotificationsCommand(custId, subAccId);
            await dynamicRepository.ExecuteAsync(command);

            return command.Status;
        }

        private IEnumerable<League> SiteGroup777DontSupportPinGoal(IEnumerable<League> leagues)
        {
            var leagueGroupPinGoal = new List<int> { 113, 122, 151 };

            return AlphaPlus.Configurations.AppSettings.IsSiteLabel777 ?
            leagues.Where(i => !leagueGroupPinGoal.Contains(i.LeagueGroupId)) :
            leagues;
        }

    }
}
