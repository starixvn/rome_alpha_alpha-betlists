////namespace Alpha.BetLists.Builders.ExcelBuilders
////{
////    using System;
////    using System.Collections.Generic;
////    using System.Linq;
////    using Fanex.BetList.Core.Builder;
////    using Fanex.BetList.Core.Entities;
////    using Fanex.BetList.Core.Utils;
////    using NPOI.HSSF.UserModel;
////    using NPOI.SS.UserModel;
////    using NPOI.SS.Util;

////    /// <summary>
////    /// Extend corebetlist excelbuilder
////    /// </summary>
////    public static class BetListExcelBuilderExt
////    {
////        #region Attributes

////        private const string FOOTER_BUILDER_NAMESPACE = "Alpha.BetLists.Builders.HtmlBuilders.FooterBuilder";
////        private const string SUPERPT_BUILDER_NAMESPACE = "Alpha.BetLists.Builders.HtmlBuilders.SuperPTBuilder";
////        private const string MASTERPT_BUILDER_NAMESPACE = "Alpha.BetLists.Builders.HtmlBuilders.MasterPTBuilder";
////        private const string AGENTPT_BUILDER_NAMESPACE = "Alpha.BetLists.Builders.HtmlBuilders.AgentPTBuilder";

////        #endregion Attributes

////        /// <summary>
////        /// Adds the no.
////        /// </summary>
////        /// <param name="factory">The factory.</param>
////        /// <param name="index">The index.</param>
////        /// <returns></returns>
////        public static BetListExcelBuilder AddNo(this BetListExcelBuilder factory, int index)
////        {
////            IRichTextString rtfNo = new HSSFRichTextString(index.ToString());
////            factory.AddCell(rtfNo);
////            return factory;
////        }

////        /// <summary>
////        /// Adds the win loss.
////        /// </summary>
////        /// <param name="factory">The factory.</param>
////        /// <param name="ticket">The ticket.</param>
////        /// <returns></returns>
////        public static BetListExcelBuilder AddWinLoss(this BetListExcelBuilder factory, ITicket ticket)
////        {
////            IWinLoss winlossBuilder = CreateWinLossBuilder();
////            WinLoss_Template resultTemplate = winlossBuilder.Render(ticket);

////            RTFNumber rtfNumber = new RTFNumber(factory.RTFHelper.RTFRenderer, factory.RTFHelper.PosFont, factory.RTFHelper.NegFont);

////            string winLoss = string.Join(null, new string[] { "\n", resultTemplate.WinLoss.winloss_value });
////            rtfNumber.Render(winLoss);

////            if (resultTemplate.Comm.Visible && !string.IsNullOrEmpty(resultTemplate.Comm))
////            {
////                string comm = string.Join(null, new string[] { "\n", resultTemplate.Comm.comm_value });
////                rtfNumber.Render(comm);
////            }

////            IRichTextString rtfWinLoss = factory.RTFHelper.RTFRenderer.Render();
////            factory.RTFHelper.RTFRenderer.Clear();
////            factory.AddCell(rtfWinLoss);

////            return factory;
////        }

////        /// <summary>
////        /// Adds the super PT.
////        /// </summary>
////        /// <param name="factory">The factory.</param>
////        /// <param name="ticket">The ticket.</param>
////        /// <returns></returns>
////        public static BetListExcelBuilder AddSuperPT(this BetListExcelBuilder factory, ITicket ticket, List<ITicketData> ticketData)
////        {
////            ISuperPT superPTBuilder = CreateSuperPTBuilder(ticket.BetTypeId);
////            SuperPT_Template resultTemplate = superPTBuilder.Render(ticket, ticketData);

////            RTFNumber rtfNumber = new RTFNumber(factory.RTFHelper.RTFRenderer, factory.RTFHelper.PosFont, factory.RTFHelper.NegFont);

////            string superPtComm = string.Empty;

////            if (resultTemplate.SuperPT != null && resultTemplate.SuperPT.Visible)
////            {
////                // Special case for Win/Place bettype
////                if (ticket.BetTypeId == 33 || ticket.BetTypeId == 1233)
////                {
////                    int breakPos = resultTemplate.SuperPT.superPT_setting.IndexOf("<br>");

