namespace Alpha.BetLists.Models.Parlay
{
    using System;

    public class ComboParlayDetail
    {
        public ComboParlayDetail()
        {
        }

        public ComboParlayDetail(
            DateTime winlostDate,
            long transId,
            long serial,
            int statusId,
            string status,
            decimal modds,
            decimal stake,
            decimal winlost,
            decimal playerComm)
        {
            WinLostDate = winlostDate;
            TransId = transId;
            Serial = serial;
            StatusId = statusId;
            Status = status;
            Odds = modds;
            Stake = stake;
            WinLost = winlost;
            PlayerComm = playerComm;
        }

        public DateTime WinLostDate { get; set; }

        public long TransId { get; set; }

        public long Serial { get; set; }

        public int StatusId { get; set; }

        public string Status { get; set; }

        public decimal MasterOdds { get; set; }

        public decimal Odds { get; set; }

        public decimal Stake { get; set; }

        public decimal WinLost { get; set; }

        public decimal PlayerComm { get; set; }

        public decimal ExtraValue { get; set; }
    }
}