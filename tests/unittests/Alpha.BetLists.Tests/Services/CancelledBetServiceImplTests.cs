using Alpha.BetLists.Criteria.CancelledBet;
using Alpha.BetLists.Models;
using Alpha.BetLists.Models.CancelledBet;
using Alpha.BetLists.Params;
using Alpha.BetLists.Services;
using Alpha.BetLists.Tests.Fixtures;
using Alpha.BetLists.Tests.TestData;
using Fanex.BetList.Core.Entities;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Alpha.BetLists.Tests.Services
{
    [Trait("Category", "CancelledBet")]
    [Collection("FanexDataCollection")]
    public class CancelledBetServiceImplTests
    {
        private readonly FanexDataFixture dataFixture;
        private readonly UserBetCount userBetCount1;
        private readonly IEnumerable<UserBetCount> userBetCounts;
        private readonly CancelledBetServiceImpl service;

        public CancelledBetServiceImplTests(FanexDataFixture dataFixture)
        {
            this.dataFixture = dataFixture;
            userBetCount1 = new UserBetCount(263, "razor", 2);
            userBetCounts = new List<UserBetCount> { userBetCount1 };
            service = new CancelledBetServiceImpl(dataFixture.DynamicRepository);
        }

        [Fact]
        public void GetCancelledBet_ViewerIsSuperByDefault_ReturnSuperMasterList()
        {
            //// Arrange
            var masterIdDefault = 0;
            var agentIdDefault = 0;
            var parameter = new CancelledBetParam(UserLevel.Super, 888, masterIdDefault, agentIdDefault, 0, "en");
            FakeToCallDynamicRepository();

            //// Act
            var act = service.GetCancelledBet(parameter).GetAwaiter().GetResult();

            //// Assert
            Assert.True(act.Filter.Masters.Any());
            Assert.True(act.Filter.Agents.Any());
        }

        [Fact]
        public void GetCancelledBet_ViewerIsSuperByDefault_ReturnCorrectSelectedIndexOfMasterAgentList()
        {
            //// Arrange
            var masterIdDefault = 0;
            var agentIdDefault = 0;
            var expectedMasterId = userBetCount1.CustId;
            var parameter = new CancelledBetParam(UserLevel.Super, 888, masterIdDefault, agentIdDefault, 0, "en");
            FakeToCallDynamicRepository();

            //// Act
            var act = service.GetCancelledBet(parameter).GetAwaiter().GetResult();

            //// Assert
            Assert.Equal(expectedMasterId, act.Filter.MasterId);
            Assert.Equal(agentIdDefault, act.Filter.AgentId);
        }

        [Theory]
        [InlineData(263, 263)]
        public void GetCancelledBet_ViewerIsSuperAndChangeAgentList_ReturnMasterAgentMemberList(int masterId, int agentId)
        {
            //// Arrange
            var parameter = new CancelledBetParam(UserLevel.Super, 888, masterId, agentId, 0, "en");
            FakeToCallDynamicRepository();

            //// Act
            var act = service.GetCancelledBet(parameter).GetAwaiter().GetResult();

            //// Assert
            Assert.True(act.Filter.Masters.Any());
            Assert.True(act.Filter.Agents.Any());
            Assert.True(act.Filter.Members.Any());
        }

        [Theory]
        [InlineData(263, 263)]
        public void GetCancelledBet_ViewerIsSuperAndChangeAgentList_ReturnCorrectSelectedIndex(int masterId, int agentId)
        {
            //// Arrange
            var memberIdDefault = 0;
            var parameter = new CancelledBetParam(UserLevel.Super, 888, masterId, agentId, memberIdDefault, "en");
            FakeToCallDynamicRepository();

            //// Act
            var act = service.GetCancelledBet(parameter).GetAwaiter().GetResult();

            //// Assert
            Assert.Equal(masterId, act.Filter.MasterId);
            Assert.Equal(agentId, act.Filter.AgentId);
            Assert.Equal(memberIdDefault, act.Filter.MemberId);
        }

        [Theory]
        [InlineData(263, 263, 263)]
        public void GetCancelledBet_ViewerIsSuperAndChangeMemberList_ReturnCorrectSelectedIndex(int masterId, int agentId, int memberId)
        {
            //// Arrange            
            var parameter = new CancelledBetParam(UserLevel.Super, 888, masterId, agentId, memberId, "en");
            FakeToCallDynamicRepository();

            //// Act
            var act = service.GetCancelledBet(parameter).GetAwaiter().GetResult();

            //// Assert
            Assert.Equal(masterId, act.Filter.MasterId);
            Assert.Equal(agentId, act.Filter.AgentId);
            Assert.Equal(memberId, act.Filter.MemberId);
        }

        [Theory]
        [InlineData(263, 263, 263)]
        public void GetCancelledBet_ViewerIsSuperAndChangeMemberList_ReturnMasterAgentMemberList(int masterId, int agentId, int memberId)
        {
            //// Arrange            
            var parameter = new CancelledBetParam(UserLevel.Super, 888, masterId, agentId, memberId, "en");
            FakeToCallDynamicRepository();

            //// Act
            var act = service.GetCancelledBet(parameter).GetAwaiter().GetResult();

            //// Assert
            Assert.True(act.Filter.Masters.Any());
            Assert.True(act.Filter.Agents.Any());
            Assert.True(act.Filter.Members.Any());
        }

        [Fact]
        public void GetCancelledBet_ViewerIsMasterByDefault_ReturnAgentMemberList()
        {
            //// Arrange
            var memberIdDefault = 0;
            var agentIdDefault = 0;
            var parameter = new CancelledBetParam(UserLevel.Master, 888, 0, agentIdDefault, memberIdDefault, "en");
            FakeToCallDynamicRepository();

            //// Act
            var act = service.GetCancelledBet(parameter).GetAwaiter().GetResult();

            //// Assert
            Assert.True(act.Filter.Members.Any());
            Assert.True(act.Filter.Agents.Any());
        }

        [Fact]
        public void GetCancelledBet_ViewerIsMasterByDefault_ReturnCorrectSelectedIndexOfAgentMemberList()
        {
            //// Arrange
            var memberIdDefault = 0;
            var agentIdDefault = 0;
            var expectedAgentId = userBetCount1.CustId;
            var expectedMemberId = 0;
            var parameter = new CancelledBetParam(UserLevel.Master, 888, 0, agentIdDefault, memberIdDefault, "en");
            FakeToCallDynamicRepository();

            //// Act
            var act = service.GetCancelledBet(parameter).GetAwaiter().GetResult();

            //// Assert
            Assert.Equal(expectedAgentId, act.Filter.AgentId);
            Assert.Equal(expectedMemberId, act.Filter.MemberId);
        }

        [Theory]
        [InlineData(263, 263)]
        public void GetCancelledBet_ViewerIsMasterAndChangeMemberList_ReturnCorrectSelectedIndexOfAgentMemberList(int agentId, int memberId)
        {
            //// Arrange
            var parameter = new CancelledBetParam(UserLevel.Master, 888, 0, agentId, memberId, "en");
            FakeToCallDynamicRepository();

            //// Act
            var act = service.GetCancelledBet(parameter).GetAwaiter().GetResult();

            //// Assert
            Assert.Equal(agentId, act.Filter.AgentId);
            Assert.Equal(memberId, act.Filter.MemberId);
        }

        [Theory]
        [InlineData(263, 263)]
        public void GetCancelledBet_ViewerIsMasterAndChangeMemberList_ReturnAgentMemberList(int agentId, int memberId)
        {
            //// Arrange
            var parameter = new CancelledBetParam(UserLevel.Master, 888, 0, agentId, memberId, "en");
            FakeToCallDynamicRepository();

            //// Act
            var act = service.GetCancelledBet(parameter).GetAwaiter().GetResult();

            //// Assert
            Assert.True(act.Filter.Agents.Any());
            Assert.True(act.Filter.Members.Any());
        }

        [Fact]
        public void GetCancelledBet_ViewerIsAgentByDefault_ReturnMemberList()
        {
            //// Arrange
            var memberIdDefault = 0;
            var parameter = new CancelledBetParam(UserLevel.Agent, 888, 0, 0, memberIdDefault, "en");
            FakeToCallDynamicRepository();

            //// Act
            var act = service.GetCancelledBet(parameter).GetAwaiter().GetResult();

            //// Assert
            Assert.True(act.Filter.Members.Any());
        }

        [Theory]
        [InlineData(263)]
        public void GetCancelledBet_ViewerIsAgentAndChangeMember_ReturnMemberList(int memberId)
        {
            //// Arrange
            var parameter = new CancelledBetParam(UserLevel.Agent, 888, 0, 0, memberId, "en");
            FakeToCallDynamicRepository();

            //// Act
            var act = service.GetCancelledBet(parameter).GetAwaiter().GetResult();

            //// Assert
            Assert.True(act.Filter.Members.Any());
        }

        [Theory]
        [InlineData(263)]
        public void GetCancelledBet_ViewerIsAgentAndChangeMember_SelectedMemberIdNotChange(int memberId)
        {
            //// Arrange
            var parameter = new CancelledBetParam(UserLevel.Agent, 888, 0, 0, memberId, "en");
            FakeToCallDynamicRepository();

            //// Act
            var act = service.GetCancelledBet(parameter).GetAwaiter().GetResult();

            //// Assert
            Assert.Equal(memberId, act.Filter.MemberId);
        }

        private void FakeToCallDynamicRepository()
        {
            dataFixture.DynamicRepository
                .FetchAsync<UserBetCount>(Arg.Any<GetUsersHasCancelledBetBySuperCriteria>())
                .Returns(userBetCounts);
            dataFixture.DynamicRepository
                .FetchAsync<UserBetCount>(Arg.Any<GetUsersHasCancelledBetByMasterCriteria>())
                .Returns(userBetCounts);
            dataFixture.DynamicRepository
                .FetchAsync<UserBetCount>(Arg.Any<GetUsersHasCancelledBetByAgentCriteria>())
                .Returns(userBetCounts);
            dataFixture.DynamicRepository
                .FetchMultipleAsync<TicketDto, TicketData>(Arg.Any<GetCancelledBetCriteria>())
                .Returns(BetListDataTest.BetListDataTuple);
        }
    }
}
