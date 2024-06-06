namespace Alpha.BetLists.Site.ViewModels.RunningBetList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AppResources;
    using BetLists.Models;
    using Components;
    using Constants;
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using MemberBetList;
    using Params;
    using Requests.RunningBetList;
    using Utils;

    public class RunningBetListViewModel : BetListBaseViewModel
    {
        private readonly RunningBetListQuery query;

        public RunningBetListViewModel(
            RunningBetListQuery query,
            RunningBetListParam param,
            BetListData betListData,
            IEnumerable<GroupProductViewModel> groupProducts,
            IEnumerable<int> supportScoreMapBetTypeIds,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper)
            : base(query, betListData, builder, ticketHelper, false)
        {
            this.query = query;

            GroupProducts = groupProducts;
            SupportScoreMapBetTypeIds = supportScoreMapBetTypeIds;

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

        public IEnumerable<int> SupportScoreMapBetTypeIds { get; }

        public IEnumerable<GroupProductViewModel> GroupProducts { get; }

        public DateRangePickerViewModel DateRangePickerViewModel =>
            new DateRangePickerViewModel(
                query.MinDate,
                DateTime.MaxValue,
                FromDate,
                ToDate,
                query.UserSelectedDateRangeOption,
                true,
                MemberBetListViewModel.RangeOptions);

        public ProductMultiSelectionViewModel ProductMultiSelectionViewModel =>
            new ProductMultiSelectionViewModel(
                GroupProducts,
                ProductsId,
                query.Language);

        public override bool CanViewScoreMap(ITicket ticket) => SupportScoreMapBetTypeIds.Contains(ticket.BetTypeId);

        public string BuildBetListLink(string link)
        {
            var routeValues = new Dictionary<string, object>
            {
                ["CustId"] = CustId,
                ["CustName"] = CustName,
                ["FromDate"] = SystemTime.Today.AddDays(-1).ToShortDateWithSlash(),
                ["ToDate"] = SystemTime.Today.ToShortDateWithSlash(),
                ["ProductIds"] = ProductsId,
                ["UserSelectedDateRangeOption"] = UserSelectedDateRangeOptionConstant.SpecificDates
            };

            return Helpers.UrlHelper.AddQueryStringParamsFunc(link, routeValues);
        }

        public static string GetMemberWin(decimal? amount)
            => amount.HasValue ? ConvertByBetType.Winloss(amount.Value) : "-";

        public string GetSMARiskAmount(TicketDto ticket)
        {
            var amount = 0m;
            if(!ticket.MemberWin.HasValue)
            {
                return "-";
            }

            if(ViewerLevel.IsSuperLevel())
            {
                amount = ticket.MemberWin.Value * ticket.SuperPositionTaking;
            }
            else if(ViewerLevel.IsMasterLevel())
            {
                amount = ticket.MemberWin.Value * ticket.MasterPositionTaking;
            }
            else
            {
                amount = ticket.MemberWin.Value * ticket.AgentPositionTaking;
            }

            return ConvertByBetType.Winloss(amount);
        }
    }
}