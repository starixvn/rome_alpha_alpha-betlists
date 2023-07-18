describe("Choice_TicketDetails_", () => {
    const PopupDetails = Common.PopupDetails.Instance(window.top);
    let popupDetailsShow: jasmine.Spy;

    beforeEach(() => {
        jasmine.getFixtures().fixturesPath = "app/templates";
        loadFixtures("common-template.html");
        popupDetailsShow = spyOn(PopupDetails, "Show").and.callFake(() => { });
    });

    describe("GoldDeluxeTicketDetails_", () => {
        describe("ShowDetails_", () => {
            let element: HTMLElement;
            let goldDeluxeTicketDetails: BetList.Choice.TicketDetails.GoldDeluxeTicketDetails;

            beforeEach(() => {
                appendSetFixtures("<div id='test'></div>");
                element = document.getElementById("test");
                goldDeluxeTicketDetails = new BetList.Choice.TicketDetails.GoldDeluxeTicketDetails();
            });

            it("When Element By Id Show Then Hide", () => {
                // Arrange
                element.style.display = "block";

                // Act
                goldDeluxeTicketDetails.ShowDetails("test");

                // Assert
                expect(element.style.display).toBe("none");
            });

            it("When Element By Id Hide Then Show", () => {
                // Arrange
                element.style.display = "none";

                // Act
                goldDeluxeTicketDetails.ShowDetails("test");

                // Assert
                expect(element.style.display).toBe("block");
            });
        });
    });

    describe("GlobalDetailsFunction_", () => {
        it("showDetails -> Assign Correct Show GoldDeluxe Details Function", () => {
            // Arrange
            const showDetailsFunc = BetList.Choice.TicketDetails.GoldDeluxeTicketDetails.Instance.ShowDetails;

            // Assert
            expect(showDetail).toBe(showDetailsFunc);
        });
    })
});