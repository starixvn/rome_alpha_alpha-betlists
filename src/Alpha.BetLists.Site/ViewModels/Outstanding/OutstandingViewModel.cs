namespace Alpha.BetLists.Site.ViewModels.Outstanding
{
    using AppResources;
    using BetLists.Models;
    using Components;
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Params;
    using Requests.Outstanding;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Utils;

    public class OutstandingViewModel : BetListBaseViewModel
    {
        private const string StatementProductIds = "1,6,2";
        private readonly OutstandingQuery query;

        public OutstandingViewModel(
            OutstandingQuery query,
            OutstandingParam param,
            BetListData betListData,
            IEnumerable<SelectListItem> products,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            bool showMemberName = false)
            : base(query, betListData, builder, ticketHelper, showMemberName)
        {
            this.query = query;

            Title = $"{Resources.BetlistRunning} - {query.CustName}";

            Products = products;
            ProductsId = param.ProductIds.JoinWithComma();
            FromDate = param.FromDate;
            ToDate = param.ToDate;
            CustId = param.CustId;
            CustName = param.UserName;

            ViewDataInstructionMessage = string.Format(
                Resources.CanOnlyViewDataAfterTimeHistoryReport,
                query.MinDate.ToShortDateWithSlash());
        }

        public bool GetAll => query.GetAll;

        public bool AllowToViewSuperPT => baseQuery.ViewerLevel.IsSuperLevel();

        public bool AllowToViewMasterPT => baseQuery.ViewerLevel.IsMasterOrSuperLevel();

        public string ViewDataInstructionMessage { get; }

        public bool IsFilterVisible => query.IsFilterVisible;

        public IEnumerable<SelectListItem> Products { get; }

        public DateRangePickerViewModel DateRangePickerViewModel =>
            new DateRangePickerViewModel(
                query.MinDate,
                DateTime.MaxValue,
                FromDate,
                ToDate,
                query.UserSelectedDateRangeOption);

        public ProductMultiSelectionViewModel ProductMultiSelectionViewModel =>
            new ProductMultiSelectionViewModel(
                Products,
                ProductsId,
                query.Language);

        public string BuildBetListLink()
        {
            var url = Helpers.UrlHelper.MapExMainPath("~/_Reports/BetList/OutstandingDetail.aspx");
            var routeValues = new Dictionary<string, object>
            {
                ["username"] = CustName,
                ["option"] = 4,
                ["custid"] = CustId,
                ["fdate"] = SystemTime.Today.AddDays(-1).ToShortDateWithSlash(),
                ["tdate"] = SystemTime.Today.ToShortDateWithSlash(),
                ["page"] = 1,
                ["chkgetall"] = "on",
                ["productIds"] = StatementProductIds,
                ["FilterPostback"] = "1"
            };

            return Helpers.UrlHelper.AddQueryStringParamsFunc(url, routeValues);
        }

        public string BuildDailyStatementLink()
        {
            var url = Helpers.UrlHelper.MapExMainPath("~/Finance/StatementMember");
            var routeValues = new Dictionary<string, object>
            {
                ["CustId"] = CustId,
                ["username"] = CustName,
                ["RoleId"] = 1,
                ["productIds"] = StatementProductIds,
                ["FilterPostback"] = "1"
            };

            return Helpers.UrlHelper.AddQueryStringParamsFunc(url, routeValues);
        }
    }
}