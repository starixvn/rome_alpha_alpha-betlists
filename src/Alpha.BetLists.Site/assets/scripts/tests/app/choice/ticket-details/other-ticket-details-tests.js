describe("Choice_TicketDetails_", function () {
    var PopupDetails = Common.PopupDetails.Instance(window.top);
    var popupDetailsShow;
    beforeEach(function () {
        jasmine.getFixtures().fixturesPath = "app/templates";
        loadFixtures("common-template.html");
        popupDetailsShow = spyOn(PopupDetails, "Show").and.callFake(function () { });
    });
    describe("NG2TicketDetails_", function () {
        it("ShowNG2BetDetails -> Show Popup Details", function () {
            // Arrange
            var refNo = "1";
            var expectedUrl = "result-path/ng2/ParlayDetail?refNo=" + refNo;
            // Act
            BetList.Choice.TicketDetails.NG2TicketDetails.Instance.ShowNG2BetDetails("1", refNo, "1");
            // Assert
            expect(popupDetailsShow).toHaveBeenCalledWith(expectedUrl, "", 600, 350);
        });
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
        it("showNG2Details -> Assign Correct Show Details Function", function () {
            // Arrange
            var showDetailsFunc = BetList.Choice.TicketDetails.NG2TicketDetails.Instance.ShowNG2BetDetails;
            // Assert
            expect(showNG2Details).toBe(showDetailsFunc);
        });
        it("showDetails -> Assign Correct Show GoldDeluxe Details Function", function () {
            // Arrange
            var showDetailsFunc = BetList.Choice.TicketDetails.GoldDeluxeTicketDetails.Instance.ShowDetails;
            // Assert
            expect(showDetail).toBe(showDetailsFunc);
        });
    });
});
//# sourceMappingURL=other-ticket-details-tests.js.map