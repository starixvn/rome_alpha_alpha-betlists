using System;
using Alpha.BetLists.Models;
using Alpha.StaticData.Npoi;
using NPOI.SS.UserModel;
using static Alpha.BetLists.StaticData.ColumnIndexes.TicketTemplateColumnIndex;

namespace Alpha.BetLists.StaticData.Mappers
{
    public static class TicketDtoMapper
    {
        public static TicketDto Map(IRow row)
            => new TicketDto
            {
                ActualStake = row.Get<decimal>(ActualStake),
                AgentComm = row.Get<decimal>(AgentComm),
                AgentDiscount = row.Get<decimal>(AgentDiscount),
                AgentPositionTaking = row.Get<decimal>(AgentPositionTaking),
                AgentWinlost = row.Get<decimal>(AgentWinlost),
                AwayId = row.Get<long>(AwayId),
                BetCheck = row.Get<string>(BetCheck),
                BetId = row.Get<long>(BetId),
                BetTeam = row.Get<string>(BetTeam),
                BetTypeId = row.Get<int>(BetTypeId),
                Comm = row.Get<decimal>(Comm),
                CommStatus = row.Get<string>(CommStatus),
                CustId = row.Get<long>(CustId),
                Currency = row.Get<string>(Currency),
                EventDate = row.Get<DateTime>(EventDate),
                EventStatus = row.Get<string>(EventStatus),
                Handicap1 = row.Get<decimal>(Handicap1),
                Handicap2 = row.Get<decimal>(Handicap2),
                HomeId = row.Get<long>(HomeId),
                IP = row.Get<string>(IP),
                IsLive = Convert.ToBoolean(row.Get<int>(IsLive)),
                IsNeutral = Convert.ToBoolean(row.Get<int>(IsNeutral)),
                LeagueId = row.Get<int>(LeagueId),
                LiveAwayScore = row.Get<int>(LiveAwayScore),
                LiveHomeScore = row.Get<int>(LiveHomeScore),
                MasterDiscount = row.Get<decimal>(MasterDiscount),
                MasterOdds = row.Get<decimal>(MasterOdds),
                MasterPositionTaking = row.Get<decimal>(MasterPositionTaking),
                MasterWinlost = row.Get<decimal>(MasterWinlost),
                MatchCode = row.Get<string>(MatchCode),
                MatchId = row.Get<int>(MatchId),
                Odds = row.Get<decimal>(Odds),
                OddsType = row.Get<string>(OddsType),
                PlayerComm = row.Get<decimal>(PlayerComm),
                Race = row.Get<string>(Race),
                RefNo = row.Get<string>(RefNo),
                ShowTime = row.Get<string>(ShowTime),
                SportTypeId = row.Get<int>(SportTypeId),
                Stake = row.Get<decimal>(Stake),
                Status = row.Get<string>(Status),
                SuperComm = row.Get<decimal>(SuperComm),
                SuperDiscount = row.Get<decimal>(SuperDiscount),
                SuperPositionTaking = row.Get<decimal>(SuperPositionTaking),
                SuperWinlost = row.Get<decimal>(SuperWinlost),
                TransDate = row.Get<DateTime>(TransDate),
                TransId = row.Get<long>(TransId),
                UserName = row.Get<string>(UserName),
                Winlost = row.Get<decimal>(Winlost),
                WinlostDate = row.Get<DateTime>(WinlostDate),
                TransDesc = row.Get<string>(TransDesc),
                OddsSpread = row.Get<decimal>(OddsSpread),
                OriginalBetType = row.Get<int>(OriginalBetType),
                BetdaqId = row.Get<long>(BetdaqId),
                OriginalStake = row.Get<decimal>(OriginalStake)
            };
    }
}
