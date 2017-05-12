namespace Alpha.BetLists.Site.ViewModels.Outstanding
{
    using Components;

    public class OutstandingFilterViewModel
    {
        public OutstandingFilterViewModel(
            int custId,
            string custName,
            DateRangePickerViewModel dateRangePickerViewModel,
            ProductMultiSelectionViewModel productMultiSelectionViewModel)
        {
            CustId = custId;
            CustName = custName;
            DateRangePickerViewModel = dateRangePickerViewModel;
            ProductMultiSelectionViewModel = productMultiSelectionViewModel;
        }

        public int CustId { get; }

        public string CustName { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public ProductMultiSelectionViewModel ProductMultiSelectionViewModel { get; }

        public string ViewDataInstructionMessage { get; }
    }
}