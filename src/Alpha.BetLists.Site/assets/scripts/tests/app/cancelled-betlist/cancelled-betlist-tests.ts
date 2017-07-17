describe("CancelledBet", () => {
    const jQueryFake = $;
    const singleSelectionDropdownComponent = new SingleSelectionDropdownComponent($);
    let cancelledBetView: CancelledBetListView,
        cacheElements: jasmine.Spy,
        registerEvents: jasmine.Spy;

    beforeEach(() => {
        jasmine.getFixtures().fixturesPath = "app/templates/cancelled-bet";
        loadFixtures("cancelled-bet-template.html");

        cancelledBetView = new CancelledBetListView(
            jQueryFake,
            singleSelectionDropdownComponent
        );

        spyOn(cancelledBetView, "registerEvents").and.callThrough();
        spyOn(cancelledBetView, "cacheElements").and.callThrough();
        spyOn(cancelledBetView, "initComponents").and.callThrough();
    });

    describe("_Init", () => {
        beforeEach(() => {
            // act
            cancelledBetView.init();
        });

        it("_Call initComponents", () => {
            // assert
            expect(cancelledBetView.initComponents).toHaveBeenCalled();
        });

        it("_Call cacheElements", () => {
            // assert
            expect(cancelledBetView.cacheElements).toHaveBeenCalled();
        });

        it("_Call registerEvents", () => {
            // assert
            expect(cancelledBetView.registerEvents).toHaveBeenCalled();
        });
    });

    describe("_cacheElements", () => {
        it("_Always_All elements have been cached", () => {
            // act
            cancelledBetView.cacheElements();

            // assert
            expect(cancelledBetView.$betlistForm).toBeDefined();
            expect(cancelledBetView.$pageTitle).toBeDefined();
        });
    });

    describe("_onClickIconFilter", () => {
        describe("_Filter is currently shown", () => {
            beforeEach(() => {
                // arrange
                cancelledBetView.cacheElements();

                // act
                cancelledBetView.onClickIconFilter();
            });

            it("_Hide filter", () => {
                // assert
                expect(cancelledBetView.$pageTitle.hasClass("hide-filter")).toBeTruthy();
                expect(cancelledBetView.$betlistForm.hasClass("hide")).toBeTruthy();
            });
        });

        describe("_Filter is currently hidden", () => {
            beforeEach(() => {
                // arrange
                loadFixtures("cancelled-bet-with-hide-filter.html");
                cancelledBetView.cacheElements();

                // act
                cancelledBetView.onClickIconFilter();
            });

            it("_Show filter", () => {
                // assert
                expect(cancelledBetView.$pageTitle.hasClass("hide-filter")).toBeFalsy();
                expect(cancelledBetView.$betlistForm.hasClass("hide")).toBeFalsy();
            });
        });
    });

    describe("_initComponents", () => {
        beforeEach(() => {
            // arrange
            spyOn(singleSelectionDropdownComponent, "render");

            // act
            cancelledBetView.initComponents();
        });

        it("_Call membersComponent.Render", () => {
            // assert
            expect(singleSelectionDropdownComponent.render).toHaveBeenCalledWith("dropdown-members", "ReportType", cancelledBetView.onChangeSelectedOfUsersDropdown);
        });

        it("_Call agentsComponent.Render", () => {
            // assert
            expect(singleSelectionDropdownComponent.render).toHaveBeenCalledWith("dropdown-agents", "ReportType", cancelledBetView.onChangeSelectedOfUsersDropdown);
        });

        it("_Call mastersComponent.Render", () => {
            // assert
            expect(singleSelectionDropdownComponent.render).toHaveBeenCalledWith("dropdown-masters", "ReportType", cancelledBetView.onChangeSelectedOfUsersDropdown);
        });
    });
})