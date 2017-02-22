namespace Alpha.BetLists.Site.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using MediatR;
    using ViewModels;
    using Web;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public abstract class BetListBaseController : BaseController
    {
        protected readonly IMediator mediator;

        protected BetListBaseController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        private FileContentResult GetExcelContent(ExcelFileViewModel viewModel)
            => File(viewModel.Content, viewModel.ContentType, viewModel.Name);

        protected ActionResult View<TResponse>(IRequest<TResponse> message)
        {
            return View(mediator.Send(message));
        }

        protected FileContentResult ExcelView(IRequest<ExcelFileViewModel> message)
        {
            return GetExcelContent(mediator.Send(message));
        }
    }
}