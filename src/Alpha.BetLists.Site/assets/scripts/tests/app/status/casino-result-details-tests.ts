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
            var params = '?gameId=' + refNo
                + '&roleId=456'
                + '&productId=6';

            // Act
            new BetList.Status.ResultDetails.RnGCasinoResultDetails().ViewResultDetails(matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId);

            // Assert
            expect(popupDetailsShow).toHaveBeenCalledWith("result-path/ThirdParty/Index" + params, "", 835, 675);
        });
    });

    describe("MacauGames_", () => {
        it("ViewResultDetails -> Show Popup With Expected Url", () => {
            // Arrange
            var params = '?custId=1' +
                '&roleId=456' +
                '&gameId=' + refNo +
                '&productId=22';

            // Act
            new BetList.Status.ResultDetails.MacauGamesResultDetails().ViewResultDetails(matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId);

            // Assert
            expect(popupDetailsShow).toHaveBeenCalledWith("result-path/ThirdParty/Index" + params, "", 835, 675);
        });
    });

    describe("FishingWorld_", () => {
        it("ViewResultDetails -> Show Popup With Expected Url", () => {
            // Arrange
            var params = '?custId=1' +
                '&roleId=456' +
                '&gameId=' + betCheck +
                '&productId=24';

            // Act
            new BetList.Status.ResultDetails.FishingWorldResultDetails().ViewResultDetails(matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId, betCheck);

            // Assert
            expect(popupDetailsShow).toHaveBeenCalledWith("result-path/ThirdParty/Index" + params, "", 835, 675);
        });
    });

    describe("PP_", () => {
        it("ViewResultDetails -> Show Popup With Expected Url", () => {
            // Arrange
            var params = '?custId=1' +
                '&roleId=456' +
                '&gameId=' + betCheck +
                '&productId=32';

            // Act
            new BetList.Status.ResultDetails.PPResultDetails().ViewResultDetails(matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId, betCheck);

            // Assert
            expect(popupDetailsShow).toHaveBeenCalledWith("result-path/ThirdParty/Index" + params, "", 835, 675);
        });
    });
});
