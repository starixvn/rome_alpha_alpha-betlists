describe("Choice_TicketDetails_Base_", () => {
    const BaseTicketDetails = new BetList.Choice.TicketDetails.BaseTicketDetails();

    beforeEach(() => {
        jasmine.getFixtures().fixturesPath = "app/templates";
        loadFixtures("common-template.html");
    });

    describe("ViewerId", () => {
        it("Should Get ViewerId Element Value", () => {
            // Act
            const actualValue = BaseTicketDetails.ViewerId();

            // Assert
            expect(actualValue).toBe('123');
        });
    });

    describe("ViewerLevelId", () => {
        it("Should Get ViewerLevelId Element Value", () => {
            // Act
            let actualValue = BaseTicketDetails.ViewerLevelId();

            // Assert
            expect(actualValue).toBe('456');
        })
    })
})