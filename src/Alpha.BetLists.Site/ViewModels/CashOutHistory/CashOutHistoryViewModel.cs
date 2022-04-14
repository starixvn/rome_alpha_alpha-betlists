using Alpha.BetLists.AppResources;
using Alpha.BetLists.Models;
using Fanex.BetList.Core.App_GlobalResources;

namespace Alpha.BetLists.Site.ViewModels.CashOutHistory
{
    public class CashOutHistoryViewModel
    {
        public CashOutHistoryViewModel(CashOutHistoryModel model)
        {
            CashOutModel = model;
            Title = $"{Resources.CashOutHistoryTitle} - {model.TransId}";
        }

        public CashOutHistoryModel CashOutModel { get; }

        public string Title { get; }

        public static int TotalColumn => totalColumn;


        private const int totalColumn = 4;
    }
}