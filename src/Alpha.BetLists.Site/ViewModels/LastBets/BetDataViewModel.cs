using System.Collections.Generic;
using System.Linq;
using Alpha.BetLists.Constants;
using Alpha.BetLists.Helpers;
using Alpha.BetLists.Models;
using Alpha.BetLists.Site.Requests.LastBets;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace Alpha.BetLists.Site.ViewModels.LastBets
{
    public class BetDataViewModel : BetListBaseViewModel
    {
        public BetDataViewModel(
            GetLastBetsQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            IEnumerable<int> supportScoreMapBettypeIds)
            : base(query, betListData, builder, ticketHelper, showMemberName: true)
        {
            TotalColumn = 7;
            SupportScoreMapBettypeIds = supportScoreMapBettypeIds;
        }

        public IEnumerable<int> SupportScoreMapBettypeIds { get; }

        public override bool CanViewScoreMap(ITicket ticket) => SupportScoreMapBettypeIds.Contains(ticket.BetTypeId);

        public List<ITicketData> CreateTicketData(ITicket ticket)
        {
            if (!BetTypeHelper.IsWinPlace(ticket.BetTypeId))
            {
                return null;
            }

            var isHorseRacingWinPlace = ticket.BetTypeId == BetTypeIdConstants.HorseRacing_WinPlace;
            var winBetType = isHorseRacingWinPlace ? BetTypeIdConstants.HorseRacing_Win : BetTypeIdConstants.VirtualHorseRacing_Win;
            var placeBetType = isHorseRacingWinPlace ? BetTypeIdConstants.HorseRacing_Place : BetTypeIdConstants.VirtualHorseRacing_Place;

            var result = new List<ITicketData>
            {
                new TicketData
                {
                    RefNo = ticket.RefNo,
                    Status = ticket.Status,
                    BetTeam = ticket.BetTeam,
                    BetTypeId = winBetType,
                    Odds = ticket.Handicap1
                 },
                new TicketData
                {
                    RefNo = ticket.RefNo,
                    Status = ticket.Status,
                    BetTypeId = placeBetType,
                    Odds = ticket.Handicap2
                }
            };

            return result;
        }
    }
}