////                    string superPT_WinString = resultTemplate.SuperPT.superPT_setting.Substring(0, breakPos);
////                    string superPT_PlaceString = resultTemplate.SuperPT.superPT_setting.Replace(superPT_WinString, string.Empty);
////                    string superPT_WinVal = ExtractStringBetweenTag("b", superPT_WinString);
////                    string superPT_PlaceVal = ExtractStringBetweenTag("b", superPT_PlaceString);

////                    //Add Win
////                    factory.RTFHelper.RTFRenderer.AddText(string.Join(null, new string[] { LocalResources.BetListLang.Win, ": " }), factory.RTFHelper.PosFont);
////                    rtfNumber.Render(superPT_WinVal);

////                    //Add Place
////                    factory.RTFHelper.RTFRenderer.AddText(string.Join(null, new string[] { "\n", LocalResources.BetListLang.place, ": " }), factory.RTFHelper.PosFont);
////                    rtfNumber.Render(superPT_PlaceVal);
////                    factory.RTFHelper.RTFRenderer.AddText("\n");
////                }
////                else
////                {
////                    superPtComm = string.Join(null, new string[] { resultTemplate.SuperPT.superPT_setting, "\n" });
////                    factory.RTFHelper.RTFRenderer.AddText(superPtComm, factory.RTFHelper.PosFont);
////                }
////            }

////            if (resultTemplate.SuperPTValue != null && resultTemplate.SuperPTValue.Visible)
////            {
////                superPtComm = string.Join(null, new string[] { resultTemplate.SuperPTValue.superPT_value, "\n" });
////                rtfNumber.Render(superPtComm);
////            }

////            if (resultTemplate.SuperComm != null && resultTemplate.SuperComm.Visible)
////            {
////                superPtComm = string.Join(null, new string[] { resultTemplate.SuperComm.superComm_setting, "\n" });
////                factory.RTFHelper.RTFRenderer.AddText(superPtComm);
////            }

////            if (resultTemplate.SuperCommValue != null && resultTemplate.SuperCommValue.Visible)
////            {
////                superPtComm = resultTemplate.SuperCommValue.superComm_value;
////                factory.RTFHelper.RTFRenderer.AddText(superPtComm);
////            }

////            IRichTextString rtfSuperPT = factory.RTFHelper.RTFRenderer.Render();
////            factory.RTFHelper.RTFRenderer.Clear();
////            factory.AddCell(rtfSuperPT);

////            return factory;
////        }

////        /// <summary>
////        /// Adds the master PT.
////        /// </summary>
////        /// <param name="factory">The factory.</param>
////        /// <param name="ticket">The ticket.</param>
////        /// <returns></returns>
////        public static BetListExcelBuilder AddMasterPT(this BetListExcelBuilder factory, ITicket ticket, List<ITicketData> ticketData)
////        {
////            IMasterPT masterPTBuilder = CreateMasterPTBuilder(ticket.BetTypeId);
////            MasterPT_Template resultTemplate = masterPTBuilder.Render(ticket, ticketData);

////            RTFNumber rtfNumber = new RTFNumber(factory.RTFHelper.RTFRenderer, factory.RTFHelper.PosFont, factory.RTFHelper.NegFont);

////            string masterPtComm = string.Empty;

////            if (resultTemplate.MasterPT != null && resultTemplate.MasterPT.Visible)
////            {
////                // Special case for Win/Place bettype
////                if (ticket.BetTypeId == 33 || ticket.BetTypeId == 1233)
////                {
////                    int breakPos = resultTemplate.MasterPT.masterPT_setting.IndexOf("<br>");

////                    string masterPT_WinString = resultTemplate.MasterPT.masterPT_setting.Substring(0, breakPos);
////                    string masterPT_PlaceString = resultTemplate.MasterPT.masterPT_setting.Replace(masterPT_WinString, string.Empty);
////                    string masterPT_WinVal = ExtractStringBetweenTag("b", masterPT_WinString);
////                    string masterPT_PlaceVal = ExtractStringBetweenTag("b", masterPT_PlaceString);

