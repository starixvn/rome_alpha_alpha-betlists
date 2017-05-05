describe("Choice_TicketDetails_", () => {
    const PopupDetails = Common.PopupDetails.Instance
    let popupDetailsShow: jasmine.Spy;

    beforeEach(() => {
        jasmine.getFixtures().fixturesPath = "app/templates";
        loadFixtures("common-template.html");
        popupDetailsShow = spyOn(PopupDetails, "Show").and.callFake(() => { });
    });

    describe("NG2TicketDetails_", () => {
        it("ShowNG2BetDetails -> Show Popup Details", () => {
            // Arrange
            const refNo = "1";
            const expectedUrl = "result-path/ng2/ParlayDetail?refNo=" + refNo;

            // Act
            BetList.Choice.TicketDetails.NG2TicketDetails.Instance.ShowNG2BetDetails("1", refNo, "1");

            // Assert
            expect(popupDetailsShow).toHaveBeenCalledWith(expectedUrl);
        });
    });

    describe("GlobalDetailsFunction_", () => {
        it("showNG2Details -> Assign Correct Show Details Function", () => {
            // Arrange
            const showDetailsFunc = BetList.Choice.TicketDetails.NG2TicketDetails.Instance.ShowNG2BetDetails;

            // Assert
            expect(showNG2Details).toBe(showDetailsFunc);
        })
    })
});