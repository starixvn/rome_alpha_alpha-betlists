namespace Alpha.BetLists.Site.ViewModels.Components
{
    using System.Collections.Generic;

    public class ProductMultiSelectionViewModel
    {
        public ProductMultiSelectionViewModel(
            IEnumerable<GroupProductViewModel> groupProducts,
            string productIds,
            string language)
        {
            GroupProducts = groupProducts;
            ProductIds = productIds;
            Language = language;
        }

        public IEnumerable<GroupProductViewModel> GroupProducts { get; }

        public string Language { get; }

        public string ProductIds { get; }
    }
}