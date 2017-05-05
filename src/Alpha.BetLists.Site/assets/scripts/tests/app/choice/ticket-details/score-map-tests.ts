describe("Choice_ScoreMap_", () => {
    const PopupDetails = Common.PopupDetails.Instance
    let popupDetailsShow: jasmine.Spy;

    beforeEach(() => {
        jasmine.getFixtures().fixturesPath = "app/templates";
        loadFixtures("common-template.html");
        popupDetailsShow = spyOn(PopupDetails, "Show").and.callFake(() => { });
    });

    [1, 3].forEach((liveIndicator) => {
        it("ShowScoreMap -> Show Popup Details", () => {
            // Arrange
            const matchIds = "1";
            const betType = "2";
            const params = liveIndicator == 1 ?
                "leagueId=-1&matchIds=" + matchIds + "&bettypes_live=" + betType + "&bettypes_dead=-1" :
                "leagueId=-1&matchIds=" + matchIds + "&bettypes_live=-1&bettypes_dead=" + betType;
            const expectedUrl = "old-path/ForecastScoreMap/Index?" + params;

            // Act
            BetList.Choice.ScoreMap.Instance.ShowScoreMap(matchIds, betType, liveIndicator);

            // Assert
            expect(popupDetailsShow).toHaveBeenCalledWith(expectedUrl);
        });
    })

    describe("GlobalDetailsFunction_", () => {
        it("OpenScoreMap -> Assign Correct Show Details Function", () => {
            // Arrange
            const showDetailsFunc = BetList.Choice.ScoreMap.Instance.ShowScoreMap;

            // Assert
            expect(OpenScoreMap).toBe(showDetailsFunc);
        })
    })
});