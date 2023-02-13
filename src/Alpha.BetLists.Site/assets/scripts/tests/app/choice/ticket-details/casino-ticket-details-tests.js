describe("Choice_TicketDetails_", function () {
    var PopupDetails = Common.PopupDetails.Instance(window.top);
    var popupDetailsShow;
    beforeEach(function () {
        jasmine.getFixtures().fixturesPath = "app/templates";
        loadFixtures("common-template.html");
        popupDetailsShow = spyOn(PopupDetails, "Show").and.callFake(function () { });
    });
    describe("Casino_", function () {
        var defaultSportName = "Saba Casino";
        var refNo = "111";
        var winLossDate = new Date();
        var custId = "1";
        var transType = "winlost";
        var createDetailPopupParam = function (sportName) {
            return "?refNo=" + refNo
                + "&transType=" + transType
                + "&custId=" + custId
                + "&sportName=" + sportName
                + "&productId=6";
        };
        describe("ShowRnGCasinoDetails()", function () {
            it("Should Call Show Popup Details With Expected Url", function () {
                // Arrange
                var params = createDetailPopupParam("Saba Casino");
                // Act
                BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowRnGCasinoDetails(refNo, winLossDate, custId, transType);
                // Assert
                expect(popupDetailsShow).toHaveBeenCalledWith("betlist-path/CasinoDetails/Index" + params, "", 1120);
            });
        });
        describe("ShowMacauGamesDetails()", function () {
            it("Should Call Show Popup Details With Expected Url", function () {
                // Arrange
                var params = createDetailPopupParam("Macau Games");
                // Act
                BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowMacauGamesDetails(refNo, winLossDate, custId, transType);
                // Assert
                expect(popupDetailsShow).toHaveBeenCalledWith("betlist-path/CasinoDetails/Index" + params, "", 1120);
            });
        });
        describe("ShowFishingWorldDetails()", function () {
            it("Should Call Show Popup Details With Expected Url", function () {
                // Arrange
                var params = createDetailPopupParam("Fishing World");
                // Act
                BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowFishingWorldDetails(refNo, winLossDate, custId, transType);
                // Assert
                expect(popupDetailsShow).toHaveBeenCalledWith("betlist-path/CasinoDetails/Index" + params, "", 1120);
            });
        });
        describe("ShowPPDetails()", function () {
            it("Should Call Show Popup Details With Expected Url", function () {
                // Arrange
                var params = createDetailPopupParam("PP");
                // Act
                BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowPPDetails(refNo, winLossDate, custId, transType);
                // Assert
                expect(popupDetailsShow).toHaveBeenCalledWith("betlist-path/CasinoDetails/Index" + params, "", 1120);
            });
        });
    });
    describe("Allbet_", function () {
        describe("ShowRunningBetsDetails()", function () {
            it("Should Call Show Popup Details With Expected Url", function () {
                // Arrange
                var refNo = "111";
                var winLossDate = new Date();
                var custId = "1";
                var transType = "winlost";
                var params = '?refno=' + refNo + '&custid=' + custId + '&transType=' + transType + '&sportName=Allbet&productId=21';
                // Act
                BetList.Choice.TicketDetails.AllbetTicketDetails.Instance.ShowRunningBetsDetails(refNo, winLossDate, custId, transType);
                // Assert
                expect(popupDetailsShow).toHaveBeenCalledWith('betlist-path/CasinoDetails/Index' + params, 'Allbet - Detailed Bet list - ' + winLossDate.toString().split(' ')[0], 1047, 800);
            });
        });
    });
    describe("GlobalDetailsFunction_", function () {
        describe("showRnGCasinoDetails", function () {
            it("Should Be Assigned Show Details Saba Casino Function", function () {
                // Arrange
                var showDetailsFunc = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowRnGCasinoDetails;
                // Assert
                expect(showRnGCasinoDetails).toBe(showDetailsFunc);
            });
        });
        describe("showVoidBridgeDetails", function () {
            it("Should Be Assigned Show Details Macau Games Function", function () {
                // Arrange
                var showDetailsFunc = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowMacauGamesDetails;
                // Assert
                expect(showVoidBridgeDetails).toBe(showDetailsFunc);
            });
        });
        describe("showGlobalGaming", function () {
            it("Should Be Assigned Show Details Fishing World Function", function () {
                // Arrange
                var showDetailsFunc = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowFishingWorldDetails;
                // Assert
                expect(showGlobalGaming).toBe(showDetailsFunc);
            });
        });
        describe("showPPDetails", function () {
            it("Should Be Assigned Show Details PP Function", function () {
                // Arrange
                var showDetailsFunc = BetList.Choice.TicketDetails.CasinoTicketDetails.Instance.ShowPPDetails;
                // Assert
                expect(showPPDetails).toBe(showDetailsFunc);
            });
        });
        describe("showTodayRunningBetsDetails", function () {
            it("Should Be Assigned Show Details AllBet Function", function () {
                // Arrange
                var showDetailsFunc = BetList.Choice.TicketDetails.AllbetTicketDetails.Instance.ShowRunningBetsDetails;
                // Assert
                expect(showTodayRunningBetsDetails).toBe(showDetailsFunc);
            });
        });
    });
});
//# sourceMappingURL=casino-ticket-details-tests.js.map