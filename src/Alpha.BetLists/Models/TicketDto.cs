namespace Alpha.BetLists.Models
{
    using Alpha.BetLists.AppResources;
    using Fanex.BetList.Core.Entities;
    using System.Resources;

    public class TicketDto : Ticket
    {
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