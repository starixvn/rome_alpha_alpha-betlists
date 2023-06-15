namespace Alpha.BetLists.Models
{
    public class ProductDetail
    {
        public ProductDetail()
        {
        }

        public ProductDetail(
            string refId,
            int resourceId,
            int productId,
            int sportType,
            int betType,
            string leagueGroupId,
            string ticketType)
        {
            RefId = refId;
            ResourceId = resourceId;
            ProductId = productId;
            SportType = sportType;
            BetType = betType;
            LeagueGroupId = leagueGroupId;
            TicketType = ticketType;
        }

        public string RefId { get; }

        public int ResourceId { get; }

        public string DisplayName { get; set; }

        public int ProductId { get; }

        public int SportType { get; }

        public int BetType { get; }

        public string LeagueGroupId { get; }

        public string[] LeagueGroupIds => LeagueGroupId != null ? LeagueGroupId.Split(',') : new string[] { };

        public string TicketType { get; }

        public string[] TicketTypeIds => TicketType != null ? TicketType.Split(',') : new string[] { };
    }
}
