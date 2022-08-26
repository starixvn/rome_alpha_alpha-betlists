describe("Status_ResultDetails_", function () {
    var matchId = 1;
    var race = 1;
    var betTypeId = 2;
    var sportTypeId = 3;
    var refNo = "324244";
    var userName = "harrison";
    var winLostDate = new Date();
    var refNoMixParlay = 1;
    var league = 3;
    var isOutright = true;
    var betId = 1;
    var betCheck = "betCheck";
    var custId = 1;
    var transDesc = "05/10/2017";
    var PopupDetails = Common.PopupDetails.Instance(window.top);
    var popupDetailsShow;
    beforeEach(function () {
        popupDetailsShow = spyOn(PopupDetails, "Show").and.callFake(function () { });
        jasmine.getFixtures().fixturesPath = "app/templates";
        loadFixtures("common-template.html");
    });
    describe("Allbet_", function () {
        it("ViewResultDetails -> Show Popup With Expected Url", function () {
            // Arrange
            var params = '?transDesc=' + transDesc
                + '&betCheck=' + betCheck
                + '&bettypeId=' + betTypeId
                + '&sportId=' + sportTypeId;
            // Act
            new BetList.Status.ResultDetails.AllbetResultDetails().ViewResultDetails(matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId, betCheck, custId, transDesc);
            // Assert
            expect(popupDetailsShow).toHaveBeenCalledWith("result-path/Normal/ResultDetails" + params, "", 835, 500);
        });
    });
    describe("RnGCasino_", function () {
        it("ViewResultDetails -> Show Popup With Expected Url", function () {
            // Arrange
            var params = '?custId=1' +
                '&transId3rd=1' +
                '&gameId3rd=betCheck' +
                '&currencyId=1' +
                '&siteId=1' +
                '&productId=6';
            // Act
            new BetList.Status.ResultDetails.RnGCasinoResultDetails().ViewResultDetails(matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId, betCheck, 1, "1", "1");
            // Assert
            expect(popupDetailsShow).toHaveBeenCalledWith("result-path/ThirdParty/RngCasinoResult" + params, "", 835, 675);
        });
    });
    describe("MacauGames_", function () {
        it("ViewResultDetails -> Show Popup With Expected Url", function () {
            // Arrange
            var params = '?custId=1' +
                '&transId3rd=1' +
                '&gameId3rd=betCheck' +
                '&currencyId=1' +
                '&siteId=1' +
                '&productId=22';
            // Act
            new BetList.Status.ResultDetails.MacauGamesResultDetails().ViewResultDetails(matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId, betCheck, 1, "1", "1");
            // Assert
            expect(popupDetailsShow).toHaveBeenCalledWith("result-path/ThirdParty/MacauGamesResult" + params, "", 835, 675);
        });
    });
    describe("FishingWorld_", function () {
        it("ViewResultDetails -> Show Popup With Expected Url", function () {
            // Arrange
            var params = '?custId=1' +
                '&roleId=456' +
                '&gameId=' + betCheck +
                '&productId=24';
            // Act
            new BetList.Status.ResultDetails.FishingWorldResultDetails().ViewResultDetails(matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId, betCheck);
            // Assert
            expect(popupDetailsShow).toHaveBeenCalledWith("result-path/ThirdParty/Detail" + params, "", 835, 675);
        });
    });
    describe("PP_", function () {
        it("ViewResultDetails -> Show Popup With Expected Url", function () {
            // Arrange
            var params = '?custId=1' +
                '&transId3rd=1' +
                '&gameId3rd=betCheck' +
                '&currencyId=1' +
                '&siteId=1' +
                '&productId=32';
            // Act
            new BetList.Status.ResultDetails.PPResultDetails().ViewResultDetails(matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId, betCheck, 1, "1", "1");
            // Assert
            expect(popupDetailsShow).toHaveBeenCalledWith("result-path/ThirdParty/PPResult" + params, "", 835, 675);
        });
    });
});
//# sourceMappingURL=casino-result-details-tests.js.map