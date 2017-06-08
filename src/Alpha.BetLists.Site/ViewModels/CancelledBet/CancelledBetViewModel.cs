using Alpha.BetLists.AppResources;
using Alpha.BetLists.Models.CancelledBet;
using Alpha.BetLists.Site.Requests;
using Alpha.BetLists.Site.Requests.CancelledBet;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Alpha.BetLists.Site.ViewModels.CancelledBet
{
    public class CancelledBetViewModel : BetListBaseViewModel
    {
        public CancelledBetViewModel(
            CancelledBetBaseQuery query,
            CancelledBetModel cancelledBet,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper) : base(query, cancelledBet.BetListData, builder, ticketHelper, true)
        {
            Filter = cancelledBet.Filter;
            Query = query;
            Title = Resources.VoidAndReject;
        }

        public CancelledBetBaseQuery Query { get; protected set; }

        private FilterCancelledBetModel Filter { get; }

        public bool IsShowMasterList => ViewerLevel.IsSuperLevel() && Filter.Masters.Any();

        public bool IsShowAgentList => ViewerLevel.IsMasterOrSuperLevel() && Filter.Agents.Any();

        public bool IsShowMemberList => !(ViewerLevel.IsSuperLevel() && Filter.AgentId == 0) && Filter.Members.Any();

        public bool IsShowFilterIcon => IsShowMasterList || IsShowAgentList || IsShowMemberList;

        public IEnumerable<SelectListItem> MasterList => BuildUserBetCount(Filter.Masters, Query.MasterId);

        public IEnumerable<SelectListItem> AgentList
        {
            get
            {
                var list = BuildUserBetCount(Filter.Agents, Query.AgentId).ToList();
                if (!ViewerLevel.IsMasterLevel())
                {
                    list.Insert(0, new SelectListItem { Text = Resources.AllAgent, Value = "0", Selected = Filter.AgentId == 0 });
                }

                return list;
            }
        }

        public IEnumerable<SelectListItem> MemberList
        {
            get
            {
                var list = BuildUserBetCount(Filter.Members, Query.MemberId).ToList();

                if (!ViewerLevel.IsAgentLevel())
                {
                    list.Insert(0, new SelectListItem { Text = Resources.AllMember, Value = "0", Selected = Filter.MemberId == 0 });
                }

                return list;
            }
        }

        private static IEnumerable<SelectListItem> BuildUserBetCount(IEnumerable<UserBetCount> users, int selectedId)
            => users.Select(user => new SelectListItem
            {
                Text = user.ToString(),
                Value = user.CustId.ToString(),
                Selected = user.CustId == selectedId
            });
    }
}