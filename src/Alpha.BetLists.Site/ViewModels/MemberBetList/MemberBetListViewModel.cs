namespace Alpha.BetLists.Site.ViewModels.MemberBetList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AppResources;
    using BetLists.Models;
    using Components;
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Params;
    using Requests.MemberBetList;
    using Utils;
    using WinLoss;

    public class MemberBetListViewModel : WinLossViewModel
    {
        private static readonly string[] ExcludingTicketStatus =
        {
            BetStatus.Running,
            BetStatus.Waiting,
            BetStatus.Void,
            BetStatus.Reject,
            BetStatus.Pending
        };

        public static readonly IEnumerable<string> RangeOptions = new List<string>
        {
            DateRangePickerViewModel.TodayOptionValue,
            DateRangePickerViewModel.YesterdayOptionValue,
            DateRangePickerViewModel.SpecificDateOptionValue
        };

        private readonly MemberBetListQuery query;

        public MemberBetListViewModel(
            MemberBetListQuery query,
            MemberBetListParam param,
            BetListData betListData,
            IEnumerable<GroupProductViewModel> groupProducts,
            IEnumerable<int> supportScoreMapBetTypeIds,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper)
            : base(query, betListData, builder, ticketHelper, query.IsEnabledJackpot, false, ExcludingTicketStatus)
        {
            this.query = query;

            GroupProducts = groupProducts;
            SupportScoreMapBetTypeIds = supportScoreMapBetTypeIds;

            FromDate = param.FromDate;
            ToDate = param.ToDate;
            CustId = param.CustId;
            CustName = param.UserName;

            Title = $"{Resources.BetListHistory} - {query.CustName}";
            ProductsId = param.ProductIds.JoinWithComma();
            ViewDataInstructionMessage = string.Format(
                Resources.CanOnlyViewDataAfterTimeHistoryReport,
                query.MinDate.ToShortDateWithSlash());
        }

        public string ViewDataInstructionMessage { get; }

        public IEnumerable<int> SupportScoreMapBetTypeIds { get; }

        public IEnumerable<GroupProductViewModel> GroupProducts { get; }

        public bool IsFilterVisible => query.IsFilterVisible;

        public DateRangePickerViewModel DateRangePickerViewModel
            => new DateRangePickerViewModel(
                query.MinDate,
                DateTime.MaxValue,
                FromDate,
                ToDate,
                query.UserSelectedDateRangeOption,
                true,
                RangeOptions);

        public ProductMultiSelectionViewModel ProductMultiSelectionViewModel
            => new ProductMultiSelectionViewModel(
                GroupProducts,
                ProductsId,
                query.Language);

        public static string GetDailyStatementLink(int custId, string custName, string productIds)
        {
            var url = Helpers.UrlHelper.MapReportsPath("~/Statement/MemberStatement");
            var routeValues = new Dictionary<string, object>
            {
                ["CustId"] = custId,
                ["CustName"] = custName,
                ["CustLevelId"] = 1,
                ["ProductIds"] = productIds
            };

            return Helpers.UrlHelper.AddQueryStringParamsFunc(url, routeValues);
        }

        public override bool CanViewScoreMap(ITicket ticket)
            => ticket.Status.Equals(BetStatus.Running, StringComparison.OrdinalIgnoreCase)
            && SupportScoreMapBetTypeIds.Contains(ticket.BetTypeId);

        public string BuildRunningBetListLink(string link)
        {
            var routeValues = new Dictionary<string, object>
            {
                ["CustId"] = CustId,
                ["CustName"] = CustName,
                ["FromDate"] = SystemTime.Today.AddDays(-7).ToShortDateWithSlash(),
                ["ToDate"] = SystemTime.Today.ToShortDateWithSlash(),
                ["ProductIds"] = ProductsId
            };

            return Helpers.UrlHelper.AddQueryStringParamsFunc(link, routeValues);
        }
    }
}
