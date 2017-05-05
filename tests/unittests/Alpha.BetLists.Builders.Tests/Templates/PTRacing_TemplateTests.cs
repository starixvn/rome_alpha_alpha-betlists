namespace Alpha.BetLists.Builders.Tests.Templates
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Builders.Templates;
    using Xunit;

    [Trait("Category", "BaseBetList")]
    public class PTRacing_TemplateTests
    {
        private const string PTRacingTemplate =
            "<span class='pt-win blue' style='font-weight:normal;'>Win<b>{0}</b></span>" +
            "<span class='pt-place' style='font-weight:normal;'>Place<b>{1}</b></span>";

        [Fact]
        public void ToString_WhenCalled_ReturnHtmlString()
        {
            // Arrange
            var positionTakingWin = "10";
            var positionTakingPlace = "100";
            var expectedTemplate = string.Format(PTRacingTemplate, positionTakingWin, positionTakingPlace);

            // Act
            var actualTemplate = new PTRacing_Template();
            actualTemplate.PT_Win = positionTakingWin;
            actualTemplate.PT_Place = positionTakingPlace;

            // Assert
            Assert.Equal(expectedTemplate, actualTemplate.ToString());
        }

        [Fact]
        public void ToString_BlockIsVisible_ReturnEmptyString()
        {
            // Act
            var actualTemplate = new PTRacing_Template();
            actualTemplate.Visible = false;

            // Assert
            Assert.True(string.IsNullOrEmpty(actualTemplate));
        }
    }
}