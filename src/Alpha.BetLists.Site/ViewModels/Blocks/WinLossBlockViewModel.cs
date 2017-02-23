namespace Alpha.BetLists.Site.ViewModels.Blocks
{
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;

    public class WinLossBlockViewModel
    {
        public WinLossBlockViewModel(ITicket ticket)
        {
            SetData(ticket);
            CommVisibility = DisplayComm(ticket);
        }

        public string WinLoss { get; private set; }

        public bool CommVisibility { get; }

        public string Comm { get; private set; }

        private void SetData(ITicket ticket)
        {
            var status = ticket.Status.ToLower();
            switch (status)
            {
                case BetStatus.Running:
                case BetStatus.Waiting:
                    {
                        WinLoss = "-";
                        Comm = string.Empty;
                    }

                    break;

                case BetStatus.Void:
                    {
                        WinLoss = ticket.BetTypeId.Equals(500) ? "(NW) 0.00" : string.Empty;
                        Comm = "0.00";
                    }

                    break;

                case BetStatus.Reject:
                    {
                        WinLoss = "0.00";
                        Comm = string.Empty;
                    }

                    break;

                default:
                    {
                        string netWin = string.Empty;
                        if (ticket.BetTypeId.Equals(500))
                        {
                            netWin = "(NW) ";
                            ticket.Winlost = ticket.ActualStake;
                            Comm = ConvertByBetType.Winloss(ticket.PlayerComm);
                        }
                        else
                        {
                            Comm = ConvertByBetType.Comm(ticket.PlayerComm);
                        }

                        WinLoss = netWin + ConvertByBetType.Winloss(ticket.Winlost);
                    }

                    break;
            }
        }

        private bool DisplayComm(ITicket ticket)
        {
            var isIMBCMatch = ticket.SuperDiscount < 0 || ticket.MasterDiscount < 0 || ticket.AgentDiscount < 0;
            switch (ticket.BetTypeId)
            {
                case 501: // Match Winner
                    return !isIMBCMatch;

                default:
                    return true;
            }
        }
    }
}