namespace Alpha.BetLists.Site.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Site.Requests;
    using MediatR;

    public class UsernameSuggestionController : BetListBaseController
    {
        public UsernameSuggestionController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Get(UsernameSuggestionQuery message)
        {
            var usernamesSuggestion = message.IsValid()
                ? await mediator.Send(message)
                : Enumerable.Empty<UsernameSuggestion>();

            return Json(usernamesSuggestion, JsonRequestBehavior.AllowGet);
        }
    }
}