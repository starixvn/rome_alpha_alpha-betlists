namespace Alpha.BetLists.StaticData.DataAccesses
{
    using ColumnIndexes;
    using Fanex.BetList.Core.Entities;
    using Fanex.Common;
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
                AgentPT = GetCellValue<decimal>(row, TicketDataTemplateColumnIndex.AgentPT),
                BetTeam = GetCellValue<string>(row, TicketDataTemplateColumnIndex.BetTeam),
                BetTypeId = GetCellValue<int>(row, TicketDataTemplateColumnIndex.BetTypeId),
                MasterPT = GetCellValue<decimal>(row, TicketDataTemplateColumnIndex.MasterPT),
                Odds = GetCellValue<decimal>(row, TicketDataTemplateColumnIndex.Odds),
                RefNo = GetCellValue<string>(row, TicketDataTemplateColumnIndex.RefNo),
                Stake = GetCellValue<decimal>(row, TicketDataTemplateColumnIndex.Stake),
                Status = GetCellValue<string>(row, TicketDataTemplateColumnIndex.Status),
                SuperPT = GetCellValue<decimal>(row, TicketDataTemplateColumnIndex.SuperPT),
                TransDesc = GetCellValue<string>(row, TicketDataTemplateColumnIndex.TransDesc)
            };

        private static TicketDto MapRowToTicketObject(IRow row)
            => new TicketDto
            {
                ActualStake = GetCellValue<decimal>(row, TicketTemplateColumnIndex.ActualStake),
                AgentComm = GetCellValue<decimal>(row, TicketTemplateColumnIndex.AgentComm),
                AgentDiscount = GetCellValue<decimal>(row, TicketTemplateColumnIndex.AgentDiscount),
                AgentPositionTaking = GetCellValue<decimal>(row, TicketTemplateColumnIndex.AgentPositionTaking),
                AgentWinlost = GetCellValue<decimal>(row, TicketTemplateColumnIndex.AgentWinlost),
                AwayId = GetCellValue<long>(row, TicketTemplateColumnIndex.AwayId),
                BetCheck = GetCellValue<string>(row, TicketTemplateColumnIndex.BetCheck),
                BetId = GetCellValue<long>(row, TicketTemplateColumnIndex.BetId),
                BetTeam = GetCellValue<string>(row, TicketTemplateColumnIndex.BetTeam),
                BetTypeId = GetCellValue<int>(row, TicketTemplateColumnIndex.BetTypeId),
                Comm = GetCellValue<decimal>(row, TicketTemplateColumnIndex.Comm),
                CommStatus = GetCellValue<string>(row, TicketTemplateColumnIndex.CommStatus),
                CustId = GetCellValue<long>(row, TicketTemplateColumnIndex.CustId),
                Currency = GetCellValue<string>(row, TicketTemplateColumnIndex.Currency),
                EventDate = GetCellValue<DateTime>(row, TicketTemplateColumnIndex.EventDate),
                EventStatus = GetCellValue<string>(row, TicketTemplateColumnIndex.EventStatus),
                Handicap1 = GetCellValue<decimal>(row, TicketTemplateColumnIndex.Handicap1),
                Handicap2 = GetCellValue<decimal>(row, TicketTemplateColumnIndex.Handicap2),
                HomeId = GetCellValue<long>(row, TicketTemplateColumnIndex.HomeId),
                IP = GetCellValue<string>(row, TicketTemplateColumnIndex.IP),
                IsLive = Convert.ToBoolean(GetCellValue<int>(row, TicketTemplateColumnIndex.IsLive)),
                IsNeutral = Convert.ToBoolean(GetCellValue<int>(row, TicketTemplateColumnIndex.IsNeutral)),
                LeagueId = GetCellValue<int>(row, TicketTemplateColumnIndex.LeagueId),
                LiveAwayScore = GetCellValue<int>(row, TicketTemplateColumnIndex.LiveAwayScore),
                LiveHomeScore = GetCellValue<int>(row, TicketTemplateColumnIndex.LiveHomeScore),
                MasterDiscount = GetCellValue<decimal>(row, TicketTemplateColumnIndex.MasterDiscount),
                MasterOdds = GetCellValue<decimal>(row, TicketTemplateColumnIndex.MasterOdds),
                MasterPositionTaking = GetCellValue<decimal>(row, TicketTemplateColumnIndex.MasterPositionTaking),
                MasterWinlost = GetCellValue<decimal>(row, TicketTemplateColumnIndex.MasterWinlost),
                MatchCode = GetCellValue<string>(row, TicketTemplateColumnIndex.MatchCode),
                MatchId = GetCellValue<int>(row, TicketTemplateColumnIndex.MatchId),
                Odds = GetCellValue<decimal>(row, TicketTemplateColumnIndex.Odds),
                OddsType = GetCellValue<string>(row, TicketTemplateColumnIndex.OddsType),
                PlayerComm = GetCellValue<decimal>(row, TicketTemplateColumnIndex.PlayerComm),
                Race = GetCellValue<string>(row, TicketTemplateColumnIndex.Race),
                RefNo = GetCellValue<string>(row, TicketTemplateColumnIndex.RefNo),
                ShowTime = GetCellValue<string>(row, TicketTemplateColumnIndex.ShowTime),
                SportTypeId = GetCellValue<int>(row, TicketTemplateColumnIndex.SportTypeId),
                Stake = GetCellValue<decimal>(row, TicketTemplateColumnIndex.Stake),
                Status = GetCellValue<string>(row, TicketTemplateColumnIndex.Status),
                SuperComm = GetCellValue<decimal>(row, TicketTemplateColumnIndex.SuperComm),
                SuperDiscount = GetCellValue<decimal>(row, TicketTemplateColumnIndex.SuperDiscount),
                SuperPositionTaking = GetCellValue<decimal>(row, TicketTemplateColumnIndex.SuperPositionTaking),
                SuperWinlost = GetCellValue<decimal>(row, TicketTemplateColumnIndex.SuperWinlost),
                TransDate = GetCellValue<DateTime>(row, TicketTemplateColumnIndex.TransDate),
                TransId = GetCellValue<long>(row, TicketTemplateColumnIndex.TransId),
                UserName = GetCellValue<string>(row, TicketTemplateColumnIndex.UserName),
                Winlost = GetCellValue<decimal>(row, TicketTemplateColumnIndex.Winlost),
                WinlostDate = GetCellValue<DateTime>(row, TicketTemplateColumnIndex.WinlostDate),
                TransDesc = GetCellValue<string>(row, TicketTemplateColumnIndex.TransDesc),
                OddsSpread = GetCellValue<decimal>(row, TicketTemplateColumnIndex.OddsSpread),
                OriginalBetType = GetCellValue<int>(row, TicketTemplateColumnIndex.OriginalBetType),
                BetdaqId = GetCellValue<long>(row, TicketTemplateColumnIndex.BetdaqId),
                OriginalStake = GetCellValue<decimal>(row, TicketTemplateColumnIndex.OriginalStake)
            };

        private static T GetCellValue<T>(IRow row, object columnIndex)
        {
            var cell = row.GetCell((int)columnIndex);
            var value = cell?.GetValue();

            if (value == null && typeof(T) == typeof(string))
            {
                value = string.Empty;
            }

            return Converter.To<T>(value);
        }

    }
}
