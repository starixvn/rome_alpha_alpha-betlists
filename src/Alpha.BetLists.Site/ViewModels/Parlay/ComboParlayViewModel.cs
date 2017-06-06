namespace Alpha.BetLists.Site.ViewModels.Parlay
{
    using System.Collections.Generic;
    using System.Linq;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests.Parlay;

    public class ComboParlayViewModel : BetListBaseViewModel
    {
        public ComboParlayViewModel(
            ComboParlayQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper)
            : base(query, betListData, builder, ticketHelper)
        {
            UnitStakeByOfferNames = GetUnitStakeByOfferName(query);
            TicketHelper.IsShowOddsWithChoice = true;
        }

        public IDictionary<string, string> UnitStakeByOfferNames { get; }

        private IDictionary<string, string> GetUnitStakeByOfferName(ComboParlayQuery query)
        {
            var offerNameUnitStakes = new Dictionary<string, string>();
            var offerNames = TicketHelper.GetCombinationMPOfferNameList(query.BetTeam, query.BetCheck, query.Handicap1, query.Handicap2);
            var comboTickets = BetListData.Tickets
                .Where(t => t.Stake > 0)
                .OrderBy(t => t.MatchId)
                .ToArray();

            if (comboTickets.Any())
            {
                var count = offerNames.Count();
                for (var i = 0; i < count; i++)
                {
                    var isSystemParlay = query.Handicap1 == 0;
                    var comboTicket = isSystemParlay ? comboTickets[0] : comboTickets[i];

                    offerNameUnitStakes.Add(offerNames[i], comboTicket.Currency + " " + comboTicket.Stake.ToString("N0"));
                }
            }

            return offerNameUnitStakes;
        }
    }
}