describe("Choice_TicketDetails_", () => {
    const PopupDetails = Common.PopupDetails.Instance(window.top);
    let popupDetailsShow: jasmine.Spy;

    beforeEach(() => {
        jasmine.getFixtures().fixturesPath = "app/templates";
        loadFixtures("common-template.html");
        popupDetailsShow = spyOn(PopupDetails, "Show").and.callFake(() => { });
    });

    describe("Sportsbook_", () => {
        it("ShowColossusBetsDetails -> Show Popup Details", () => {
            // Arrange
            const refNo = "111";
            const winLossDate: Date = new Date();
            const custId = "1";
            const transId = "2";
            const eventId = "3";
            const params = "Refno=" + refNo
                + "&WinLossDate=" + winLossDate
                + "&TransId=" + transId
                + "&CustId=" + custId
                + "&EventId=" + eventId
                + "&RoleId=" + 456
                + "&AccId=" + 123;

            // Act
            BetList.Choice.TicketDetails.SportsbookTicketDetails.Instance
                .ShowColossusBetsDetails(refNo, winLossDate, transId, custId, eventId);

            // Assert
            expect(popupDetailsShow).toHaveBeenCalledWith('old-path/ColossusBets/BetDetails?' + params, "Ticket details of 111", 1120, 800);
        });
    })

    describe("Parlay_", () => {
        const transId = "12";
        const transIndex = "34";

        beforeEach(() => {
            appendLoadFixtures("parlay-template.html");
        });

        it("ShowComboParlayDetails -> Show Popup Details", () => {
            // Arrange
            const refNo = "111";
            const winLossDate: Date = new Date();
            const custId = "1";
            const transId = "2";
            const betId = "3";
            const betTeam = "1";
            const params = '?refno=' + refNo
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

        it("ShowComboParlayNewTickets_WhenTicketsContentHtmlIsEmpty -> Call Ajax Get Content", () => {
            // Arrange
            const ticketsContent = $("#divEvent_" + transId + transIndex);
            const comboOfferName = $('#combinationOfferName_' + transId + transIndex);
            const custId = "1";
            const url = "betlist-path/Parlay/ComboParlay" + "?transid=" + transId + "&custId=1";
            ticketsContent.html('');

            spyOn($, 'ajax').and.callFake((options) => {
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

        it("ShowComboParlayNewTickets_TicketsContentHtmlIsNotEmptyAndVisible -> Not Call Ajax Get Content", () => {
            // Arrange
            const ticketsContent = $("#divEvent_" + transId + transIndex);
            const comboOfferName = $('#combinationOfferName_' + transId + transIndex);
            ticketsContent.html('12342343');
            ticketsContent.show();
            const custId = "1";

            // Act
            BetList.Choice.TicketDetails.ParlayTicketDetails.Instance.ShowComboParlayOldTickets(transId, transIndex, '', custId);

            // Assert
            expect(ticketsContent).toHaveHtml('12342343');
            expect(ticketsContent).toBeHidden();
            expect(comboOfferName).toBeVisible();
        });

        it("ShowComboParlayNewTickets_TicketsContentHtmlIsNotEmptyAndHidden -> Not Call Ajax Get Content", () => {
            // Arrange
            const ticketsContent = $("#divEvent_" + transId + transIndex);
            const comboOfferName = $('#combinationOfferName_' + transId + transIndex);
            ticketsContent.html('12342343');
            ticketsContent.hide();
            const custId = "1";

            // Act
            BetList.Choice.TicketDetails.ParlayTicketDetails.Instance.ShowComboParlayOldTickets(transId, transIndex, '', custId);

            // Assert
            expect(ticketsContent).toHaveHtml('12342343');
            expect(ticketsContent).toBeVisible();
            expect(comboOfferName).not.toBeHidden();
        });

        it("ShowMixParlayTickets_Always -> Ajax Send Correct Url", () => {
            // Arrange
            const ticketsContent = $("#divEvent_" + transId);
            const url = "betlist-path/Parlay/MixParlay" + "?transid=" + transId + "&custId=1";
            ticketsContent.html('');

            // CallBack Assert
            spyOn($, 'ajax').and.callFake((options) => {
                expect(options.url).toBe(url);
                options.success('responseHtml');
            });

            // Act
            BetList.Choice.TicketDetails.ParlayTicketDetails.Instance.ShowMixParlayTickets(transId, "2", 1);

            // Assert
            expect(ticketsContent).toHaveHtml('responseHtml');
        });

        it("ShowComboParlayNewTickets_Always -> Ajax Send Correct Url", () => {
            // Arrange
            const refNo = "1";
            const handicap1 = "2";
            const handicap2 = "3";
            const betCheck = "4";
            const betTeam = "5";
            const custId = "1";
            const params = '?transid=' + transId
                + '&handicap1=' + handicap1
                + '&handicap2=' + handicap2
                + '&betcheck=' + betCheck
                + '&betteam=' + betTeam
                + '&custId=1';

            const ticketsContent = $("#divEvent_" + transId + transIndex);
            const url = "betlist-path/Parlay/ComboParlay" + params;
            ticketsContent.html('');

            // CallBack Assert
            spyOn($, 'ajax').and.callFake((options) => {
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

    describe("GlobalDetailsFunction_", () => {
        it("showMP -> Assign Correct Show Details Function", () => {
            // Arrange
            const showDetailsFunc = BetList.Choice.TicketDetails.ParlayTicketDetails.Instance.ShowMixParlayTickets;

            // Assert
            expect(showMP).toBe(showDetailsFunc);
        })

        it("showCombMPDetail -> Assign Correct Show Details Function", () => {
            // Arrange
            const showDetailsFunc = BetList.Choice.TicketDetails.ParlayTicketDetails.Instance.ShowComboParlayDetails;

            // Assert
            expect(showCombMPDetail).toBe(showDetailsFunc);
        })

        it("showCombMP -> Assign Correct Show Details Function", () => {
            // Arrange
            const showDetailsFunc = BetList.Choice.TicketDetails.ParlayTicketDetails.Instance.ShowComboParlayOldTickets;

            // Assert
            expect(showCombMP).toBe(showDetailsFunc);
        })

        it("showNewCombMP -> Assign Correct Show Details Function", () => {
            // Arrange
            const showDetailsFunc = BetList.Choice.TicketDetails.ParlayTicketDetails.Instance.ShowComboParlayNewTickets;

            // Assert
            expect(showNewCombMP).toBe(showDetailsFunc);
        })

        it("showColossusBetsDetails -> Assign Correct Show Details Function", () => {
            // Arrange
            const showDetailsFunc = BetList.Choice.TicketDetails.SportsbookTicketDetails.Instance.ShowColossusBetsDetails;

            // Assert
            expect(showColossusBetsDetails).toBe(showDetailsFunc);
        })
    })
});
