namespace Alpha.BetLists.Site.ViewModels.MemberBetList
{
    using AppResources;
    using BetLists.Models;
    using Components;
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Params;
    using Requests.MemberBetList;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class MemberBeListViewModel : BetListBaseViewModel
    {
        private readonly MemberBetListQuery query;

        public MemberBeListViewModel(
            MemberBetListQuery query,
            MemberBetListParam param,
            BetListData betListData,
            IEnumerable<SelectListItem> products,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper)
            : base(query, betListData, builder, ticketHelper, false)
        {
            this.query = query;

            Products = products;
            FromDate = param.FromDate;
            ToDate = param.ToDate;
            CustId = param.CustId;
            CustName = param.UserName;

            Title = $"{Resources.BetList} - {query.CustName}";
            ViewDataInstructionMessage = string.Format(
                Resources.CanOnlyViewDataAfterTimeHistoryReport,
                query.MinDate.ToShortDateWithSlash());
        }

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

        public string BuildRunningBetListLink(string link)
        {
            var routeValues = new Dictionary<string, object>
            {
                ["CustId"] = CustId,
                ["CustName"] = CustName,
                ["FromDate"] = DateTime.Today.AddDays(-7).ToShortDateWithSlash(),
                ["ToDate"] = DateTime.Today.ToShortDateWithSlash(),
                ["ProductIds"] = ProductsId
            };

            return Helpers.UrlHelper.AddQueryStringParamsFunc(link, routeValues);
        }
    }
}