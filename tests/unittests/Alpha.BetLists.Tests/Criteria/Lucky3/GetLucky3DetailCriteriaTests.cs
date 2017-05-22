using Alpha.BetLists.Criteria.Lucky3;
using Fanex.BetListDetail.Lucky3.Criteria;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Alpha.BetLists.Tests.Criteria.Lucky3
{
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
