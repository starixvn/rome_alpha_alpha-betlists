namespace Alpha.BetLists.Builders.ExcelBuilders
{
    using System.Collections.Generic;
    using AppResources;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Models;
    using NPOI.SS.UserModel;
    using PTComm;

    public static class BetListExcelBuilderExt
    {
        public static IRichTextString BuildFooterHeader(RTFHelper rtfHelper)
        {
            rtfHelper.RTFRenderer.AddText($"{Resources.Subtotal_wl}:\n");
            rtfHelper.RTFRenderer.AddText($"{Resources.Subtotal_com}:\n");
            rtfHelper.RTFRenderer.AddText($"{Resources.total}:");

            var footerHeaderRtfText = rtfHelper.RTFRenderer.Render();
            rtfHelper.RTFRenderer.Clear();

            return footerHeaderRtfText;
        }

        public static IRichTextString BuildFooter(string totalWinLoss, string totalCommission, string total, RTFHelper rtfHelper)
        {
            var rtfNumber = new RTFNumber(rtfHelper.RTFRenderer, rtfHelper.PosFont, rtfHelper.NegFont);
            rtfNumber.Render(totalWinLoss);
            rtfNumber.Render("\n" + totalCommission);
            rtfNumber.Render("\n" + total);

            var footerRtfText = rtfHelper.RTFRenderer.Render();
            rtfHelper.RTFRenderer.Clear();

            return footerRtfText;
        }

        public static IRichTextString BuildWinLoss(string winLoss, string commission, RTFHelper rtfHelper)
        {
            var rtfNumber = new RTFNumber(rtfHelper.RTFRenderer, rtfHelper.PosFont, rtfHelper.NegFont);

            if (!string.IsNullOrEmpty(winLoss))
            {
                rtfNumber.Render(winLoss);
            }

            if (!string.IsNullOrEmpty(commission))
            {
                rtfNumber.Render("\n" + commission);
            }

            var winLossRtfText = rtfHelper.RTFRenderer.Render();
            rtfHelper.RTFRenderer.Clear();

            return winLossRtfText;
        }

        public static IRichTextString BuildPTCommmission(ITicket ticket, List<ITicketData> ticketData, UserLevel userLevel, RTFHelper rtfHelper)
        {
            var rtfNumber = new RTFNumber(rtfHelper.RTFRenderer, rtfHelper.PosFont, rtfHelper.NegFont);

            var positionTakingComm = PTCommFactory.CreateInstance(ticket, ticketData, userLevel);

            RenderPTPercent(rtfNumber, rtfHelper, ticket, positionTakingComm);
            RenderPTValue(rtfNumber, positionTakingComm);
            RenderCommPercent(rtfNumber, positionTakingComm);
            RenderCommValue(rtfNumber, positionTakingComm);

            var rtfText = rtfHelper.RTFRenderer.Render();
            rtfHelper.RTFRenderer.Clear();

            return rtfText;
        }

        private static void RenderPTPercent(RTFNumber rtfNumber, RTFHelper rtfHelper, ITicket ticket, PTComm positionTakingComm)
        {
            if (ticket.BetTypeId == (33 | 1233))
            {
                RenderRacingPTValue(rtfNumber, rtfHelper, positionTakingComm);
                return;
            }

            if (!string.IsNullOrEmpty(positionTakingComm.PositionTakingPercent) && positionTakingComm.VisibilePositionTakingPercent)
            {
                rtfNumber.Render(positionTakingComm.PositionTakingPercent + "\n");
            }
        }

        private static void RenderPTValue(RTFNumber rtfNumber, PTComm positionTakingComm)
        {
            if (!string.IsNullOrEmpty(positionTakingComm.PositionTaking) && positionTakingComm.VisiblePositionTaking)
            {
                rtfNumber.Render(positionTakingComm.PositionTaking + "\n");
            }
        }

        private static void RenderCommPercent(RTFNumber rtfNumber, PTComm positionTakingComm)
        {
            if (!string.IsNullOrEmpty(positionTakingComm.CommissionPercent) && positionTakingComm.VisibileCommisionPercent)
            {
                rtfNumber.Render(positionTakingComm.CommissionPercent + "\n");
            }
        }

        private static void RenderCommValue(RTFNumber rtfNumber, PTComm positionTakingComm)
        {
            if (!string.IsNullOrEmpty(positionTakingComm.Commission) && positionTakingComm.VisibleCommision)
            {
                rtfNumber.Render(positionTakingComm.Commission + "\n");
            }
        }

        private static void RenderRacingPTValue(RTFNumber rtfNumber, RTFHelper rtfHelper, PTComm positionTakingComm)
        {
            var positionTakingComm33 = positionTakingComm as PTComm33;
            rtfHelper.RTFRenderer.AddText($"{Resources.Win}: ", rtfHelper.PosFont);
            rtfNumber.Render(positionTakingComm33.PositionTakingWin);

            rtfHelper.RTFRenderer.AddText($"\n{Resources.place}: ", rtfHelper.PosFont);
            rtfNumber.Render(positionTakingComm33.PositionTakingPlace + "\n");
        }
    }
}