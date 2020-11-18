namespace Alpha.BetLists.Site.Tests.Request.MemberBetList
{
    using System;
    using Alpha.BetLists.Services;
    using Alpha.BetLists.Site.Tests.Constants;
    using BetLists.Models;
    using BetLists.Tests.Constants;
    using Fixtures;
    using NSubstitute;
    using Requests.MemberBetList;
    using Utils;
    using Xunit;

    [Trait("Category", "MemberBetList")]
    [Collection("BetListCollection")]
    public class MemberBetListQueryTests
    {
        private readonly IUser user;

        public MemberBetListQueryTests(BetListFixture betlistFixture)
        {
            user = betlistFixture.User;

            SystemTime.Today = DefaultVariables.CurrentDate;

            var reportDateService = Substitute.For<ReportDateService>();
            ReportDateServiceImpl.Instance = reportDateService;
            reportDateService.GetMoveBetsDay().Returns(DefaultVariablesTest.MoveBetDate);
        }

        [Fact]
        public void MinDate_Always_ReturnsMoveBetDate()
        {
            // Arrange
            var query = new MemberBetListQuery(user);
            var expectedMinDate = DefaultVariablesTest.MoveBetDate.AddDays(1);

            // Act
            var actualMinDate = query.MinDate;

            // Assert
            Assert.Equal(expectedMinDate, actualMinDate);
        }

        [Fact]
        public void FromDate_Always_ReturnsCurrentDate()
        {
            // Arrange
            var query = new MemberBetListQuery(user);
            var expectedFromDate = DefaultVariables.CurrentDate;

            // Act
            var actualFromDate = query.FromDate;

            // Assert
            Assert.Equal(expectedFromDate, actualFromDate);
        }

        [Fact]
        public void ToDate_Always_ReturnsCurrentDate()
        {
            // Arrange
            var query = new MemberBetListQuery(user);
            var expectedToDate = DefaultVariables.CurrentDate;

            // Act
            var actualToDate = query.ToDate;

            // Assert
            Assert.Equal(expectedToDate, actualToDate);
        }
    }
}