namespace Alpha.BetLists.Models
{
    public class ProductDetail
    {
        public ProductDetail()
        {
        }

        public ProductDetail(
            int resourceId,
            int productId,
            int sportType,
            int betType)
        {
            ResourceId = resourceId;
            ProductId = productId;
            SportType = sportType;
            BetType = betType;
        }

        public int ResourceId { get; }

        public int ProductId { get; }

        public int SportType { get; }

        public int BetType { get; }
    }
}