////                    //Add Win
////                    factory.RTFHelper.RTFRenderer.AddText(string.Join(null, new string[] { LocalResources.BetListLang.Win, ": " }), factory.RTFHelper.PosFont);
////                    rtfNumber.Render(masterPT_WinVal);

////                    //Add Place
////                    factory.RTFHelper.RTFRenderer.AddText(string.Join(null, new string[] { "\n", LocalResources.BetListLang.place, ": " }), factory.RTFHelper.PosFont);
////                    rtfNumber.Render(masterPT_PlaceVal);
////                    factory.RTFHelper.RTFRenderer.AddText("\n");
////                }
////                else
////                {
////                    masterPtComm = resultTemplate.MasterPT.masterPT_setting + "\n";
////                    factory.RTFHelper.RTFRenderer.AddText(masterPtComm, factory.RTFHelper.PosFont);
////                }
////            }

////            if (resultTemplate.MasterPTValue != null && resultTemplate.MasterPTValue.Visible)
////            {
////                masterPtComm = resultTemplate.MasterPTValue.masterPT_value + "\n";
////                rtfNumber.Render(masterPtComm);
////            }

////            if (resultTemplate.MasterComm != null && resultTemplate.MasterComm.Visible)
////            {
////                masterPtComm = resultTemplate.MasterComm.masterComm_setting + "\n";
////                factory.RTFHelper.RTFRenderer.AddText(masterPtComm);
////            }

////            if (resultTemplate.MasterCommValue != null && resultTemplate.MasterCommValue.Visible)
////            {
////                masterPtComm = resultTemplate.MasterCommValue.masterComm_value;
////                factory.RTFHelper.RTFRenderer.AddText(masterPtComm);
////            }

////            IRichTextString rtfMasterPT = factory.RTFHelper.RTFRenderer.Render();
////            factory.RTFHelper.RTFRenderer.Clear();
////            factory.AddCell(rtfMasterPT);

////            return factory;
////        }

////        /// <summary>
////        /// Adds the agent PT.
////        /// </summary>
////        /// <param name="factory">The factory.</param>
////        /// <param name="ticket">The ticket.</param>
////        /// <returns></returns>
////        public static BetListExcelBuilder AddAgentPT(this BetListExcelBuilder factory, ITicket ticket, List<ITicketData> ticketData)
////        {
////            IAgentPT agentPTBuilder = CreateAgentPTBuilder(ticket.BetTypeId);
////            AgentPT_Template resultTemplate = agentPTBuilder.Render(ticket, ticketData);

////            RTFNumber rtfNumber = new RTFNumber(factory.RTFHelper.RTFRenderer, factory.RTFHelper.PosFont, factory.RTFHelper.NegFont);

////            string agentPtComm = string.Empty;

////            if (resultTemplate.AgentPT != null && resultTemplate.AgentPT.Visible)
////            {
////                // Special case for Win/Place bettype
////                if (ticket.BetTypeId == 33 || ticket.BetTypeId == 1233)
////                {
////                    int breakPos = resultTemplate.AgentPT.agentPT_setting.IndexOf("<br>");

////                    string agentPT_WinString = resultTemplate.AgentPT.agentPT_setting.Substring(0, breakPos);
////                    string agentPT_PlaceString = resultTemplate.AgentPT.agentPT_setting.Replace(agentPT_WinString, string.Empty);
////                    string agentPT_WinVal = ExtractStringBetweenTag("b", agentPT_WinString);
////                    string agentPT_PlaceVal = ExtractStringBetweenTag("b", agentPT_PlaceString);

////                    //Add Win
////                    factory.RTFHelper.RTFRenderer.AddText(string.Join(null, new string[] { LocalResources.BetListLang.Win, ": " }), factory.RTFHelper.PosFont);
////                    rtfNumber.Render(agentPT_WinVal);

////                    //Add Place
////                    factory.RTFHelper.RTFRenderer.AddText(string.Join(null, new string[] { "\n", LocalResources.BetListLang.place, ": " }), factory.RTFHelper.PosFont);
////                    rtfNumber.Render(agentPT_PlaceVal);
////                    factory.RTFHelper.RTFRenderer.AddText("\n");
////                }
////                else
////                {
////                    agentPtComm = resultTemplate.AgentPT.agentPT_setting + "\n";
////                    factory.RTFHelper.RTFRenderer.AddText(agentPtComm, factory.RTFHelper.PosFont);
////                }
////            }

