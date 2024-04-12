namespace Alpha.BetLists.Models
{
    using Alpha.BetLists.AppResources;
    using Alpha.BetLists.Constants;
    using Fanex.BetList.Core.Entities;

    public class TicketDto : Ticket
    {
        public string BookedBetStatus => BookedBetStatusConstants.All.ContainsKey(StatusId.Value) ? BookedBetStatusConstants.All[StatusId.Value] : Status;

        public string Reason { get; set; }

        public string Country { get; set; }

        public bool IsInWatchlist { get; set; }

        public bool IsHighlight { get; set; }

        public string BetType { get; set; }

        public int? DeviceType { get; set; }

        public string Platform
        {
            get
            {
                if (DeviceType == null)
                {
                    return Resources.Platform_NA;
                }
                
                var deviceInfo = Resources.ResourceManager.GetString(string.Format("Platform_{0}", DeviceType.ToString()));

                return deviceInfo ?? Resources.Platform_Uncategorized;
            }
        }
    }
}