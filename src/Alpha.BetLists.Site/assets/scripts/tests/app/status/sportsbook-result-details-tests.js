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
    var transdesc = "transdesc";
    var PopupDetails = Common.PopupDetails.Instance(window.top);
    var popupDetailsShow;
    beforeEach(function () {
        jasmine.getFixtures().fixturesPath = "app/templates";
        loadFixtures("common-template.html");
        popupDetailsShow = spyOn(PopupDetails, "Show").and.callFake(function () { });
    });
    describe("Sportsbook_", function () {
        it("ViewResultDetails -> Show Popup With Expected Url", function () {
            // Arrange
            var params = '?matchid=' + matchId
                + '&bettypeid=' + betTypeId
                + '&race=' + race
                + '&sportId=' + sportTypeId
                + '&leagueId=' + league
                + '&isOutright=' + isOutright
                + '&refnum=' + refNo
                + '&betid=' + betId
                + '&transdesc=' + transdesc
                + '&isViewResultFromBetList=true';
            // Act
            new BetList.Status.ResultDetails.SportsbookResultDetails().ViewResultDetails(matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId, transdesc);
            // Assert
            expect(popupDetailsShow).toHaveBeenCalledWith("result-path/Normal/ResultDetails" + params, "", 730, 250);
        });
    });
    describe("MixParlay_", function () {
        it("ViewResultDetails -> Show Popup With Expected Url", function () {
            // Arrange
            var params = '?refnum=' + refNoMixParlay
                + '&wldate=' + winLostDate
                + '&bettypeid=' + betTypeId
                + '&matchid=1&isViewResultFromBetList=true';
            // Act
            new BetList.Status.ResultDetails.MixParlayResultDetails().ViewResultDetails(matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId);
            // Assert
            expect(popupDetailsShow).toHaveBeenCalledWith("result-path/Normal/ResultDetails" + params, "", 800, 500);
        });
    });
});
//# sourceMappingURL=sportsbook-result-details-tests.js.map