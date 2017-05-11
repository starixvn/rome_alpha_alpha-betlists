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

    describe("NG2_", () => {
        it("ViewResultDetails -> Redirect Expected Url", () => {
            // Arrange
            const params = '?gambleId=' + matchId;

            // Act
            new BetList.Status.ResultDetails.NG2ResultDetails().ViewResultDetails(matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId);

            // Assert
            expect(popupDetailsShow).toHaveBeenCalledWith("result-path/ng2/TicketDetail" + params);
        });
    });
});