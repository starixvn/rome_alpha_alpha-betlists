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

    beforeEach(() => {
        jasmine.getFixtures().fixturesPath = "app/templates";
        loadFixtures("common-template.html");
    });

    describe("Casino_", () => {
        it("ViewResultDetails -> Show Popup With Expected Url", () => {
            // Arrange
            const params = '?typegame=casinogame&ibcrefno=' + refNo
                + '&bettype=Casino'
                + '&uname=' + userName
                + '&type=' + "winLoss"
                + '&custId=' + "1"
                + '&productIds=' + "1,2"
                + '&fdate=' + "05/10/2017"
                + '&tdate=' + "06/10/2017"
                + '&isHistoryReport=1';
            let locationReplace = spyOn(window.location, "replace");

            // Act
            new BetList.Status.ResultDetails.CasinoResultDetails().ViewResultDetails(matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId);

            // Assert
            expect(locationReplace).toHaveBeenCalledWith('old-path/RnGCasino/HandHistory' + params);
        });
    });
});
