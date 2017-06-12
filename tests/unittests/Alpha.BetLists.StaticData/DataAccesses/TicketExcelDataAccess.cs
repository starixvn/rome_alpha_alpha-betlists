using Alpha.BetLists.Models;
using Alpha.BetLists.StaticData.ColumnIndex;
using Alpha.BetLists.StaticData.Helpers;
using Fanex.BetList.Core.Entities;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Alpha.BetLists.StaticData.DataAccesses
{
    public class TicketExcelDataAccess
    {
        private string fileName;
        private IWorkbook workbook;
        private IFormulaEvaluator formulaEvaluator;
        private DataFormatter dataFormatter;

        public TicketExcelDataAccess(string fileName)
        {
            this.fileName = fileName;
        }

        public IEnumerable<TicketDto> GetTickets()
        {
            LazyLoadWorkbook();

            var betTicketSheet = workbook.GetSheet("BetTickets");

            IList<TicketDto> tickets = new List<TicketDto>();

            if (betTicketSheet != null)
            {
                betTicketSheet.RemoveRow(betTicketSheet.GetRow(0));

                foreach (IRow row in betTicketSheet)
                {
                    var ticket = MapRowToTicketObject(row);
                    if (ticket != null && ticket.BetTypeId > 0)
                    {
                        tickets.Add(ticket);
                    }
                }
            }

            return tickets;
        }

        public IList<ITicketData> GetTicketData()
        {
            LazyLoadWorkbook();

            var ticketDataSheet = workbook.GetSheet("ReferenceData");

            IList<ITicketData> ticketData = new List<ITicketData>();

            if (ticketDataSheet != null)
            {
                ticketDataSheet.RemoveRow(ticketDataSheet.GetRow(0));

                foreach (IRow row in ticketDataSheet)
                {
                    var data = MapRowToTicketDataObject(row);
                    if (data != null)
                    {
                        ticketData.Add(data);
                    }
                }
            }

            return ticketData;
        }

        private void LazyLoadWorkbook()
        {
            // Check to lazily initialize workbook object
            if (workbook == null)
            {
                // Check if file path is valid
                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    workbook = NpoiHelper.LoadWorkbook(fileName);
                    formulaEvaluator = new HSSFFormulaEvaluator(workbook);
                    dataFormatter = new HSSFDataFormatter(new CultureInfo("en-US"));
                }
            }
        }

        private ITicketData MapRowToTicketDataObject(IRow row)
        {
            ITicketData data = new TicketData
            {
                AgentPT = Convert.ToDecimal(GetCellValue(row.GetCell((int)TicketDataTemplateColumnIndex.AgentPT))),
                BetTeam = GetCellValue(row.GetCell((int)TicketDataTemplateColumnIndex.BetTeam)),
                BetTypeId = Convert.ToInt32(GetCellValue(row.GetCell((int)TicketDataTemplateColumnIndex.BetTypeId))),
                MasterPT = Convert.ToDecimal(GetCellValue(row.GetCell((int)TicketDataTemplateColumnIndex.MasterPT))),
                Odds = Convert.ToDecimal(GetCellValue(row.GetCell((int)TicketDataTemplateColumnIndex.Odds))),
                RefNo = GetCellValue(row.GetCell((int)TicketDataTemplateColumnIndex.RefNo)),
                Stake = Convert.ToDecimal(GetCellValue(row.GetCell((int)TicketDataTemplateColumnIndex.Stake))),
                Status = GetCellValue(row.GetCell((int)TicketDataTemplateColumnIndex.Status)),
                SuperPT = Convert.ToDecimal(GetCellValue(row.GetCell((int)TicketDataTemplateColumnIndex.SuperPT))),
                TransDesc = GetCellValue(row.GetCell((int)TicketDataTemplateColumnIndex.TransDesc))
            };

            return data;
        }

        private TicketDto MapRowToTicketObject(IRow row)
        {
            var ticket = new TicketDto();
            ticket.ActualStake = Convert.ToDecimal(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.ActualStake)));
            ticket.AgentComm = Convert.ToDecimal(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.AgentComm)));
            ticket.AgentDiscount = Convert.ToDecimal(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.AgentDiscount)));
            ticket.AgentPositionTaking = Convert.ToDecimal(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.AgentPositionTaking)));
            ticket.AgentWinlost = Convert.ToDecimal(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.AgentWinlost)));
            ticket.AwayId = Convert.ToInt64(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.AwayId)));
            ticket.BetCheck = GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.BetCheck));
            ticket.BetId = Convert.ToInt64(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.BetId)));
            ticket.BetTeam = GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.BetTeam));
            ticket.BetTypeId = Convert.ToInt32(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.BetTypeId)));
            ticket.Comm = Convert.ToDecimal(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.Comm)));
            ticket.CommStatus = GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.CommStatus));
            ticket.CustId = Convert.ToInt64(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.CustId)));
            ticket.EventDate = Convert.ToDateTime(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.EventDate)));
            ticket.EventStatus = GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.EventStatus));
            ticket.Handicap1 = Convert.ToDecimal(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.Handicap1)));
            ticket.Handicap2 = Convert.ToDecimal(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.Handicap2)));
            ticket.HomeId = Convert.ToInt64(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.HomeId)));
            ticket.IP = GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.IP));
            ticket.IsLive = Convert.ToBoolean(Convert.ToInt32(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.IsLive))));
            ticket.IsNeutral = Convert.ToBoolean(Convert.ToInt32(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.IsNeutral))));
            ticket.LeagueId = Convert.ToInt32(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.LeagueId)));
            ticket.LiveAwayScore = Convert.ToInt32(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.LiveAwayScore)));
            ticket.LiveHomeScore = Convert.ToInt32(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.LiveHomeScore)));
            ticket.MasterDiscount = Convert.ToDecimal(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.MasterDiscount)));
            ticket.MasterOdds = Convert.ToDecimal(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.MasterOdds)));
            ticket.MasterPositionTaking = Convert.ToDecimal(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.MasterPositionTaking)));
            ticket.MasterWinlost = Convert.ToDecimal(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.MasterWinlost)));
            ticket.MatchCode = GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.MatchCode));
            ticket.MatchId = Convert.ToInt32(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.MatchId)));
            ticket.Odds = Convert.ToDecimal(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.Odds)));
            ticket.OddsType = GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.OddsType));
            ticket.PlayerComm = Convert.ToDecimal(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.PlayerComm)));
            ticket.Race = GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.Race));
            ticket.RefNo = GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.RefNo));
            ticket.ShowTime = GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.ShowTime));
            ticket.SportTypeId = Convert.ToInt32(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.SportTypeId)));
            ticket.Stake = Convert.ToDecimal(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.Stake)));
            ticket.Status = GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.Status));
            ticket.SuperComm = Convert.ToDecimal(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.SuperComm)));
            ticket.SuperDiscount = Convert.ToDecimal(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.SuperDiscount)));
            ticket.SuperPositionTaking = Convert.ToDecimal(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.SuperPositionTaking)));
            ticket.SuperWinlost = Convert.ToDecimal(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.SuperWinlost)));
            ticket.TransDate = Convert.ToDateTime(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.TransDate)));
            ticket.TransId = Convert.ToInt64(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.TransId)));
            ticket.UserName = GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.UserName));
            ticket.Winlost = Convert.ToDecimal(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.Winlost)));
            ticket.WinlostDate = Convert.ToDateTime(GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.WinlostDate)));
            ticket.TransDesc = GetCellValue(row.GetCell((int)TicketTemplateColumnIndex.TransDesc));

            return ticket;
        }

        private string GetCellValue(ICell cell)
        {
            return NpoiHelper.GetValue(cell, dataFormatter, formulaEvaluator);
        }
    }
}