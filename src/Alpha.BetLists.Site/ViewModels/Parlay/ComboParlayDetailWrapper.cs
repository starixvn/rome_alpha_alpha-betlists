namespace Alpha.BetLists.Site.ViewModels.Parlay
{
    using System.Collections.Generic;
    using System.Linq;
    using AppResources;
    using BetLists.Models.Parlay;
    using Fanex.BetList.Core.App_GlobalResources;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;

    public class ComboParlayDetailWrapper
    {
        private readonly IDictionary<string, string> statusToResources = new Dictionary<string, string>
        {
            { BetStatus.Running, Resources.running },
            { BetStatus.Reject, Resources.reject },
            { BetStatus.Won, Resources.won },
            { BetStatus.Lose, Resources.lose },
            { BetStatus.Draw, Resources.draw },
            { BetStatus.Waiting, Resources.waiting },
            { BetStatus.Void, Resources.Void },
            { BetStatus.Refund, Resources.refund },
            { BetStatus.Pending, CoreBetList.pending }
        };

        public ComboParlayDetailWrapper(
            ComboParlayDetail comboParlayDetail,
            IEnumerable<MatchDetail> matchDetails,
            ITicketHelper ticketHelper)
        {
            ComboParlayDetail = comboParlayDetail;
            MatchDetails = matchDetails;
            Odds = GetOdds();
            string statusResource;
            Status = statusToResources.TryGetValue(ComboParlayDetail.Status.ToLowerInvariant(), out statusResource) ? statusResource : string.Empty;
            Events = GetEvents(ticketHelper);
            WinLostDate = Formatter.ShortDateFormat(comboParlayDetail.WinLostDate);
            WinLost = ConvertByBetType.Winloss(comboParlayDetail.WinLost);
            Comm = ConvertByBetType.Comm(comboParlayDetail.PlayerComm);
            Stake = ConvertByBetType.Stake(comboParlayDetail.Stake);
        }

        public ComboParlayDetail ComboParlayDetail { get; }

        public IEnumerable<MatchDetail> MatchDetails { get; }

        public string WinLostDate { get; }

        public string WinLost { get; }

        public string Comm { get; }

        public string Status { get; }

        public string Stake { get; }

        public string Odds { get; }

        public List<string> Events { get; }

        private List<string> GetEvents(ITicketHelper ticketHelper)
        {
            var events = new List<string>();

            var currentSerial = ComboParlayDetail.Serial;
            var matches = MatchDetails.Where(m => m.Serial == currentSerial);

            foreach (var match in matches)
            {
                events.Add($"{ticketHelper.GetTeamNameById(match.HomeId)} - vs - {ticketHelper.GetTeamNameById(match.AwayId)}");
            }

            return events;
        }

        private string GetOdds()
            =>ConvertByBetType.CommValue(ComboParlayDetail.Odds);
    }
}
