describe("Choice_TicketDetails_Base_", () => {
    const BaseTicketDetails = new BetList.Choice.TicketDetails.BaseTicketDetails()
    beforeEach(() => {
        jasmine.getFixtures().fixturesPath = "app/templates";
        loadFixtures("common-template.html");
    });

    it("ViewerId -> Get expected element value", () => {
        // Act
        let actualValue = BaseTicketDetails.ViewerId();

        // Assert
        expect(actualValue).toBe('123');
    })

    it("ViewerLevelId -> Get expected element value", () => {
        // Act
        let actualValue = BaseTicketDetails.ViewerLevelId();

        // Assert
        expect(actualValue).toBe('456');
    })
})