namespace Alpha.BetLists.Site.Tests.ViewModels.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using KellermanSoftware.CompareNetObjects;
    using Site.ViewModels.Components;
    using Xunit;

    [Trait("Category", "Components")]
    public class DateRangePickerViewModelTests
    {
        private readonly CompareLogic compareLogic;

        public DateRangePickerViewModelTests()
        {
            compareLogic = new CompareLogic();
        }

        [Fact]
        public void RangeOptions_NotSupportVisibleOptionValues_ReturnDefaultRangeOptions()
        {
            // Arrange
            var viewModel = new DateRangePickerViewModel(DateTime.Today, DateTime.Today, DateTime.Today, DateTime.Today, 1);
            var expectedRangeOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Today", Value = "0" },
                new SelectListItem { Text = "Yesterday", Value = "1", Selected = true },
                new SelectListItem { Text = "This Week", Value = "2" },
                new SelectListItem { Text = "Last Week", Value = "3" },
                new SelectListItem { Text = "This Month", Value = "4" },
                new SelectListItem { Text = "Last Month", Value = "5" },
                new SelectListItem { Text = "Since Last Month", Value = "6" },
                new SelectListItem { Text = "Specific Dates", Value = "7" }
            };

            // Act
            var actualRangeOptions = viewModel.RangeOptions.ToList();

            // Assert
            Assert.True(compareLogic.Compare(expectedRangeOptions, actualRangeOptions).AreEqual);
        }

        [Fact]
        public void RangeOptions_SupportVisibleOptionValues_ReturnCorrectRangeOptions()
        {
            // Arrange
            var visibleOptionValues = new List<string> { "0", "1", "7" };
            var viewModel = new DateRangePickerViewModel(DateTime.Today, DateTime.Today, DateTime.Today, DateTime.Today, 1, true, visibleOptionValues);
            var expectedRangeOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Today", Value = "0" },
                new SelectListItem { Text = "Yesterday", Value = "1", Selected = true },
                new SelectListItem { Text = "Specific Dates", Value = "7" }
            };

            // Act
            var actualRangeOptions = viewModel.RangeOptions.ToList();

            // Assert
            Assert.True(compareLogic.Compare(expectedRangeOptions, actualRangeOptions).AreEqual);
        }
    }
}