////            if (resultTemplate.AgentPTValue != null && resultTemplate.AgentPTValue.Visible)
////            {
////                agentPtComm = resultTemplate.AgentPTValue.agentPT_value + "\n";
////                rtfNumber.Render(agentPtComm);
////            }

////            if (resultTemplate.AgentComm != null && resultTemplate.AgentComm.Visible)
////            {
////                agentPtComm = resultTemplate.AgentComm.agentComm_setting + "\n";
////                factory.RTFHelper.RTFRenderer.AddText(agentPtComm);
////            }

////            if (resultTemplate.AgentCommValue != null && resultTemplate.AgentCommValue.Visible)
////            {
////                agentPtComm = resultTemplate.AgentCommValue.agentComm_value;
////                factory.RTFHelper.RTFRenderer.AddText(agentPtComm);
////            }

////            var rtfAgentPT = factory.RTFHelper.RTFRenderer.Render();
////            factory.RTFHelper.RTFRenderer.Clear();
////            factory.AddCell(rtfAgentPT);

////            return factory;
////        }

////        /// <summary>
////        /// Adds the member.
////        /// </summary>
////        /// <param name="factory">The factory.</param>
////        /// <param name="ticket">The ticket.</param>
////        /// <returns></returns>
////        public static BetListExcelBuilder AddMember(this BetListExcelBuilder factory, ITicket ticket, ITicketHelper ticketHelper)
////        {
////            IRichTextString rtfMember = new HSSFRichTextString(ticket.UserName);
////            factory.AddCell(rtfMember, ticketHelper.Index % 2 == 0 ? factory.OddCellStyleCenterAligned : factory.EvenCellStyleCenterAligned);

////            return factory;
////        }

////        /// <summary>
////        /// Adds the caption.
////        /// </summary>
////        /// <param name="factory">The factory.</param>
////        /// <param name="caption">The caption.</param>
////        /// <param name="columnsNum">The columns num.</param>
////        /// <returns></returns>
////        public static BetListExcelBuilder AddCaption(this BetListExcelBuilder factory, string caption, int columnsNum)
////        {
////            ICellStyle captionStyle = factory.CreateCellStyle();
////            captionStyle.CloneStyleFrom(factory.OddCellStyle);
////            captionStyle.Alignment = HorizontalAlignment.Left;
////            captionStyle.SetFont(factory.RTFHelper.PosFont);

////            IRichTextString rtfCaption = new HSSFRichTextString(string.Join(null, new string[] { "        ", caption.ToUpper() }));

////            CellAddress headerCellAddress = factory.AddCell(rtfCaption, captionStyle);
////            CellRangeAddress mergeRange = new CellRangeAddress(headerCellAddress.RowIndex, headerCellAddress.RowIndex, 0, columnsNum - 1);
////            factory.MergeCell(mergeRange, captionStyle);

////            return factory;
////        }

////        /// <summary>
////        /// Adds the header.
////        /// </summary>
////        /// <param name="factory">The factory.</param>
////        /// <param name="isShowWinLost">if set to <c>true</c> [is show win lost].</param>
////        /// <param name="isShowSuperPT">if set to <c>true</c> [is show super PT].</param>
////        /// <param name="isShowMasterPT">if set to <c>true</c> [is show master PT].</param>
////        /// <param name="isShowAgentPT">if set to <c>true</c> [is show agent PT].</param>
////        /// <param name="isShowMember">if set to <c>true</c> [is show member].</param>
////        /// <returns></returns>
////        public static BetListExcelBuilder AddHeader(this BetListExcelBuilder factory, bool isShowWinLost, bool isShowSuperPT, bool isShowMasterPT, bool isShowAgentPT, bool isShowMember)
////        {
////            ICellStyle headerStyle = factory.CreateCellStyle();
////            headerStyle.CloneStyleFrom(factory.EvenCellStyle);
////            headerStyle.FillForegroundColor = 44;
////            headerStyle.Alignment = HorizontalAlignment.Center;
////            headerStyle.VerticalAlignment = VerticalAlignment.Center;
////            headerStyle.SetFont(factory.RTFHelper.PosFont);

