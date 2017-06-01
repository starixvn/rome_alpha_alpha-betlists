describe("Choice_TicketDetails_", () => {
    const PopupDetails = Common.PopupDetails.Instance
    let popupDetailsShow: jasmine.Spy;

    beforeEach(() => {
        jasmine.getFixtures().fixturesPath = "app/templates";
        loadFixtures("common-template.html");
        popupDetailsShow = spyOn(PopupDetails, "Show").and.callFake(() => { });
    });

    describe("Casino_", () => {
        const defaultSportName = "RNG Casino";
        const refNo = "111";
        const winLossDate: Date = new Date();
        const custId = "1";
        const transType = "winlost";

        const createDetailPopupParam = (sportName: string) => {
            return "?refNo=" + refNo
                + "&transType=" + transType
                + "&custId=" + custId
                + "&sportName=" + sportName;
        };

        describe("ShowRnGCasinoDetails()", () => {
            it("Should Call Show Popup Details With Expected Url", () => {
                // Arrange
                const params = createDetailPopupParam("RNG Casino");

                // Act
                BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowRnGCasinoDetails(refNo, winLossDate, custId, transType);

                // Assert
                expect(popupDetailsShow).toHaveBeenCalledWith("old-path/LiveCasinoBetList/BetDetail/RNGBetDetail" + params);
            });
        });

        describe("ShowMacauGamesDetails()", () => {
            it("Should Call Show Popup Details With Expected Url", () => {
                // Arrange
                const params = createDetailPopupParam("Macau Games");

                // Act
                BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowMacauGamesDetails(refNo, winLossDate, custId, transType);

                // Assert
                expect(popupDetailsShow).toHaveBeenCalledWith("old-path/LiveCasinoBetList/BetDetail/RNGBetDetail" + params);
            });
        });

        describe("ShowFishingWorldDetails()", () => {
            it("Should Call Show Popup Details With Expected Url", () => {
                // Arrange
                const params = createDetailPopupParam("Fishing World");

                // Act
                BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowFishingWorldDetails(refNo, winLossDate, custId, transType);

                // Assert
                expect(popupDetailsShow).toHaveBeenCalledWith("old-path/LiveCasinoBetList/BetDetail/RNGBetDetail" + params);
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
                const showDetailsFunc = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowRnGCasinoDetails;

                // Assert
                expect(showRnGCasinoDetails).toBe(showDetailsFunc);
            });
        });

        describe("showVoidBridgeDetails", () => {
            it("Should Be Assigned Show Details Macau Games Function", () => {
                // Arrange
                const showDetailsFunc = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowMacauGamesDetails;

                // Assert
                expect(showVoidBridgeDetails).toBe(showDetailsFunc);
            });
        });

        describe("showGlobalGaming", () => {
            it("Should Be Assigned Show Details Fishing World Function", () => {
                // Arrange
                const showDetailsFunc = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowFishingWorldDetails;

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