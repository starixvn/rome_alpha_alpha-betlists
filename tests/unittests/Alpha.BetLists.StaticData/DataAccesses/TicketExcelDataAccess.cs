namespace Alpha.BetLists.StaticData.DataAccesses
{
    using ColumnIndexes;
    using Fanex.BetList.Core.Entities;
    using Helpers;
    using IntegrationTest;
    using Models;
    using NPOI.SS.UserModel;
    using System;
    using System.Collections.Generic;

    public class TicketExcelDataAccess
    {
        private readonly IWorkbook workbook;

        public TicketExcelDataAccess(string fileName)
        {
            workbook = NpoiHelper.LoadWorkbook(fileName);
        }

        public IEnumerable<TicketDto> GetTickets() => workbook.GetData("BetTickets", MapRowToTicketObject);

        public IEnumerable<ITicketData> GetTicketData() => workbook.GetData("ReferenceData", MapRowToTicketDataObject);

        private static ITicketData MapRowToTicketDataObject(IRow row)
            => new TicketData
            {
                AgentPT = row.GetCellValue<decimal>(TicketDataTemplateColumnIndex.AgentPT),
                BetTeam = row.GetCellValue<string>(TicketDataTemplateColumnIndex.BetTeam),
                BetTypeId = row.GetCellValue<int>(TicketDataTemplateColumnIndex.BetTypeId),
                MasterPT = row.GetCellValue<decimal>(TicketDataTemplateColumnIndex.MasterPT),
                Odds = row.GetCellValue<decimal>(TicketDataTemplateColumnIndex.Odds),
                RefNo = row.GetCellValue<string>(TicketDataTemplateColumnIndex.RefNo),
                Stake = row.GetCellValue<decimal>(TicketDataTemplateColumnIndex.Stake),
                Status = row.GetCellValue<string>(TicketDataTemplateColumnIndex.Status),
                SuperPT = row.GetCellValue<decimal>(TicketDataTemplateColumnIndex.SuperPT),
                TransDesc = row.GetCellValue<string>(TicketDataTemplateColumnIndex.TransDesc)
            };

        private static TicketDto MapRowToTicketObject(IRow row)
            => new TicketDto
            {
                ActualStake = row.GetCellValue<decimal>(TicketTemplateColumnIndex.ActualStake),
                AgentComm = row.GetCellValue<decimal>(TicketTemplateColumnIndex.AgentComm),
                AgentDiscount = row.GetCellValue<decimal>(TicketTemplateColumnIndex.AgentDiscount),
                AgentPositionTaking = row.GetCellValue<decimal>(TicketTemplateColumnIndex.AgentPositionTaking),
                AgentWinlost = row.GetCellValue<decimal>(TicketTemplateColumnIndex.AgentWinlost),
                AwayId = row.GetCellValue<long>(TicketTemplateColumnIndex.AwayId),
                BetCheck = row.GetCellValue<string>(TicketTemplateColumnIndex.BetCheck),
                BetId = row.GetCellValue<long>(TicketTemplateColumnIndex.BetId),
                BetTeam = row.GetCellValue<string>(TicketTemplateColumnIndex.BetTeam),
                BetTypeId = row.GetCellValue<int>(TicketTemplateColumnIndex.BetTypeId),
                Comm = row.GetCellValue<decimal>(TicketTemplateColumnIndex.Comm),
                CommStatus = row.GetCellValue<string>(TicketTemplateColumnIndex.CommStatus),
                CustId = row.GetCellValue<long>(TicketTemplateColumnIndex.CustId),
                Currency = row.GetCellValue<string>(TicketTemplateColumnIndex.Currency),
                EventDate = row.GetCellValue<DateTime>(TicketTemplateColumnIndex.EventDate),
                EventStatus = row.GetCellValue<string>(TicketTemplateColumnIndex.EventStatus),
                Handicap1 = row.GetCellValue<decimal>(TicketTemplateColumnIndex.Handicap1),
                Handicap2 = row.GetCellValue<decimal>(TicketTemplateColumnIndex.Handicap2),
                HomeId = row.GetCellValue<long>(TicketTemplateColumnIndex.HomeId),
                IP = row.GetCellValue<string>(TicketTemplateColumnIndex.IP),
                IsLive = Convert.ToBoolean(row.GetCellValue<int>(TicketTemplateColumnIndex.IsLive)),
                IsNeutral = Convert.ToBoolean(row.GetCellValue<int>(TicketTemplateColumnIndex.IsNeutral)),
                LeagueId = row.GetCellValue<int>(TicketTemplateColumnIndex.LeagueId),
                LiveAwayScore = row.GetCellValue<int>(TicketTemplateColumnIndex.LiveAwayScore),
                LiveHomeScore = row.GetCellValue<int>(TicketTemplateColumnIndex.LiveHomeScore),
                MasterDiscount = row.GetCellValue<decimal>(TicketTemplateColumnIndex.MasterDiscount),
                MasterOdds = row.GetCellValue<decimal>(TicketTemplateColumnIndex.MasterOdds),
                MasterPositionTaking = row.GetCellValue<decimal>(TicketTemplateColumnIndex.MasterPositionTaking),
                MasterWinlost = row.GetCellValue<decimal>(TicketTemplateColumnIndex.MasterWinlost),
                MatchCode = row.GetCellValue<string>(TicketTemplateColumnIndex.MatchCode),
                MatchId = row.GetCellValue<int>(TicketTemplateColumnIndex.MatchId),
                Odds = row.GetCellValue<decimal>(TicketTemplateColumnIndex.Odds),
                OddsType = row.GetCellValue<string>(TicketTemplateColumnIndex.OddsType),
                PlayerComm = row.GetCellValue<decimal>(TicketTemplateColumnIndex.PlayerComm),
                Race = row.GetCellValue<string>(TicketTemplateColumnIndex.Race),
                RefNo = row.GetCellValue<string>(TicketTemplateColumnIndex.RefNo),
                ShowTime = row.GetCellValue<string>(TicketTemplateColumnIndex.ShowTime),
                SportTypeId = row.GetCellValue<int>(TicketTemplateColumnIndex.SportTypeId),
                Stake = row.GetCellValue<decimal>(TicketTemplateColumnIndex.Stake),
                Status = row.GetCellValue<string>(TicketTemplateColumnIndex.Status),
                SuperComm = row.GetCellValue<decimal>(TicketTemplateColumnIndex.SuperComm),
                SuperDiscount = row.GetCellValue<decimal>(TicketTemplateColumnIndex.SuperDiscount),
                SuperPositionTaking = row.GetCellValue<decimal>(TicketTemplateColumnIndex.SuperPositionTaking),
                SuperWinlost = row.GetCellValue<decimal>(TicketTemplateColumnIndex.SuperWinlost),
                TransDate = row.GetCellValue<DateTime>(TicketTemplateColumnIndex.TransDate),
                TransId = row.GetCellValue<long>(TicketTemplateColumnIndex.TransId),
                UserName = row.GetCellValue<string>(TicketTemplateColumnIndex.UserName),
                Winlost = row.GetCellValue<decimal>(TicketTemplateColumnIndex.Winlost),
                WinlostDate = row.GetCellValue<DateTime>(TicketTemplateColumnIndex.WinlostDate),
                TransDesc = row.GetCellValue<string>(TicketTemplateColumnIndex.TransDesc),
                OddsSpread = row.GetCellValue<decimal>(TicketTemplateColumnIndex.OddsSpread),
                OriginalBetType = row.GetCellValue<int>(TicketTemplateColumnIndex.OriginalBetType),
                BetdaqId = row.GetCellValue<long>(TicketTemplateColumnIndex.BetdaqId),
                OriginalStake = row.GetCellValue<decimal>(TicketTemplateColumnIndex.OriginalStake)
            };
    }
}
