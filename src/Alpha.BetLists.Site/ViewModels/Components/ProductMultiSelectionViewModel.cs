namespace Alpha.BetLists.Site.ViewModels.Components
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class ProductMultiSelectionViewModel
    {
        public ProductMultiSelectionViewModel(
            IEnumerable<SelectListItem> products,
            string productIds,
            string language)
        {
            Products = products;
            ProductIds = productIds;
            Language = language;
        }

        public IEnumerable<SelectListItem> Products { get; }

        public string Language { get; }

        public string ProductIds { get; }
    }
}