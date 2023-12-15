using Alpha.BetLists.Models;
using Fanex.BetList.Core.Entities;
using Fanex.Common;
using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace Alpha.BetLists.DataAccesses
{
    public interface IBetListRepository
    {
        IList<TicketDto> Fetch(ICriteria criteria);

        (IList<TicketDto>, List<TicketData>) FetchMultiple(ICriteria criteria);
    }

    public class BetListRepository : IBetListRepository
    {
        private readonly Func<DataRow, TicketDto> mapTicketObjectFunction = r => new TicketDto
        {
            ActualStake = r.Table.Columns.Contains("actual_stake") ? Converter.To<decimal>(r["actual_stake"]) : 0,

            AgentWinlost = r.Table.Columns.Contains("awinlost") ? Converter.ToDecimal(r["awinlost"]) : 0,
            AgentPositionTaking = r.Table.Columns.Contains("apositiontaking") ? Converter.ToDecimal(r["apositiontaking"]) : 0,
            Comm = r.Table.Columns.Contains("comm") ? Converter.ToDecimal(r["comm"]) : 0,
            AgentDiscount = r.Table.Columns.Contains("adiscount") ? Converter.ToDecimal(r["adiscount"]) : 0,

            AwayId = r.Table.Columns.Contains("awayid") ? Converter.To<long>(r["awayid"]) : 0,
            BetId = r.Table.Columns.Contains("betid") ? Converter.To<long>(r["betid"]) : 0,
            BetTeam = r.Table.Columns.Contains("betteam") ? Converter.ToString2(r["betteam"]) : string.Empty,
            BetTypeId = r.Table.Columns.Contains("bettype") ? Converter.To<int>(r["bettype"]) : 0,
            CommStatus = r.Table.Columns.Contains("comstatus") ? Converter.To<string>(r["comstatus"]) : string.Empty,
            CustId = r.Table.Columns.Contains("custid") ? Converter.To<long>(r["custid"]) : 0,
            Handicap1 = r.Table.Columns.Contains("hdp1") ? Converter.ToDecimal(r["hdp1"]) : 0,
            Handicap2 = r.Table.Columns.Contains("hdp2") ? Converter.ToDecimal(r["hdp2"]) : 0,
            HomeId = r.Table.Columns.Contains("homeid") ? Converter.To<long>(r["homeid"]) : 0,
            EventDate = r.Table.Columns.Contains("eventdate") ? Converter.ToDateTime(r["eventdate"]) : DateTime.Now,
            IP = r.Table.Columns.Contains("ip") ? Converter.ToString2(r["ip"]) : string.Empty,
            IsLive = r.Table.Columns.Contains("liveindicator") && Converter.ToBool(r["liveindicator"]),
            IsNeutral = r.Table.Columns.Contains("isNeutral") && Converter.ToBool(r["isNeutral"]),
            LeagueId = r.Table.Columns.Contains("leagueid") ? Converter.To<int>(r["leagueid"]) : 0,
            LiveHomeScore = r.Table.Columns.Contains("livehomescore") ? Converter.To<int>(r["livehomescore"]) : 0,
            LiveAwayScore = r.Table.Columns.Contains("liveawayscore") ? Converter.To<int>(r["liveawayscore"]) : 0,

            MasterWinlost = r.Table.Columns.Contains("mwinlost") ? Converter.ToDecimal(r["mwinlost"]) : 0,
            MasterPositionTaking = r.Table.Columns.Contains("mpositiontaking") ? Converter.ToDecimal(r["mpositiontaking"]) : 0,
            AgentComm = r.Table.Columns.Contains("acomm") ? Converter.ToDecimal(r["acomm"]) : 0,
            MasterDiscount = r.Table.Columns.Contains("MDiscount") ? Converter.ToDecimal(r["MDiscount"]) : 0,

            MasterOdds = r.Table.Columns.Contains("modds") ? Converter.ToDecimal(r["modds"]) : 0,
            MatchCode = r.Table.Columns.Contains("matchcode") ? Converter.ToString2(r["matchcode"]) : string.Empty,
            MatchId = r.Table.Columns.Contains("matchid") ? Converter.To<int>(r["matchid"]) : 0,
            Odds = r.Table.Columns.Contains("odds") ? Converter.ToDecimal(r["odds"]) : 0,
            OddsType = r.Table.Columns.Contains("OddsType") ? Converter.ToString2(r["OddsType"]) : string.Empty,
            PlayerComm = r.Table.Columns.Contains("PlayerComm") ? Converter.ToDecimal(r["PlayerComm"]) : 0,
            Race = r.Table.Columns.Contains("race") ? Converter.ToString2(r["race"]) : string.Empty,
            RefNo = r.Table.Columns.Contains("refno") ? Converter.ToString2(r["refno"]) : string.Empty,
            ShowTime = r.Table.Columns.Contains("showtime") ? Converter.ToString2(r["showtime"]) : string.Empty,
            SportTypeId = r.Table.Columns.Contains("sporttype") ? Converter.To<int>(r["sporttype"]) : 0,
            Stake = r.Table.Columns.Contains("stake") ? Converter.To<decimal>(r["stake"]) : 0,
            Status = r.Table.Columns.Contains("status") ? Converter.ToString2(r["status"]) : string.Empty,

            SuperWinlost = r.Table.Columns.Contains("swinlost") ? Converter.ToDecimal(r["swinlost"]) : 0,
            SuperPositionTaking = r.Table.Columns.Contains("spositiontaking") ? Converter.ToDecimal(r["spositiontaking"]) : 0,
            SuperComm = r.Table.Columns.Contains("scomm") ? Converter.ToDecimal(r["scomm"]) : 0,
            SuperDiscount = r.Table.Columns.Contains("sdiscount") ? Converter.ToDecimal(r["sdiscount"]) : 0,

            TransDate = r.Table.Columns.Contains("transdate") ? Converter.ToDateTime(r["transdate"]) : DateTime.MinValue,
            TransId = r.Table.Columns.Contains("transid") ? Converter.To<long>(r["transid"]) : 0,
            UserName = r.Table.Columns.Contains("username") ? Converter.ToString2(r["username"]) : string.Empty,
            Winlost = r.Table.Columns.Contains("winlost") ? Converter.ToDecimal(r["winlost"]) : 0,
            WinlostDate = r.Table.Columns.Contains("winlostdate") ? Convert.ToDateTime(r["winlostdate"]) : DateTime.MinValue,
            TransDesc = r.Table.Columns.Contains("transdesc") ? Converter.ToString2(r["transdesc"]) : string.Empty,

            BetCheck = r.Table.Columns.Contains("betcheck") ? Converter.ToString2(r["betcheck"]) : string.Empty,
            OddsSpread = r.Table.Columns.Contains("OddsSpread") ? Converter.ToDecimal(r["OddsSpread"]) : 0,

            OriginalBetType = r.Table.Columns.Contains("OriginalBetType") ? Converter.To<int>(r["OriginalBetType"]) : 0,
            BetdaqId = r.Table.Columns.Contains("BetdaqId") ? Converter.To<long>(r["BetdaqId"]) : 0,
            ExtraInfo = r.Table.Columns.Contains("ExtraInfo") ? Converter.ToString2(r["ExtraInfo"]) : string.Empty,

            Currency = r.Table.Columns.Contains("Currency") ? Converter.ToString2(r["Currency"]) : string.Empty,
            CurrencyId = r.Table.Columns.Contains("CurrencyId") ? Converter.ToInt(r["CurrencyId"]) : 0,

            WalletType = r.Table.Columns.Contains("wallettype") ? Converter.To<byte>(r["wallettype"]) : (byte)0,
            RefCode = r.Table.Columns.Contains("refcode") ? Converter.To<long>(r["refcode"]) : 0,
            SettlementStatus = r.Table.Columns.Contains("SettlementStatus") ? Converter.To<byte>(r["SettlementStatus"]) : (byte)0,
            SiteId = r.Table.Columns.Contains("SiteId") ? Converter.ToString2(r["SiteId"]) : "",
            JackpotInfo = r.Table.Columns.Contains("JackpotInfo") ? Converter.ToString2(r["JackpotInfo"]) : string.Empty,
            DeviceType = r.Table.Columns.Contains("DeviceType") && !r.IsNull("DeviceType") ? Converter.To<int>(r["DeviceType"]) : (int?)null,
        };

        public IList<TicketDto> Fetch(ICriteria criteria)
        {
            var tickets = new List<TicketDto>();
            if (!criteria.IsValid())
            {
                Logger.Log.Info(JsonConvert.SerializeObject(criteria, Formatting.Indented));
                return tickets;
            }

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                DataSet dataSet = db.ExecuteDataSet(criteria);
                if (dataSet != null
                    && dataSet.Tables.Count > 0)
                {
                    DataTable currentTable = dataSet.Tables[0];
                    foreach (DataRow row in currentTable.Rows)
                    {
                        tickets.Add(mapTicketObjectFunction(row));
                    }
                }
            }

            return tickets;
        }

        public (IList<TicketDto>, List<TicketData>) FetchMultiple(ICriteria criteria)
        {
            var tickets = new List<TicketDto>();
            var outputList = new List<TicketData>();
            if (!criteria.IsValid())
            {
                Logger.Log.Info(JsonConvert.SerializeObject(criteria, Formatting.Indented));
                return (tickets, outputList);
            }

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                DataSet dataSet = db.ExecuteDataSet(criteria);
                if (dataSet != null
                    && dataSet.Tables.Count > 0)
                {
                    DataTable currentTable = dataSet.Tables[0];
                    foreach (DataRow row in currentTable.Rows)
                    {
                        tickets.Add(mapTicketObjectFunction(row));
                    }

                    if (dataSet.Tables.Count > 1)
                    {
                        DataTable secondTable = dataSet.Tables[1];
                        foreach (DataRow row in secondTable.Rows)
                        {
                            outputList.Add(new TicketData
                            {
                                RefNo = Converter.ToString2(row["refno"]),
                                BetTypeId = Converter.To<int>(row["bettype"]),
                                Status = Converter.ToString2(row["status"]),
                                Odds = Converter.To<decimal>(row["odds"]),
                                Stake = Converter.To<decimal>(row["stake"]),
                                BetTeam = Converter.ToString2(row["betteam"]),
                                TransDesc = Converter.ToString2(row["TransDesc"]),
                                AgentPT = Converter.ToDecimal(row["apositiontaking"]),
                                MasterPT = Converter.ToDecimal(row["mpositiontaking"]),
                                SuperPT = Converter.ToDecimal(row["spositiontaking"]),
                            });
                        }
                    }
                }
            }

            return (tickets, outputList);
        }
    }
}