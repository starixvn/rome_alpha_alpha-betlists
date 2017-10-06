namespace Alpha.BetLists.Site.Tests.Helpers
{
    using System;
    using System.Web.Mvc;
    using Requests.MemberBetList;
    using Site.Helpers;
    using Site.Models;
    using Xunit;

    public class QueryEntityModelBinderTests : QueryEntityModelBinder
    {
        protected override object CreateInstanceOfAssignableReportBaseQuery(Type modelType)
        {
            return "CreateInstanceOfAssignableReportBaseQuery";
        }

        protected override object CallBaseCreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            return "CallBaseCreateModel";
        }

        [Fact]
        public void CreateModel_ConstructorParamIsAssignableFromReportBaseQuery_CallCreateInstanceOfAssignableReportBaseQuery()
        {
            // Arrange
            var memberBetListQuery = typeof(MemberBetListQuery);

            // Act
            var createdInstance = CreateModel(null, null, memberBetListQuery);

            // Assert
            Assert.Equal("CreateInstanceOfAssignableReportBaseQuery", createdInstance);
        }

        [Fact]
        public void CreateModel_ConstructorParamIsNotAssignableFromReportBaseQuery_CallBaseCreateModel()
        {
            // Arrange
            var userType = typeof(User);

            // Act
            var createdInstance = CreateModel(null, null, userType);

            // Assert
            Assert.Equal("CallBaseCreateModel", createdInstance);
        }
    }
}
