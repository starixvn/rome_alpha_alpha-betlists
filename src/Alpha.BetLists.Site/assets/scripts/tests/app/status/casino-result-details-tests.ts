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
    const betCheck: string = "betCheck";
    const custId: number = 1;
    const transDesc: string = "05/10/2017";
    const PopupDetails = Common.PopupDetails.Instance(window.top);
    let popupDetailsShow: jasmine.Spy;

    beforeEach(() => {
        popupDetailsShow = spyOn(PopupDetails, "Show").and.callFake(() => { });
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

    describe("Allbet_", () => {
        it("ViewResultDetails -> Show Popup With Expected Url", () => {
            // Arrange
            const params = '?transDesc=' + transDesc
                + '&betCheck=' + betCheck
                + '&bettypeId=' + betTypeId
                + '&sportId=' + sportTypeId;

            // Act
            new BetList.Status.ResultDetails.AllbetResultDetails().ViewResultDetails(matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId, betCheck, custId, transDesc);

            // Assert
            expect(popupDetailsShow).toHaveBeenCalledWith("result-path/Normal/ResultDetails" + params, "", 835, 800);
        });
    });

    describe("RnGCasino_", () => {
        it("ViewResultDetails -> Show Popup With Expected Url", () => {
            // Arrange
            var params = '?refno=' + refNo
                + '&roleId=456';

            // Act
            new BetList.Status.ResultDetails.RnGCasinoResultDetails().ViewResultDetails(matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId);

            // Assert
            expect(popupDetailsShow).toHaveBeenCalledWith("result-path/HandHistory/RNGCasino" + params, "", 835, 675);
        });
    });

    describe("MacauGames_", () => {
        it("ViewResultDetails -> Show Popup With Expected Url", () => {
            // Arrange
            var params = '?custId=1' +
                '&roleId=456' +
                '&refNo=' + refNo;

            // Act
            new BetList.Status.ResultDetails.MacauGamesResultDetails().ViewResultDetails(matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId);

            // Assert
            expect(popupDetailsShow).toHaveBeenCalledWith("result-path/HandHistory/MacauGames" + params, "", 835, 675);
        });
    });

    describe("FishingWorld_", () => {
        it("ViewResultDetails -> Show Popup With Expected Url", () => {
            // Arrange
            var params = '?custId=1' +
                '&roleId=456' +
                '&refNo=' + refNo;

            // Act
            new BetList.Status.ResultDetails.FishingWorldResultDetails().ViewResultDetails(matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId);

            // Assert
            expect(popupDetailsShow).toHaveBeenCalledWith("result-path/HandHistory/FishingWorld" + params, "", 835, 675);
        });
    });
});