////            //Set Width
////            CellAddress cellAddr;
////            cellAddr = factory.AddCell(new HSSFRichTextString("#"), headerStyle);
////            factory.SetColumnWidth(cellAddr.ColumnIndex, 1200);

////            if (isShowMember)
////            {
////                cellAddr = factory.AddCell(new HSSFRichTextString(LocalResources.BetListLang.member), headerStyle);
////                factory.SetColumnWidth(cellAddr.ColumnIndex, 4800);
////            }

////            cellAddr = factory.AddCell(new HSSFRichTextString(LocalResources.BetListLang.transtime), headerStyle);
////            factory.SetColumnWidth(cellAddr.ColumnIndex, 5400);

////            cellAddr = factory.AddCell(new HSSFRichTextString(LocalResources.BetListLang.choice), headerStyle);
////            factory.SetColumnWidth(cellAddr.ColumnIndex, 15500);

////            cellAddr = factory.AddCell(new HSSFRichTextString(LocalResources.BetListLang.odds), headerStyle);
////            factory.SetColumnWidth(cellAddr.ColumnIndex, 2400);

////            cellAddr = factory.AddCell(new HSSFRichTextString(LocalResources.BetListLang.stake), headerStyle);
////            factory.SetColumnWidth(cellAddr.ColumnIndex, 2400);

////            if (isShowWinLost)
////            {
////                cellAddr = factory.AddCell(new HSSFRichTextString(LocalResources.BetListLang.winloss), headerStyle);
////                factory.SetColumnWidth(cellAddr.ColumnIndex, 3400);
////            }

////            cellAddr = factory.AddCell(new HSSFRichTextString(LocalResources.BetListLang.status), headerStyle);
////            factory.SetColumnWidth(cellAddr.ColumnIndex, 3400);

////            if (isShowSuperPT)
////            {
////                cellAddr = factory.AddCell(new HSSFRichTextString(LocalResources.BetListLang.superPTcomm.Replace("<br/>", "\n")), headerStyle);
////                factory.SetColumnWidth(cellAddr.ColumnIndex, 3400);
////            }

////            if (isShowMasterPT)
////            {
////                cellAddr = factory.AddCell(new HSSFRichTextString(LocalResources.BetListLang.masterPTcomm.Replace("<br/>", "\n")), headerStyle);
////                factory.SetColumnWidth(cellAddr.ColumnIndex, 3400);
////            }

////            if (isShowAgentPT)
////            {
////                cellAddr = factory.AddCell(new HSSFRichTextString(LocalResources.BetListLang.agentPTcomm.Replace("<br/>", "\n")), headerStyle);
////                factory.SetColumnWidth(cellAddr.ColumnIndex, 3400);
////            }

////            return factory;
////        }

////        /// <summary>
////        /// Adds the footer.
////        /// </summary>
////        /// <param name="factory">The factory.</param>
////        /// <param name="tickets">The tickets.</param>
////        /// <param name="isShowMember">if set to <c>true</c> [is show member].</param>
////        /// <param name="totalColumns">The total columns.</param>
////        /// <returns></returns>
////        public static BetListExcelBuilder AddFooter(this BetListExcelBuilder factory, IList<ITicket> tickets, bool isShowMember, int totalColumns)
////        {
////            ICellStyle footerStyle = factory.CreateCellStyle();
////            footerStyle.CloneStyleFrom(factory.EvenCellStyle);
////            footerStyle.FillForegroundColor = 47;
////            footerStyle.Alignment = HorizontalAlignment.Right;
////            footerStyle.BorderTop = BorderStyle.None;
////            footerStyle.BorderLeft = BorderStyle.None;
////            footerStyle.BorderBottom = BorderStyle.None;
////            footerStyle.SetFont(factory.RTFHelper.PosFont);

////            var rtfNumberRenderer = new RTFNumber(factory.RTFHelper.RTFRenderer, factory.RTFHelper.PosFont, factory.RTFHelper.NegFont);

////            string[] excludeStatuses = new string[] { BetStatus.Running, BetStatus.Waiting };
////            IList<ITicket> filteredTickets = tickets.Where(ticket => !excludeStatuses.Contains(ticket.Status.ToLower())).ToList();

