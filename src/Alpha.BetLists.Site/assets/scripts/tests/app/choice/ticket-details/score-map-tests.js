describe("Choice_ScoreMap_", function () {
    var PopupDetails = Common.PopupDetails.Instance(window.top);
    var popupDetailsShow;
    beforeEach(function () {
        jasmine.getFixtures().fixturesPath = "app/templates";
        loadFixtures("common-template.html");
        popupDetailsShow = spyOn(PopupDetails, "Show").and.callFake(function () { });
    });
    [1, 3].forEach(function (liveIndicator) {
        it("ShowScoreMap -> Show Popup Details", function () {
            // Arrange
            var matchIds = "1";
            var betType = "2";
            var params = liveIndicator == 1 ?
                "leagueId=-1&matchIds=" + matchIds + "&bettypes_live=" + betType + "&bettypes_dead=-1" :
                "leagueId=-1&matchIds=" + matchIds + "&bettypes_live=-1&bettypes_dead=" + betType;
            var expectedUrl = "totalbets-forecast-path/ForecastScoreMap/Index?" + params;
            // Act
            BetList.Choice.ScoreMap.Instance.ShowScoreMap(matchIds, betType, liveIndicator);
            // Assert
            expect(popupDetailsShow).toHaveBeenCalledWith(expectedUrl, "", 1008);
        });
    });
    describe("GlobalDetailsFunction_", function () {
        it("OpenScoreMap -> Assign Correct Show Details Function", function () {
            // Arrange
            var showDetailsFunc = BetList.Choice.ScoreMap.Instance.ShowScoreMap;
            // Assert
            expect(OpenScoreMap).toBe(showDetailsFunc);
        });
    });
});
//# sourceMappingURL=score-map-tests.js.map