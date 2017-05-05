describe("Choice_TicketDetails_", () => {
    const PopupDetails = Common.PopupDetails.Instance
    let popupDetailsShow: jasmine.Spy;

    beforeEach(() => {
        jasmine.getFixtures().fixturesPath = "app/templates";
        loadFixtures("common-template.html");
        popupDetailsShow = spyOn(PopupDetails, "Show").and.callFake(() => { });
    });

    describe("Casino_", () => {
        ["sport",
            null].forEach((sportName) => {
                it("ShowDetails -> Call Show Popup Details", () => {
                    // Arrange
                    const defaultSportName = "RNG Casino";
                    const refNo = "111";
                    const winLossDate: Date = new Date();
                    const custId = "1";
                    const transType = "winlost";
                    const params = "?refNo=" + refNo
                        + "&transType=" + transType
                        + "&custId=" + custId
                        + "&sportName=" + (sportName ? sportName : defaultSportName);

                    // Act
                    BetList.Choice.TicketDetails.CasinoTicketDetails.CreateInstance(defaultSportName).ShowDetails(refNo, winLossDate, custId, transType, sportName);

                    // Assert
                    expect(popupDetailsShow).toHaveBeenCalledWith("old-path/LiveCasinoBetList/BetDetail/RNGBetDetail" + params);
                });
            });
    });

    describe("Allbet_", () => {
        it("ShowRunningBetsDetails -> Call Show Popup Details", () => {
            // Arrange
            const refNo = "111";
            const winLossDate: Date = new Date();
            const custId = "1";
            const transType = "winlost";
            const params = 'refno=' + refNo + '&custid=' + custId + '&transType=' + transType;

            // Act
            BetList.Choice.TicketDetails.AllbetTicketDetails.Instance.ShowRunningBetsDetails(refNo, winLossDate, custId, transType);

            // Assert
            expect(popupDetailsShow).toHaveBeenCalledWith(
                'old-path/LiveCasinoBetList/BetDetail/Index?' + params,
                'Allbet - Detailed Bet list - ' + winLossDate.toString().split(' ')[0])
        });
    })

    describe("GlobalDetailsFunction_", () => {
        it("showRnGCasinoDetails -> Assign Correct Show Details Function", () => {
            // Arrange
            const showDetailsFunc = BetList.Choice.TicketDetails.CasinoTicketDetails.CreateInstance("RNG Casino").ShowDetails;

            // Assert
            expect(showRnGCasinoDetails).toBe(showDetailsFunc);
        })

        it("showVoidBridgeDetails -> Assign Correct Show Details Function", () => {
            // Arrange
            const showDetailsFunc = BetList.Choice.TicketDetails.CasinoTicketDetails.CreateInstance("Macau Games").ShowDetails;

            // Assert
            expect(showVoidBridgeDetails).toBe(showDetailsFunc);
        })

        it("showGlobalGaming -> Assign Correct Show Details Function", () => {
            // Arrange
            const showDetailsFunc = BetList.Choice.TicketDetails.CasinoTicketDetails.CreateInstance("Fishing World").ShowDetails;

            // Assert
            expect(showGlobalGaming).toBe(showDetailsFunc);
        })

        it("showTodayRunningBetsDetails -> Assign Correct Show Details Function", () => {
            // Arrange
            const showDetailsFunc = BetList.Choice.TicketDetails.AllbetTicketDetails.Instance.ShowRunningBetsDetails;

            // Assert
            expect(showTodayRunningBetsDetails).toBe(showDetailsFunc);
        })
    })
});