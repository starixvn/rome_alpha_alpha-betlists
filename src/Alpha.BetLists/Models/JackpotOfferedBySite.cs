namespace Alpha.BetLists.Models
{
    public class JackpotOfferedBySite
    {
        public JackpotOfferedBySite()
        {

        }

        public JackpotOfferedBySite(int product, string site)
        {
            Site = site;
            Product = product;
        }

        public string Site { get; }

        public int Product { get; }
    }
}
