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
    public class PTComm_TemplateTests
    {
        private const string PTCommTemplate
            = "<div class=\"position-taking bl_underdog\">" +
            "<font class=\"pt-value\">{0}</font><br />" +
            "<span class=\"pt-winloss\">{1}</span><br />" +
            "</div>" +
            "{2}<br />" +
            "{3}<br />";

        [Fact]
        public void ToString_WhenCalled_ReturnHtmlString()
        {
            // Arrange
            var positionTakingPercent = "10";
            var positionTakingValue = "100";
            var commPercent = "11";
            var comValue = "110";
            var expectedTemplate = string.Format(PTCommTemplate, positionTakingPercent, positionTakingValue, commPercent, comValue);

            // Act
            var actualTemplate = new PTComm_Template();
            actualTemplate.PT.PT_Percent = positionTakingPercent;
            actualTemplate.PTValue.PT_Value = positionTakingValue;
            actualTemplate.Comm.Comm_Percent = commPercent;
            actualTemplate.CommValue.Comm_value = comValue;

            // Assert
            Assert.Equal(expectedTemplate, actualTemplate.ToString());
        }

        [Fact]
        public void ToString_BlockIsVisible_ReturnEmptyString()
        {
            // Act
            var actualTemplate = new PTComm_Template();
            actualTemplate.Visible = false;

            // Assert
            Assert.True(string.IsNullOrEmpty(actualTemplate));
        }
    }
}