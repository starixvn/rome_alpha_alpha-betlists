namespace Alpha.BetLists.Site.ViewModels.Components
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class StatusMultiSelectionViewModel
    {
        public StatusMultiSelectionViewModel(IEnumerable<SelectListItem> statusListItem, string selectedStatusIds,
            string language)
        {
            StatusListItem = statusListItem;
            SelectedStatusIds = selectedStatusIds;
            Language = language;
        }

        public IEnumerable<SelectListItem> StatusListItem { get; }

        public string SelectedStatusIds { get; }

        public string Language { get; }
    }
}