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

        spyOn(jQueryFake, "blockUI");
        spyOn(jQueryFake, "unblockUI");
        spyOn(memberBetListView, "cacheElements").and.callThrough();
        spyOn(memberBetListView, "bindFilter").and.callThrough();
        spyOn(memberBetListView, "registerEvents").and.callThrough();
    });

    describe("_init", () => {
        beforeEach(() => {
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

    describe("_bindFilter", () => {
        beforeEach(() => {
            // arrange
            spyOn(betlistTypeComponent, "render");
            spyOn(dateRangePickerComponent, "render");

            // act
            memberBetListView.bindFilter();
        });

        it("_Calls dateRangePickerComponent.render", () => {
            // assert
            expect(dateRangePickerComponent.render).toHaveBeenCalled();
        });

        it("_Calls betlistTypeComponent.render", () => {
            // assert
            expect(betlistTypeComponent.render).toHaveBeenCalledWith(
                new SingleSelectionDropdownOption("dropdown-betlist-type", "BetlistType", memberBetListView.changeBetlistType));
        });
    });

    describe("_changeBetlistType", () => {
        beforeEach(() => {
            // arrange
            memberBetListView.init();
            memberBetListView.betlistTypeComponent.$dropdown = $("#betlist-type");

            // act
            memberBetListView.changeBetlistType();
        });

        it("_assign location.href url", () => {
            // assert
            expect(windowLocation.href).toBe("NewPage");
        });
    });

    describe("onSubmitEvent", () => {
        let event = { preventDefault: jasmine.createSpy("preventDefault", () => null) };

        beforeEach(() => {
            // arrange
            spyOn(productMultiSelectionComponent, "render").and.callThrough();
            memberBetListView.init();
        });

        it("_Not select any product -> Blocks UI; Shows validate product error; Unblock UI;", () => {
            // arrange
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(() => []);

            // act
            memberBetListView.onSubmitEvent(event);

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
            expect(memberBetListView.$multiSelectProductButton.hasClass("error")).toBeTruthy();
        });

        it("_Have selected any products -> Blocks UI; Unblock UI; Set correct Params;", () => {
            // arrange
            spyOn(memberBetListView.$isFilterVisible, "val");

            // act
            memberBetListView.onSubmitEvent(event);

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
            expect(memberBetListView.$isFilterVisible.val).toHaveBeenCalledWith("true");
            expect(memberBetListView.$multiSelectProductButton.hasClass("error")).toBeFalsy();
        });
    });
});