namespace Alpha.BetLists.Site.Controllers
{
    public class TicketDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal AgentDiscount { get; set; }

        public decimal MasterDiscount { get; set; }
    }

}