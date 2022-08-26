describe("Choice_TicketDetails_Base_", function () {
    var BaseTicketDetails = new BetList.Choice.TicketDetails.BaseTicketDetails();
    beforeEach(function () {
        jasmine.getFixtures().fixturesPath = "app/templates";
        loadFixtures("common-template.html");
    });
    describe("ViewerId", function () {
        it("Should Get ViewerId Element Value", function () {
            // Act
            var actualValue = BaseTicketDetails.ViewerId();
            // Assert
            expect(actualValue).toBe('123');
        });
    });
    describe("ViewerLevelId", function () {
        it("Should Get ViewerLevelId Element Value", function () {
            // Act
            var actualValue = BaseTicketDetails.ViewerLevelId();
            // Assert
            expect(actualValue).toBe('456');
        });
    });
});
//# sourceMappingURL=base-ticket-details-tests.js.map