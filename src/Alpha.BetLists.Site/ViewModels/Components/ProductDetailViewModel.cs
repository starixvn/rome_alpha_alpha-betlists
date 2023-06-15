namespace Alpha.BetLists.Site.ViewModels.Components
{
    public class ProductDetailViewModel
    {
        public ProductDetailViewModel(int productId, int id, string name, bool selected)
        {
            ProductId = productId;
            Id = id;
            Name = name;
            Checked = selected;
        }

        public int ProductId { get; }

        public int Id { get; }

        public string Name { get; }

        public bool Checked { get; }
    }
}