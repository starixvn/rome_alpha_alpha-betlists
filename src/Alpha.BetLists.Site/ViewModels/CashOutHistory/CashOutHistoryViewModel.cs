using Alpha.BetLists.Models;

namespace Alpha.BetLists.Site.ViewModels.CashOutHistory
{
    public class CashOutHistoryViewModel
    {
        public CashOutHistoryViewModel(CashOutHistoryModel model)
        {
            CashOutModel = model;
            Title = $"{AppResources.Resources.CashOutHistoryTitle} - {model.TransId}";
        }

        public CashOutHistoryModel CashOutModel { get; }

        public string Title { get; }

        public static int TotalColumn => totalColumn;


        private const int totalColumn = 4;
    }
}