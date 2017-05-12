describe("Choice_TicketDetails_", () => {
    const PopupDetails = Common.PopupDetails.Instance
    let popupDetailsShow: jasmine.Spy;

    beforeEach(() => {
        jasmine.getFixtures().fixturesPath = "app/templates";
        loadFixtures("common-template.html");
        popupDetailsShow = spyOn(PopupDetails, "Show").and.callFake(() => { });
    });

    describe("Casino_", () => {
        describe("ShowDetails()", () => {
            ["sport", null].forEach((sportName) => {
                it("Should Call Show Popup Details With Expected Url", () => {
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
    });

    describe("Allbet_", () => {
        describe("ShowRunningBetsDetails()", () => {
            it("Should Call Show Popup Details With Expected Url", () => {
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
        });
    });

    describe("GlobalDetailsFunction_", () => {
        describe("showRnGCasinoDetails", () => {
            it("Should Be Assigned Show Details RnG Casino Function", () => {
                // Arrange
                const showDetailsFunc = BetList.Choice.TicketDetails.CasinoTicketDetails.CreateInstance("RNG Casino").ShowDetails;

                // Assert
                expect(showRnGCasinoDetails).toBe(showDetailsFunc);
            });
        });

        describe("showVoidBridgeDetails", () => {
            it("Should Be Assigned Show Details Macau Games Function", () => {
                // Arrange
                const showDetailsFunc = BetList.Choice.TicketDetails.CasinoTicketDetails.CreateInstance("Macau Games").ShowDetails;

                // Assert
                expect(showVoidBridgeDetails).toBe(showDetailsFunc);
            });
        });

        describe("showGlobalGaming", () => {
            it("Should Be Assigned Show Details Fishing World Function", () => {
                // Arrange
                const showDetailsFunc = BetList.Choice.TicketDetails.CasinoTicketDetails.CreateInstance("Fishing World").ShowDetails;

                // Assert
                expect(showGlobalGaming).toBe(showDetailsFunc);
            });
        });

        describe("showTodayRunningBetsDetails", () => {
            it("Should Be Assigned Show Details AllBet Function", () => {
                // Arrange
                const showDetailsFunc = BetList.Choice.TicketDetails.AllbetTicketDetails.Instance.ShowRunningBetsDetails;

                // Assert
                expect(showTodayRunningBetsDetails).toBe(showDetailsFunc);
            });
        });
    })
});