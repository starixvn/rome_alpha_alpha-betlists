namespace Alpha.BetLists.Site.Features.BookedBet
{
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Params;
    using Alpha.BetLists.Services;
    using Alpha.BetLists.Site.Handlers;
    using Alpha.IPInfo.Services;
    using AlphaPlus.Utilities.IP;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using System.Threading.Tasks;

    public class BookedBetHandler :
        BetListBaseHandler,
        IAsyncRequestHandler<BookedBetQuery, BookedBetViewModel>,
        IAsyncRequestHandler<BookedBetDataQuery, BookedBetDataViewModel>
    {
        private readonly BookedBetService service;

        public BookedBetHandler(
            BookedBetService service,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            IPService ipService,
            IIPInfoService ipInfoService) : base(builder, ticketHelper, null, ipService, ipInfoService)
        {
            this.service = service;
        }

        public async Task<BookedBetViewModel> Handle(BookedBetQuery message)
        {
            var parameter = new BookedBetParam(
                message.CustId,
                message.FromDate,
                message.ToDate,
                message.ViewerLevelId,
                message.StatusIds,
                message.PageSize,
                message.PageIndex);
            var bookedbets = await service.Get(parameter);
            await AssignCountry(bookedbets, message.DisplayVNIP);

            return new BookedBetViewModel(message, bookedbets, Builder, TicketHelper);
        }

        public async Task<BookedBetDataViewModel> Handle(BookedBetDataQuery message)
        {
            var parameter = new BookedBetParam(
                message.CustId,
                message.FromDate,
                message.ToDate,
                message.ViewerLevelId,
                message.StatusIds,
                message.PageSize,
                message.PageIndex);
            var bookedbets = await service.Get(parameter);
            await AssignCountry(bookedbets, message.DisplayVNIP);

            return new BookedBetDataViewModel(message, bookedbets, Builder, TicketHelper);
        }
    }
}