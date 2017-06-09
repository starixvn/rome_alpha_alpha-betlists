namespace Alpha.BetLists.Site.ViewModels.Components
{
    using AppResources;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class DateRangePickerViewModel
    {
        public const string TodayOptionValue = "0";
        public const string YesterdayOptionValue = "1";
        private const string ThisWeekOptionValue = "2";
        private const string LastWeekOptionValue = "3";
        private const string ThisMonthOptionValue = "4";
        private const string LastMonthOptionValue = "5";
        private const string SinceLastMonthOptionValue = "6";
        public const string SpecificDateOptionValue = "7";

        public DateRangePickerViewModel(
            DateTime minDate,
            DateTime maxDate,
            DateTime fromDate,
            DateTime toDate,
            int userSelectedDateRangeOption,
            bool isRenderSubmitButton = true,
            IEnumerable<string> visibleOptionValues = null)
        {
            MinDate = minDate;
            MaxDate = maxDate;
            FromDate = fromDate;
            ToDate = toDate;
            UserSelectedDateRangeOption = userSelectedDateRangeOption;

            //// TODO: ricky will refactor here
            RangeOptions = BuildRangeOptions(userSelectedDateRangeOption);

            if (visibleOptionValues != null)
            {
                RangeOptions = FilterRangeOptions(RangeOptions, visibleOptionValues);
            }

            SelectedRangeOption = RangeOptions.FirstOrDefault(item => item.Value == userSelectedDateRangeOption.ToString());

            IsRenderSubmitButton = isRenderSubmitButton;
        }

        public DateTime MinDate { get; }

        public DateTime MaxDate { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int UserSelectedDateRangeOption { get; }

        public bool IsRenderSubmitButton { get; }

        public IEnumerable<SelectListItem> RangeOptions { get; }

        public SelectListItem SelectedRangeOption { get; private set; }

        private IEnumerable<SelectListItem> BuildRangeOptions(int userSelectedDateRangeOption)
        {
            // Cannot use static here because Localization.
            return (new List<SelectListItem>
            {
                new SelectListItem { Text = Resources.Today, Value = TodayOptionValue, Selected = true },
                new SelectListItem { Text = Resources.Yesterday, Value = YesterdayOptionValue },
                new SelectListItem { Text = Resources.ThisWeek, Value = ThisWeekOptionValue },
                new SelectListItem { Text = Resources.LastWeek, Value = LastWeekOptionValue },
                new SelectListItem { Text = Resources.ThisMonth, Value = ThisMonthOptionValue },
                new SelectListItem { Text = Resources.LastMonth, Value = LastMonthOptionValue },
                new SelectListItem { Text = Resources.SinceLastMonth, Value = SinceLastMonthOptionValue },
                new SelectListItem { Text = Resources.SpecificDate, Value = SpecificDateOptionValue }
            }).Select(item =>
            {
                item.Selected = item.Value == userSelectedDateRangeOption.ToString();

                return item;
            });
        }

        private IEnumerable<SelectListItem> FilterRangeOptions(
            IEnumerable<SelectListItem> rangeOptions,
            IEnumerable<string> displayValues)
            => rangeOptions.Where(item => displayValues.Contains(item.Value));
    }
}