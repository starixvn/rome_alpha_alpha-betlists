namespace Alpha.BetLists.Models
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    public class CashOutHistoryModel
    {
        public long TransId { get; set; }

        public string CashoutExtraInfo { get; set; }

        public List<CashoutHistoryDetail> DetailHistory
        {
            get
            {
                var detailHistory = new List<CashoutHistoryDetail>();
                if (!string.IsNullOrEmpty(CashoutExtraInfo))
                {
                    var root = XElement.Parse(CashoutExtraInfo);
                    foreach (XElement item in root.Elements())
                    {
                        var time = item.Attribute("ct").Value;
                        var amount = item.Attribute("w").Value;
                        var action = item.Attribute("s").Value;

                        detailHistory.Add(new CashoutHistoryDetail(DateTime.Parse(time), decimal.Parse(amount), action));
                    }
                }

                return detailHistory;
            }
        }
    }
}
