describe("Choice_TicketDetails_", () => {
    const PopupDetails = Common.PopupDetails.Instance(window.top);
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
            expect(popupDetailsShow).toHaveBeenCalledWith(expectedUrl, "", 600, 350);
        });
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
        it("showNG2Details -> Assign Correct Show Details Function", () => {
            // Arrange
            const showDetailsFunc = BetList.Choice.TicketDetails.NG2TicketDetails.Instance.ShowNG2BetDetails;

            // Assert
            expect(showNG2Details).toBe(showDetailsFunc);
        });

        it("showDetails -> Assign Correct Show GoldDeluxe Details Function", () => {
            // Arrange
            const showDetailsFunc = BetList.Choice.TicketDetails.GoldDeluxeTicketDetails.Instance.ShowDetails;

            // Assert
            expect(showDetail).toBe(showDetailsFunc);
        });
    })
});