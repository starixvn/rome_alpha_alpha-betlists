namespace Alpha.BetLists.Site.Requests.MonitoringBets
{
    using System.Collections.Generic;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Models.MonitoringBets;
    using MediatR;

    public class CheckUsernamesQuery : BaseQuery, IRequest<IEnumerable<MemberErrCode>>
    {
        public CheckUsernamesQuery(IUser viewer) : base(viewer)
        {
        }

        public string Usernames { get; set; }
    }
}