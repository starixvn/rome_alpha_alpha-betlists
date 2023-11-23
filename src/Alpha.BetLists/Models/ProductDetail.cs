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
            int order,
            int leagueGroupId,
            int ticketType,
            int childResourceId)
        {
            RefId = refId;
            ResourceId = resourceId;
            ProductId = productId;
            SportType = sportType;
            BetType = betType;
            Order = order;
            LeagueGroupId = leagueGroupId;
            TicketType = ticketType;
            ChildResourceId = childResourceId;
        }

        public int Order { get; }
        
        public string RefId { get; set; }


        public int ResourceId { get; set; }

        public string DisplayName { get; set; }

        public int ProductId { get; set; }

        public int SportType { get; set; }

        public int BetType { get; set; }

        public int LeagueGroupId { get; }

        public int TicketType { get; }

        public int ChildResourceId { get; }

        public override string ToString()
            => string.Format(
                    "<ProductDetail ProductId=\"{0}\" Sporttype=\"{1}\" Bettype=\"{2}\" LeagueGroupId=\"{3}\" TicketType=\"{4}\"/>",
                    ProductId,
                    SportType,
                    BetType,
                    LeagueGroupId,
                    TicketType);
    }
}
