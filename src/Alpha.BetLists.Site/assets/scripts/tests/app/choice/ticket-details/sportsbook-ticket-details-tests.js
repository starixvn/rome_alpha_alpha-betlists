describe("Choice_TicketDetails_", function () {
    var PopupDetails = Common.PopupDetails.Instance(window.top);
    var popupDetailsShow;
    beforeEach(function () {
        jasmine.getFixtures().fixturesPath = "app/templates";
        loadFixtures("common-template.html");
        popupDetailsShow = spyOn(PopupDetails, "Show").and.callFake(function () { });
    });
    describe("Parlay_", function () {
        var transId = "12";
        var transIndex = "34";
        beforeEach(function () {
            appendLoadFixtures("parlay-template.html");
        });
        it("ShowComboParlayDetails -> Show Popup Details", function () {
            // Arrange
            var refNo = "111";
            var winLossDate = new Date();
            var custId = "1";
            var transId = "2";
            var betId = "3";
            var betTeam = "1";
            var params = '?refno=' + refNo
                + '&winlostdate=' + winLossDate
                + '&transid=' + transId
                + '&betid=' + betId
                + '&custid=' + custId
                + '&betteam=' + betTeam;
            // Act
            BetList.Choice.TicketDetails.ParlayTicketDetails.Instance
                .ShowComboParlayDetails(refNo, winLossDate, transId, betId, custId, betTeam);
            // Assert
            expect(popupDetailsShow).toHaveBeenCalledWith('betlist-path/Parlay/ComboParlayDetail' + params, "", 800, 438);
        });
        it("ShowComboParlayNewTickets_WhenTicketsContentHtmlIsEmpty -> Call Ajax Get Content", function () {
            // Arrange
            var ticketsContent = $("#divEvent_" + transId + transIndex);
            var comboOfferName = $('#combinationOfferName_' + transId + transIndex);
            var custId = "1";
            var url = "betlist-path/Parlay/ComboParlay" + "?transid=" + transId + "&custId=1";
            ticketsContent.html('');
            spyOn($, 'ajax').and.callFake(function (options) {
                options.beforeSend();
                expect(ticketsContent).toHaveHtml("<span class=\"block-icon-small\"></span>");
                expect(ticketsContent).toBeVisible();
                expect(options.method).toBe('GET');
                expect(options.url).toBe(url);
                options.success('responseHtml');
            });
            // Act
            BetList.Choice.TicketDetails.ParlayTicketDetails.Instance.ShowComboParlayOldTickets(transId, transIndex, '', custId);
            // Assert
            expect(ticketsContent).toHaveHtml('responseHtml');
            expect(comboOfferName).not.toBeHidden();
        });
        it("ShowComboParlayNewTickets_TicketsContentHtmlIsNotEmptyAndVisible -> Not Call Ajax Get Content", function () {
            // Arrange
            var ticketsContent = $("#divEvent_" + transId + transIndex);
            var comboOfferName = $('#combinationOfferName_' + transId + transIndex);
            ticketsContent.html('12342343');
            ticketsContent.show();
            var custId = "1";
            // Act
            BetList.Choice.TicketDetails.ParlayTicketDetails.Instance.ShowComboParlayOldTickets(transId, transIndex, '', custId);
            // Assert
            expect(ticketsContent).toHaveHtml('12342343');
            expect(ticketsContent).toBeHidden();
            expect(comboOfferName).toBeVisible();
        });
        it("ShowComboParlayNewTickets_TicketsContentHtmlIsNotEmptyAndHidden -> Not Call Ajax Get Content", function () {
            // Arrange
            var ticketsContent = $("#divEvent_" + transId + transIndex);
            var comboOfferName = $('#combinationOfferName_' + transId + transIndex);
            ticketsContent.html('12342343');
            ticketsContent.hide();
            var custId = "1";
            // Act
            BetList.Choice.TicketDetails.ParlayTicketDetails.Instance.ShowComboParlayOldTickets(transId, transIndex, '', custId);
            // Assert
            expect(ticketsContent).toHaveHtml('12342343');
            expect(ticketsContent).toBeVisible();
            expect(comboOfferName).not.toBeHidden();
        });
        it("ShowMixParlayTickets_Always -> Ajax Send Correct Url", function () {
            // Arrange
            var ticketsContent = $("#divEvent_" + transId);
            var url = "betlist-path/Parlay/MixParlay" + "?transid=" + transId + "&custId=1";
            ticketsContent.html('');
            // CallBack Assert
            spyOn($, 'ajax').and.callFake(function (options) {
                expect(options.url).toBe(url);
                options.success('responseHtml');
            });
            // Act
            BetList.Choice.TicketDetails.ParlayTicketDetails.Instance.ShowMixParlayTickets(transId, "2", 1);
            // Assert
            expect(ticketsContent).toHaveHtml('responseHtml');
        });
        it("ShowComboParlayNewTickets_Always -> Ajax Send Correct Url", function () {
            // Arrange
            var refNo = "1";
            var handicap1 = "2";
            var handicap2 = "3";
            var betCheck = "4";
            var betTeam = "5";
            var custId = "1";
            var params = '?transid=' + transId
                + '&handicap1=' + handicap1
                + '&handicap2=' + handicap2
                + '&betcheck=' + betCheck
                + '&betteam=' + betTeam
                + '&custId=1';
            var ticketsContent = $("#divEvent_" + transId + transIndex);
            var url = "betlist-path/Parlay/ComboParlay" + params;
            ticketsContent.html('');
            // CallBack Assert
            spyOn($, 'ajax').and.callFake(function (options) {
                expect(options.url).toBe(url);
                options.success('responseHtml');
            });
            // Act
            BetList.Choice.TicketDetails.ParlayTicketDetails.Instance
                .ShowComboParlayNewTickets(transId, transIndex, refNo, handicap1, handicap2, betCheck, betTeam, custId);
            // Assert
            expect(ticketsContent).toHaveHtml('responseHtml');
        });
    });
    describe("GlobalDetailsFunction_", function () {
        it("showMP -> Assign Correct Show Details Function", function () {
            // Arrange
            var showDetailsFunc = BetList.Choice.TicketDetails.ParlayTicketDetails.Instance.ShowMixParlayTickets;
            // Assert
            expect(showMP).toBe(showDetailsFunc);
        });
        it("showCombMPDetail -> Assign Correct Show Details Function", function () {
            // Arrange
            var showDetailsFunc = BetList.Choice.TicketDetails.ParlayTicketDetails.Instance.ShowComboParlayDetails;
            // Assert
            expect(showCombMPDetail).toBe(showDetailsFunc);
        });
        it("showCombMP -> Assign Correct Show Details Function", function () {
            // Arrange
            var showDetailsFunc = BetList.Choice.TicketDetails.ParlayTicketDetails.Instance.ShowComboParlayOldTickets;
            // Assert
            expect(showCombMP).toBe(showDetailsFunc);
        });
        it("showNewCombMP -> Assign Correct Show Details Function", function () {
            // Arrange
            var showDetailsFunc = BetList.Choice.TicketDetails.ParlayTicketDetails.Instance.ShowComboParlayNewTickets;
            // Assert
            expect(showNewCombMP).toBe(showDetailsFunc);
        });
    });
});
//# sourceMappingURL=sportsbook-ticket-details-tests.js.map