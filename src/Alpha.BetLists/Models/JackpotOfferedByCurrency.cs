namespace Alpha.BetLists.Models
{
    public class JackpotOfferedByCurrency
    {
        public JackpotOfferedByCurrency()
        {
        }

        public JackpotOfferedByCurrency(int product, int currency)
        {
            Currency = currency;
            Product = product;
        }

        public int Currency { get; set; }

        public int Product { get; set; }
    }
}
