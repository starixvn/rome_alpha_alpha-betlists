describe("Status_ResultDetails_", () => {
    const matchId: number = 1;
    const race: number = 1;
    const betTypeId: number = 2;
    const sportTypeId: number = 3;
    const refNo: string = "324244";
    const userName: string = "harrison";
    const winLostDate: Date = new Date();
    const refNoMixParlay: number = 1;
    const league: number = 3;
    const isOutright: boolean = true;
    const betId: number = 1;
    const PopupDetails = Common.PopupDetails.Instance
    let popupDetailsShow: jasmine.Spy;

    beforeEach(() => {
        jasmine.getFixtures().fixturesPath = "app/templates";
        loadFixtures("common-template.html");
        popupDetailsShow = spyOn(PopupDetails, "Show").and.callFake(() => { });
    });

    describe("Sportsbook_", () => {
        it("ViewResultDetails -> Show Popup With Expected Url", () => {
            // Arrange
            const params = '?matchid=' + matchId
                + '&bettypeid=' + betTypeId
                + '&race=' + race
                + '&sportId=' + sportTypeId
                + '&leagueId=' + league
                + '&isOutright=' + isOutright
                + '&refnum=' + refNo
                + '&betid=' + betId
                + '&isViewResultFromBetList=true';

            // Act
            new BetList.Status.ResultDetails.SportsbookResultDetails().ViewResultDetails(matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId);

            // Assert
            expect(popupDetailsShow).toHaveBeenCalledWith("result-path/Normal/ResultDetails" + params);
        });
    });

    describe("MixParlay_", () => {
        it("ViewResultDetails -> Show Popup With Expected Url", () => {
            // Arrange
            const params = '?refnum=' + refNoMixParlay
                + '&wldate=' + winLostDate
                + '&bettypeid=' + betTypeId
                + '&matchid=1&isViewResultFromBetList=true';

            // Act
            new BetList.Status.ResultDetails.MixParlayResultDetails().ViewResultDetails(matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId);

            // Assert
            expect(popupDetailsShow).toHaveBeenCalledWith("result-path/Normal/ResultDetails" + params);
        });
    });
});