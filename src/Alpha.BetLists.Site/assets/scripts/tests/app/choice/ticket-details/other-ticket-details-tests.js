describe("Choice_TicketDetails_", function () {
    var PopupDetails = Common.PopupDetails.Instance(window.top);
    var popupDetailsShow;
    beforeEach(function () {
        jasmine.getFixtures().fixturesPath = "app/templates";
        loadFixtures("common-template.html");
        popupDetailsShow = spyOn(PopupDetails, "Show").and.callFake(function () { });
    });
    describe("GoldDeluxeTicketDetails_", function () {
        describe("ShowDetails_", function () {
            var element;
            var goldDeluxeTicketDetails;
            beforeEach(function () {
                appendSetFixtures("<div id='test'></div>");
                element = document.getElementById("test");
                goldDeluxeTicketDetails = new BetList.Choice.TicketDetails.GoldDeluxeTicketDetails();
            });
            it("When Element By Id Show Then Hide", function () {
                // Arrange
                element.style.display = "block";
                // Act
                goldDeluxeTicketDetails.ShowDetails("test");
                // Assert
                expect(element.style.display).toBe("none");
            });
            it("When Element By Id Hide Then Show", function () {
                // Arrange
                element.style.display = "none";
                // Act
                goldDeluxeTicketDetails.ShowDetails("test");
                // Assert
                expect(element.style.display).toBe("block");
            });
        });
    });
    describe("GlobalDetailsFunction_", function () {
        it("showDetails -> Assign Correct Show GoldDeluxe Details Function", function () {
            // Arrange
            var showDetailsFunc = BetList.Choice.TicketDetails.GoldDeluxeTicketDetails.Instance.ShowDetails;
            // Assert
            expect(showDetail).toBe(showDetailsFunc);
        });
    });
});
//# sourceMappingURL=other-ticket-details-tests.js.map