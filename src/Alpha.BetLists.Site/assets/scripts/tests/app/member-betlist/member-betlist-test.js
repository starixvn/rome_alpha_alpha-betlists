describe("MemberBetList", function () {
    var jQueryFake = $;
    var dateRangePickerComponent = new DateRangePickerComponent($);
    var productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    var betlistTypeComponent = new SingleSelectionDropdownComponent($);
    var windowLocation = {
        href: ""
    };
    var memberBetListView, cacheElements, registerEvents, blockUI, unblockUI;
    beforeEach(function () {
        jasmine.getFixtures().fixturesPath = "app/templates/member-betlist";
        loadFixtures("member-betlist-template.html");
        memberBetListView = new MemberBetListView(jQueryFake, dateRangePickerComponent, productMultiSelectionComponent, betlistTypeComponent, windowLocation);
        spyOn(jQueryFake, "blockUI");
        spyOn(jQueryFake, "unblockUI");
        spyOn(memberBetListView, "cacheElements").and.callThrough();
        spyOn(memberBetListView, "bindFilter").and.callThrough();
        spyOn(memberBetListView, "registerEvents").and.callThrough();
    });
    describe("_init", function () {
        beforeEach(function () {
            // act
            memberBetListView.init();
        });
        it("_Call cacheElements", function () {
            // assert
            expect(memberBetListView.cacheElements).toHaveBeenCalled();
        });
        it("_Call bindFilter", function () {
            // assert
            expect(memberBetListView.bindFilter).toHaveBeenCalled();
        });
        it("_Call registerEvents", function () {
            // assert
            expect(memberBetListView.registerEvents).toHaveBeenCalled();
        });
    });
    describe("_cacheElements", function () {
        it("_Always -> All neccessary elements have been cached", function () {
            // act
            memberBetListView.cacheElements();
            // assert
            expect(memberBetListView.$betlistForm).toBeDefined();
            expect(memberBetListView.$pageTitle).toBeDefined();
            expect(memberBetListView.$custId).toBeDefined();
            expect(memberBetListView.$isFilterVisible).toBeDefined();
        });
    });
    describe("_bindFilter", function () {
        beforeEach(function () {
            // arrange
            spyOn(betlistTypeComponent, "render");
            spyOn(dateRangePickerComponent, "render");
            // act
            memberBetListView.bindFilter();
        });
        it("_Calls dateRangePickerComponent.render", function () {
            // assert
            expect(dateRangePickerComponent.render).toHaveBeenCalled();
        });
        it("_Calls betlistTypeComponent.render", function () {
            // assert
            expect(betlistTypeComponent.render).toHaveBeenCalledWith(new SingleSelectionDropdownOption("dropdown-betlist-type", "BetlistType", memberBetListView.changeBetlistType));
        });
    });
    describe("_changeBetlistType", function () {
        beforeEach(function () {
            // arrange
            memberBetListView.init();
            memberBetListView.betlistTypeComponent.$dropdown = $("#betlist-type");
            // act
            memberBetListView.changeBetlistType();
        });
        it("_assign location.href url", function () {
            // assert
            expect(windowLocation.href).toBe("NewPage");
        });
    });
    describe("onSubmitEvent", function () {
        var event = { preventDefault: jasmine.createSpy("preventDefault", function () { return null; }) };
        beforeEach(function () {
            // arrange
            spyOn(productMultiSelectionComponent, "render").and.callThrough();
            memberBetListView.init();
        });
        it("_Not select any product -> Blocks UI; Shows validate product error; Unblock UI;", function () {
            // arrange
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(function () { return []; });
            // act
            memberBetListView.onSubmitEvent(event);
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
            expect(memberBetListView.$multiSelectProductButton.hasClass("error")).toBeTruthy();
        });
        it("_Have selected any products -> Blocks UI; Unblock UI; Set correct Params;", function () {
            // arrange
            spyOn(memberBetListView.$isFilterVisible, "val");
            // act
            memberBetListView.onSubmitEvent(event);
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(memberBetListView.$isFilterVisible.val).toHaveBeenCalledWith("true");
            expect(memberBetListView.$multiSelectProductButton.hasClass("error")).toBeFalsy();
        });
    });
    describe("_onClickIconFilter", function () {
        describe("_Filter is currently shown", function () {
            beforeEach(function () {
                // arrange
                memberBetListView.cacheElements();
                // act
                memberBetListView.onClickIconFilter();
            });
            it("_Hides Filter", function () {
                // assert
                expect(memberBetListView.$pageTitle.hasClass("hide-filter")).toBeTruthy();
                expect(memberBetListView.$betlistForm.hasClass("hide")).toBeTruthy();
            });
        });
        describe("_Filter is currently hidden", function () {
            beforeEach(function () {
                // arrange
                loadFixtures("member-betlist-with-hide-filter.html");
                memberBetListView.cacheElements();
                // act
                memberBetListView.onClickIconFilter();
            });
            it("_Shows Filter", function () {
                // assert
                expect(memberBetListView.$pageTitle.hasClass("hide-filter")).toBeFalsy();
                expect(memberBetListView.$betlistForm.hasClass("hide")).toBeFalsy();
            });
        });
    });
});
//# sourceMappingURL=member-betlist-test.js.map