////            decimal sumWinLoss = filteredTickets.Sum(ticket => IsIncludeWinLoss(ticket) ? ticket.Winlost : 0);
////            decimal sumPlayerComm = filteredTickets.Sum(ticket => IsIncludeComm(ticket) ? ticket.PlayerComm : 0);

////            HSSFRichTextString rtfSubtotalWinlossText = new HSSFRichTextString(string.Join(null, new string[] { LocalResources.BetListLang.subtotalWinlost, ":" }));

////            rtfNumberRenderer.Render(ConvertByBetType.Winloss(sumWinLoss));

////            IRichTextString rtfSubtotalWinlossValue = factory.RTFHelper.RTFRenderer.Render();
////            factory.RTFHelper.RTFRenderer.Clear();

////            HSSFRichTextString rtfSubtotalCommText = new HSSFRichTextString(string.Join(null, new string[] { LocalResources.BetListLang.subtotalCom, ":" }));

////            rtfNumberRenderer.Render(ConvertByBetType.Comm(sumPlayerComm));
////            IRichTextString rtfSubtotalCommValue = factory.RTFHelper.RTFRenderer.Render();
////            factory.RTFHelper.RTFRenderer.Clear();

////            HSSFRichTextString rtfBetListTotalText = new HSSFRichTextString(string.Join(null, new string[] { LocalResources.BetListLang.total, ":" }));

////            rtfNumberRenderer.Render(ConvertByBetType.Winloss(sumWinLoss + sumPlayerComm));

////            IRichTextString rtfBetListTotalValue = factory.RTFHelper.RTFRenderer.Render();
////            factory.RTFHelper.RTFRenderer.Clear();

////            List<KeyValuePair<IRichTextString, IRichTextString>> footerContent = new List<KeyValuePair<IRichTextString, IRichTextString>>() {
////                new KeyValuePair<IRichTextString,IRichTextString>(rtfSubtotalWinlossText,rtfSubtotalWinlossValue),
////                new KeyValuePair<IRichTextString,IRichTextString>(rtfSubtotalCommText,rtfSubtotalCommValue)
////            };

////            int lastRowIndex;
////            CellRangeAddress subtotalRange, emtyCellsRange;
////            int subtotalLastCellIndex = isShowMember ? 5 : 4;

////            foreach (KeyValuePair<IRichTextString, IRichTextString> pair in footerContent)
////            {
////                CellAddress firstCell = factory.AddCell(pair.Key, footerStyle);
////                lastRowIndex = factory.LastContentRow;
////                subtotalRange = new CellRangeAddress(lastRowIndex, lastRowIndex, 0, subtotalLastCellIndex);
////                emtyCellsRange = new CellRangeAddress(lastRowIndex, lastRowIndex, subtotalLastCellIndex + 2, totalColumns - 1);
////                factory.MergeCell(subtotalRange, footerStyle);
////                factory.AddCell(pair.Value, footerStyle);
////                factory.AddCell(new HSSFRichTextString(string.Empty), footerStyle);
////                factory.MergeCell(emtyCellsRange, footerStyle);
////            }

////            //Total row with bottom bordered

////            ICellStyle totalCellStyle = factory.CreateCellStyle();
////            totalCellStyle.CloneStyleFrom(footerStyle);
////            totalCellStyle.BorderBottom = BorderStyle.Thin;
////            totalCellStyle.SetFont(factory.RTFHelper.PosFont);

////            CellAddress cell = factory.AddCell(rtfBetListTotalText, totalCellStyle);
////            lastRowIndex = factory.LastContentRow;
////            subtotalRange = new CellRangeAddress(lastRowIndex, lastRowIndex, 0, subtotalLastCellIndex);
////            emtyCellsRange = new CellRangeAddress(lastRowIndex, lastRowIndex, subtotalLastCellIndex + 2, totalColumns - 1);
////            factory.MergeCell(subtotalRange, totalCellStyle);
////            factory.AddCell(rtfBetListTotalValue, totalCellStyle);
////            factory.AddCell(new HSSFRichTextString(string.Empty), totalCellStyle);
////            factory.MergeCell(emtyCellsRange, totalCellStyle);

