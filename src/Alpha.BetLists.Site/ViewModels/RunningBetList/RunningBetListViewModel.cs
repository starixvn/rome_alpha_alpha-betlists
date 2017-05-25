namespace Alpha.BetLists.Site.ViewModels.RunningBetList
{
    using AppResources;
    using BetLists.Models;
    using Components;
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Params;
    using Requests.RunningBetList;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Utils;

    public class RunningBetListViewModel : BetListBaseViewModel
    {
        private readonly RunningBetListQuery query;

        public RunningBetListViewModel(
            RunningBetListQuery query,
            RunningBetListParam param,
            BetListData betListData,
            IEnumerable<SelectListItem> products,
            IEnumerable<int> supportScoreMapBettypeIds,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper)
            : base(query, betListData, builder, ticketHelper, false)
        {
            this.query = query;

            Products = products;
            SupportScoreMapBettypeIds = supportScoreMapBettypeIds;

            Title = $"{Resources.BetlistRunning} - {query.CustName}";
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

        public IEnumerable<int> SupportScoreMapBettypeIds { get; }

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

        public override bool CanViewScoreMap(ITicket ticket) => SupportScoreMapBettypeIds.Contains(ticket.BetTypeId);

        public string BuildBetListLink(string link)
        {
            var routeValues = new Dictionary<string, object>
            {
                ["CustId"] = CustId,
                ["CustName"] = CustName,
                ["FromDate"] = SystemTime.Today.AddDays(-1).ToShortDateWithSlash(),
                ["ToDate"] = SystemTime.Today.ToShortDateWithSlash(),
                ["ProductIds"] = ProductsId
            };

            return Helpers.UrlHelper.AddQueryStringParamsFunc(link, routeValues);
        }
    }
}