namespace Alpha.BetLists.Tests.Criteria.Lucky3
{
    using Alpha.BetLists.Criteria.Lucky3;
    using Fanex.BetDetails.Lucky3.Criteria;
    using NSubstitute;
    using Xunit;

    public class GetLucky3DetailCriteriaTests
    {
        private readonly IGetLucky3DetailParameter parameter;
        private readonly GetLucky3DetailCriteria criteria;

        public GetLucky3DetailCriteriaTests()
        {
            parameter = Substitute.For<IGetLucky3DetailParameter>();
            criteria = new GetLucky3DetailCriteria(parameter);
        }

        [Fact]
        public void CustId_Always_ReturnCustomerIdOfParameter()
        {
            //// Arrange
            var expectedId = 123456;
            parameter.CustomerId.Returns(expectedId);

            //// Act
            var act = criteria.CustId;

            //// Assert
            Assert.Equal(expectedId, act);
        }

        [Fact]
        public void TransactionId_Always_ReturnTransactionIdOfParameter()
        {
            //// Arrange
            var expectedId = 123456;
            parameter.TransactionId.Returns(expectedId);

            //// Act
            var act = criteria.TransId;

            //// Assert
            Assert.Equal(expectedId, act);
        }

        [Fact]
        public void GetSettingKey_Alway_ReturnCorrectKey()
        {
            //// Arrange            
            var expectedKey = "Acc_BetList_Lucky3_Detail";

            //// Act
            var act = criteria.GetSettingKey();

            //// Assert
            Assert.Equal(expectedKey, act);
        }

        [Fact]
        public void IsValid_Always_CallIsValidOfParameter()
        {
            //// Arrange

            //// Act
            var act = criteria.IsValid();

            //// Assert
            parameter.Received().IsValid();
        }
    }
}
