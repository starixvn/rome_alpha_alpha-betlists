namespace Alpha.BetLists.Site.Features.BookedBet
{
    using Alpha.BetLists.Site.ViewModels.Components;
    using BetLists.Constants;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class BookedBetViewModel : BookedBetBaseViewModel
    {
        public BookedBetViewModel(
            BookedBetBaseQuery query,
            IEnumerable<TicketDto> tickets,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper) : base(query, tickets, builder, ticketHelper)
        {
            Language = query.Language;
            Title = "Booked bets title";

            DateRangePickerViewModel = new DateRangePickerNewViewModel(
                query.FromDate,
                query.ToDate,                
                query.MinDate);
        }

        public IEnumerable<SelectListItem> StatusMultiSelectionViewModel
            => BookedBetStatusConstants.Dropdownlist.Select(s =>
                  {
                      var item = new SelectListItem
                      {
                          Text = s.Value,
                          Value = s.Key.ToString(),
                          Selected = true
                      };

                      return item;
                  });

        public string Language { get; }

        public DateRangePickerNewViewModel DateRangePickerViewModel { get; }
    }
}