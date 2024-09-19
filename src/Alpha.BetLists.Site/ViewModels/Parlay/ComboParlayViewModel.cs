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
            : base(query, betListData, builder, ticketHelper, showMemberName: false, supportRiskAmount: true)
        {
            TicketHelper.IsShowOddsWithChoice = true;
            HideOfferNameUnitStakes = string.IsNullOrEmpty(query.BetCheck);
            ComboParlayOfferNames = string.IsNullOrEmpty(query.BetCheck) ?
                new List<string>() :
                TicketHelper.GetCombinationMPOfferNameList(query.BetTeam, query.BetCheck, query.Handicap1, query.Handicap2);
            ComboParlaySingleTickets = BetListData.Tickets
                .Where(t => t.Stake > 0)
                .OrderBy(t => t.MatchId)
                .ToList();

            UnitStakeByOfferNames = GetUnitStakeByOfferName(query);
        }

        public IDictionary<string, string> UnitStakeByOfferNames { get; }

        private IList<string> ComboParlayOfferNames { get; }

        private List<TicketDto> ComboParlaySingleTickets { get; }

        private bool HideOfferNameUnitStakes { get; }

        public bool HasValidData
            => ComboParlaySingleTickets.Any() &&
            (HideOfferNameUnitStakes || ComboParlayOfferNames.Count <= ComboParlaySingleTickets.Count);

        private IDictionary<string, string> GetUnitStakeByOfferName(ComboParlayQuery query)
        {
            var offerNameUnitStakes = new Dictionary<string, string>();
            if (HideOfferNameUnitStakes)
            {
                return offerNameUnitStakes;
            }

            var isSystemParlay = query.Handicap1 == 0;

            if (HasValidData)
            {
                var offerNamesAmount = ComboParlayOfferNames.Count;
                for (var i = 0; i < offerNamesAmount; i++)
                {
                    var comboTicket = isSystemParlay ? ComboParlaySingleTickets[0] : ComboParlaySingleTickets[i];

                    offerNameUnitStakes.Add(ComboParlayOfferNames[i], comboTicket.Currency + " " + comboTicket.Stake.ToString("N0"));
                }
            }

            return offerNameUnitStakes;
        }
    }
}