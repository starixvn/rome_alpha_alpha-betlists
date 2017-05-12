namespace Alpha.BetLists.Site.ViewModels.Outstanding
{
    using AppResources;
    using BetLists.Models;
    using Components;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests.Outstanding;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class OutstandingViewModel : BetListBaseViewModel
    {
        private readonly OutstandingQuery query;

        public OutstandingViewModel(
            OutstandingQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            bool showMemberName = false)
            : base(query, betListData, builder, ticketHelper, showMemberName)
        {
            this.query = query;
            Title = $"{Resources.BetlistRunning} - {query.CustName}";
        }

        public bool AllowToViewSuperPT => baseQuery.ViewerLevel.IsSuperLevel();

        public bool AllowToViewMasterPT => baseQuery.ViewerLevel.IsMasterOrSuperLevel();

        public IEnumerable<SelectListItem> Products { get; }

        private DateRangePickerViewModel DateRangePickerViewModel =>
            new DateRangePickerViewModel(
                query.MinDate,
                DateTime.MaxValue,
                FromDate,
                ToDate,
                query.UserSelectedDateRangeOption);

        private ProductMultiSelectionViewModel ProductMultiSelectionViewModel =>
            new ProductMultiSelectionViewModel(
                Products,
                ProductsId,
                query.Language);

        public OutstandingFilterViewModel FilterViewModel =>
            new OutstandingFilterViewModel(
                CustId,
                CustName,
                DateRangePickerViewModel,
                ProductMultiSelectionViewModel);
    }
}