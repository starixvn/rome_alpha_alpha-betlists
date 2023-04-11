namespace Alpha.BetLists.Site.ViewModels.Components
{
    using System;
    using Alpha.BetLists.AppResources;

    public class WeekSelectionViewModel
    {
        public WeekSelectionViewModel(
            DateTime fromDate,
            DateTime toDate,
            DateTime? selectedFromDate,
            DateTime? selectedToDate)
        {
            FromDate = fromDate;
            ToDate = toDate;
            IsActive = selectedFromDate.HasValue
                && selectedToDate.HasValue
                && selectedFromDate.Value == fromDate
                && selectedToDate.Value == toDate;
        }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string Display
        {
            get
            {
                var displayToDate = $"{ToDate.ToString("dd")} {Resources.ResourceManager.GetString(ToDate.ToString("MMM"))}";

                return FromDate == ToDate
                    ? displayToDate
                    : $"{FromDate.ToString("dd")} - {displayToDate}";
            }
        }

        public bool IsActive { get; }

        public bool IsEnable { get; set; }
    }
}