////            return factory;
////        }

////        /// <summary>
////        /// Creates the win loss builder.
////        /// </summary>
////        /// <returns></returns>
////        private static IWinLoss CreateWinLossBuilder()
////        {
////            return new WinLost1();
////        }

////        /// <summary>
////        /// Creates the master PT builder.
////        /// </summary>
////        /// <param name="bettype">The bettype.</param>
////        /// <returns></returns>
////        private static IMasterPT CreateMasterPTBuilder(int bettype)
////        {
////            string typeName = string.Join(
////                                            null,
////                                            new string[]
////                                            {
////                                                MASTERPT_BUILDER_NAMESPACE,
////                                                ".MasterPT",
////                                                bettype.ToString()
////                                            }
////                                         );

////            Type type = Type.GetType(typeName);

////            if (type == null)
////            {
////                return new MasterPT1();
////            }
////            else
////            {
////                return Activator.CreateInstance(type) as IMasterPT;
////            }
////        }

////        /// <summary>
////        /// Creates the super PT builder.
////        /// </summary>
////        /// <param name="bettype">The bettype.</param>
////        /// <returns></returns>
////        private static ISuperPT CreateSuperPTBuilder(int bettype)
////        {
////            string typeName = string.Join(
////                                            null,
////                                            new string[]
////                                            {
////                                                SUPERPT_BUILDER_NAMESPACE,
////                                                ".SuperPT",
////                                                bettype.ToString()
////                                            }
////                                         );

////            Type type = Type.GetType(typeName);

////            if (type == null)
////            {
////                return new SuperPT1();
////            }
////            else
////            {
////                return Activator.CreateInstance(type) as ISuperPT;
////            }
////        }

////        /// <summary>
////        /// Creates the agent PT builder.
////        /// </summary>
////        /// <param name="bettype">The bettype.</param>
////        /// <returns></returns>
////        private static IAgentPT CreateAgentPTBuilder(int bettype)
////        {
////            string typeName = string.Join(
////                                           null,
////                                           new string[]
////                                            {
////                                                AGENTPT_BUILDER_NAMESPACE,
////                                                ".AgentPT",
////                                                bettype.ToString()
////                                            }
////                                        );

////            Type type = Type.GetType(typeName);

////            if (type == null)
////            {
////                return new AgentPT1();
////            }
////            else
////            {
////                return Activator.CreateInstance(type) as IAgentPT;
////            }
////        }

////        /// <summary>
////        /// Creates the footer builder.
////        /// </summary>
////        /// <param name="footerType">Type of the footer.</param>
////        /// <returns></returns>
////        private static Alpha.BetLists.Builders.HtmlBuilders.FooterBuilder.IFooter CreateFooterBuilder(int footerType)
////        {
////            return new Footer1();
////        }

////        /// <summary>
////        /// Extract string value between tags from a source string
////        /// </summary>
////        /// <param name="tag">tag name</param>
////        /// <param name="source">source string</param>
////        /// <returns></returns>
////        private static string ExtractStringBetweenTag(string tag, string source)
////        {
////            string startTag = string.Join(null, new string[] { "<", tag, ">" });
////            string endTag = string.Join(null, new string[] { "</", tag, ">" });

////            int startIndex = source.IndexOf(startTag) + startTag.Length;
////            int endIndex = source.IndexOf(endTag, startIndex);
////            return source.Substring(startIndex, endIndex - startIndex);
////        }

////        private static bool IsIncludeWinLoss(ITicket ticket)
////        {
////            var status = ticket.Status.ToLower();
////            return !status.Equals(BetStatus.Running) &&
////                !status.Equals(BetStatus.Waiting) &&
////                !status.Equals(BetStatus.Void) &&
////                !status.Equals(BetStatus.Reject) &&
////                !ticket.BetTypeId.Equals(500);
////        }

////        private static bool IsIncludeComm(ITicket ticket)
////        {
////            var status = ticket.Status.ToLower();
////            return !status.Equals(BetStatus.Running) &&
////                !status.Equals(BetStatus.Waiting) &&
////                !status.Equals(BetStatus.Void) &&
////                !status.Equals(BetStatus.Reject);
////        }
////    }
////}