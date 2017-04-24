namespace Alpha.BetLists.Tests.DataAccesses
{
    using System.Collections.Generic;
    using BetLists.DataAccesses;
    using Criteria.SmartPunter;
    using Fixtures;
    using NSubstitute;
    using SmartPunter.Criteria;
    using SmartPunter.DataAccesses;
    using Xunit;

    [Trait("Category", "BetListBaseDataAccess")]
    [Collection("FanexDataCollection")]
    public class SmartPunterDataAccessTests
    {
        private readonly FanexDataFixture dataFixture;
        private readonly ISmartPunterDataAccess smartPunter;

        public SmartPunterDataAccessTests(FanexDataFixture dataFixture)
        {
            this.dataFixture = dataFixture;
            smartPunter = new SmartPunterDataAccess(this.dataFixture.DynamicRepository);
        }

        [Fact]
        public void CheckInternalAdmin_Always_ReturnCommandIsInternalAdmin()
        {
            // Arrange
            var custId = 1;
            dataFixture.DynamicRepository.Execute(Arg.Is<InternalAdminCheckerCommand>(c => c.CustId == custId));

            // Act
            var actual = smartPunter.CheckInternalAdmin(new InternalAdminChecker(custId));

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void CheckInternalAdmin_Always_DynamicRepositoryCallExecute()
        {
            // Arrange
            var commandData = new InternalAdminChecker(1);

            // Act
            var actual = smartPunter.CheckInternalAdmin(commandData);

            // Assert
            dataFixture.DynamicRepository.Received().Execute(Arg.Is<InternalAdminCheckerCommand>(
                c => c.CustId == 1));
        }

        [Fact]
        public void CheckPermission_Always_ReturnFunctionPermission()
        {
            // Arrange
            var custId = 1;
            var functionName = "function";
            var functionNameList = new List<string> { functionName };
            dataFixture.DynamicRepository
                .Fetch<string>(Arg.Is<NAPUserPermissionCriteria>(c => c.CustId == custId))
                .Returns(functionNameList);

            // Act
            var actual = smartPunter.CheckPermission(new NAPUserPermission(custId), functionName);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void CheckPermission_Always_DynamicRepositoryCallFetch()
        {
            // Arrange
            var custId = 1;
            var functionName = "function";
            var functionNameList = new List<string> { functionName };
            dataFixture.DynamicRepository
                .Fetch<string>(Arg.Is<NAPUserPermissionCriteria>(c => c.CustId == custId))
                .Returns(functionNameList);

            // Act
            var actual = smartPunter.CheckPermission(new NAPUserPermission(custId), functionName);

            // Assert
            dataFixture.DynamicRepository.Received().Fetch<string>(Arg.Is<NAPUserPermissionCriteria>(
                c => c.CustId == 1));
        }

        [Fact]
        public void CheckSmartPunter_Always_ReturnSMACustomer()
        {
            // Arrange
            var commandData = new SmartPunterSMAChecker(1, "harrison");

            // Act
            var actual = smartPunter.CheckSmartPunter(commandData);

            // Assert
            Assert.True(actual.IsSmartPunter == false && actual.HasDownlineSmartPunter == false);
        }

        [Fact]
        public void CheckSmartPunter_Always_DynamicRepositoryCallExecute()
        {
            // Arrange
            var commandData = new SmartPunterSMAChecker(1, "harrison");

            // Act
            var actual = smartPunter.CheckSmartPunter(commandData);

            // Assert
            dataFixture.DynamicRepository.Received().Execute(Arg.Is<SmartPunterSMACheckerCommand>(
                c => c.Username == "harrison"
                && c.CustId == 1));
        }

        [Fact]
        public void CheckSmartPunterAndWriteLog_Always_DynamicRepositoryCallExecute()
        {
            // Arrange
            var commandData = new SmartPunterSMACheckerWriteLog("harrison", 1, true, 2, "10.1.1.1");

            // Act
            smartPunter.CheckSmartPunterAndWriteLog(commandData);

            // Assert
            dataFixture.DynamicRepository.Received().Execute(Arg.Is<SmartPunterSMACheckerWriteLogCommand>(
                c => c.ListUserName == "harrison"
                && c.ActionId == 1
                && c.IsRestricted == true
                && c.UserId == 2
                && c.IP == "10.1.1.1"));
        }

        [Fact]
        public void WriteLog_Always_DynamicRepositoryCallExecute()
        {
            // Arrange
            var commandData = new SmartPunterWriteLog("harrison", 1, true, 2, "10.1.1.1");

            // Act
            smartPunter.WriteLog(commandData);

            // Assert
            dataFixture.DynamicRepository.Received().Execute(Arg.Is<SmartPunterWriteLogCommand>(
                c => c.ListUserName == "harrison"
                && c.ActionId == 1
                && c.IsRestricted == true
                && c.UserId == 2
                && c.IP == "10.1.1.1"));
        }

        [Fact]
        public void WriteLogNoData_Always_DynamicRepositoryCallExecute()
        {
            // Arrange
            var commandData = new SmartPunterWriteLogNoData(1, 1, true, 2, "10.1.1.1");

            // Act
            smartPunter.WriteLogNoData(commandData);

            // Assert
            dataFixture.DynamicRepository.Received().Execute(Arg.Is<SmartPunterWriteLogNoDataCommand>(
                c => c.CustId == 1
                && c.ActionId == 1
                && c.IsRestricted == true
                && c.UserId == 2
                && c.IP == "10.1.1.1"));
        }
    }
}