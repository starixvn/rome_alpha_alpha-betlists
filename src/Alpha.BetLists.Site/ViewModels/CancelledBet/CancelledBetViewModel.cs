namespace Alpha.BetLists.Site.ViewModels.CancelledBet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Alpha.BetLists.AppResources;
    using Alpha.BetLists.Site.Requests.CancelledBet;
    using BetLists.Constants;
    using BetLists.Models;
    using Components;
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Params;

    public class CancelledBetViewModel : BetListBaseViewModel
    {
        public static readonly IEnumerable<string> RangeOptions = new List<string>
        {
            DateRangePickerViewModel.TodayOptionValue,
            DateRangePickerViewModel.YesterdayOptionValue,
            DateRangePickerViewModel.ThisWeekOptionValue,
            DateRangePickerViewModel.LastWeekOptionValue,
            DateRangePickerViewModel.ThisMonthOptionValue,
            DateRangePickerViewModel.SpecificDateOptionValue
        };

        public CancelledBetViewModel(
            CancelledBetBaseQuery query,
            CancelledBetParam param,
            PagedReportData<BetListData> reportData,
            IEnumerable<SelectListItem> products,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper) : base(query, reportData.DataSource.FirstOrDefault(), builder, ticketHelper, true)
        {
            Products = products;
            ProductsId = param.ProductIds.JoinWithComma();
            ReportData = reportData;
            Language = query.Language;
            MinDate = query.MinDate;
            UserSelectedDateRangeOption = query.UserSelectedDateRangeOption;
            StatusIds = string.IsNullOrEmpty(query.StatusIds) ?
                $"{TicketStatusIdConstants.Void},{TicketStatusIdConstants.Refund},{TicketStatusIdConstants.Reject}" :
                query.StatusIds;

            Title = Resources.VoidAndReject;
            ViewDataInstructionMessage = string.Format(Resources.CanOnlyViewDataAfterTimeHistoryReport, MinDate.ToShortDateWithSlash());
            IsFilterVisible = query.IsFilterVisible;
        }

        public bool IsFilterVisible { get; }

        public PagerViewModel PagerViewModel =>
            new PagerViewModel(ReportData.PageIndex, ReportData.PageSize, ReportData.TotalRecords);

        public string ViewDataInstructionMessage { get; }

        public string StatusIds { get; }

        public DateRangePickerViewModel DateRangePickerViewModel
            => new DateRangePickerViewModel(
                MinDate,
                DateTime.MaxValue,
                FromDate,
                ToDate,
                UserSelectedDateRangeOption,
                true,
                RangeOptions);

        public ProductMultiSelectionViewModel ProductMultiSelectionViewModel
           => new ProductMultiSelectionViewModel(Products, ProductsId, Language);

        public StatusMultiSelectionViewModel StatusMultiSelectionViewModel
        {
            get
            {
                var statuses = new List<StatusModel>
                {
                    new StatusModel(TicketStatusIdConstants.Void, Resources.Void),
                    new StatusModel(TicketStatusIdConstants.Reject, Resources.reject),
                    new StatusModel(TicketStatusIdConstants.Refund, Resources.refund)
                };

                var statusListItem = statuses.Select(s =>
                 {
                     var item = new SelectListItem
                     {
                         Text = s.Name,
                         Value = s.Id.ToString(),
                         Selected = StatusIds.Contains(s.Id.ToString())
                     };

                     return item;
                 });

                return new StatusMultiSelectionViewModel(statusListItem, StatusIds, Language);
            }
        }

        public int BuildNo(int no)
            => no + 1 + ((ReportData.PageIndex - 1) * ReportData.PageSize);

        private IEnumerable<SelectListItem> Products { get; }

        private string Language { get; }

        private int UserSelectedDateRangeOption { get; }

        private DateTime MinDate { get; }

        private PagedReportData<BetListData> ReportData { get; }
    }
}