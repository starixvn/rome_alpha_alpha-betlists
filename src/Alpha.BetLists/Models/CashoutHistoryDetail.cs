using System;
using System.Collections.Generic;
using Alpha.BetLists.AppResources;

namespace Alpha.BetLists.Models
{
    public class CashoutHistoryDetail
    {
        public CashoutHistoryDetail(DateTime changeTime, decimal amount, string actionId)
        {
            ChangeTime = changeTime;
            CashoutAmount = amount;
            ActionId = actionId;
        }
        private DateTime ChangeTime { get; }

#pragma warning disable S1449 // Culture should be specified for "string" operations
        public string ChangeTimeText => ChangeTime.ToString();
#pragma warning restore S1449 // Culture should be specified for "string" operations

        public decimal CashoutAmount { get; }

        private string ActionId { get; }

        public string ActionName

        {
            get
            {
                var actionList = new Dictionary<string, string>
                {
                    { "1", Resources.CashOut },
                    { "6", Resources.CashOutCancelled },
                    { "8", Resources.CashOutRecovered }
                };

                return actionList.ContainsKey(ActionId) ? actionList[ActionId] : ActionId;
            }
        }
    }
}
