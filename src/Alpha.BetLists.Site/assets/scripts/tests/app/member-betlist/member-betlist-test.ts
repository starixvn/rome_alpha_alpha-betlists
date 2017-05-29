describe("MemberBetList", () => {
    const jQueryFake = $;
    const dateRangePickerComponent = new DateRangePickerComponent($);
    const productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    const betlistTypeComponent = new SingleSelectionDropdownComponent($);
    const windowLocation = {
        href: ""
    };

    let memberBetListView: MemberBetListView;

    let cacheElements: jasmine.Spy,
        registerEvents: jasmine.Spy,
        blockUI: jasmine.Spy,
        unblockUI: jasmine.Spy;

    beforeEach(() => {
        jasmine.getFixtures().fixturesPath = "app/templates";
        loadFixtures("member-betlist-template.html");

        memberBetListView = new MemberBetListView(
            jQueryFake,
            dateRangePickerComponent,
            productMultiSelectionComponent,
            betlistTypeComponent,
            windowLocation);
    });

    describe("_init", () => {
        beforeEach(() => {
            // arrange
            spyOn(jQueryFake, "blockUI");
            spyOn(jQueryFake, "unblockUI");
            spyOn(memberBetListView, "cacheElements").and.callThrough();
            spyOn(memberBetListView, "bindFilter").and.callThrough();
            spyOn(memberBetListView, "registerEvents").and.callThrough();

            // act
            memberBetListView.init();
        });

        it("_Calls blockUI", () => {
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
        });

        it("_Call cacheElements", () => {
            // assert
            expect(memberBetListView.cacheElements).toHaveBeenCalled();
        });

        it("_Call bindFilter", () => {
            // assert
            expect(memberBetListView.bindFilter).toHaveBeenCalled();
        });

        it("_Calls unblockUI", () => {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });

    describe("_cacheElements", () => {
        it("_Always -> All neccessary elements have been cached", () => {
            // act
            memberBetListView.cacheElements();

            // assert
            expect(memberBetListView.$betlistForm).toBeDefined();
            expect(memberBetListView.$pageTitle).toBeDefined();
            expect(memberBetListView.$custId).toBeDefined();
            expect(memberBetListView.$isFilterVisible).toBeDefined();
        });
    });
});