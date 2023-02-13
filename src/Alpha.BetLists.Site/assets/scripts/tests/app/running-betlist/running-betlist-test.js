describe("RunningBetList", function () {
    var jQueryFake = $;
    var dateRangePickerComponent = new DateRangePickerComponent($);
    var productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    var betlistTypeComponent = new SingleSelectionDropdownComponent($);
    var windowLocation = {
        href: ""
    };
    var runningBetlistView, cacheDOMElements, registerEvents, blockUI, unblockUI;
    beforeEach(function () {
        jasmine.getFixtures().fixturesPath = "app/templates/running-betlist";
        loadFixtures("running-betlist-template.html");
        runningBetlistView = new RunningBetListView(jQueryFake, dateRangePickerComponent, productMultiSelectionComponent, betlistTypeComponent, windowLocation);
        spyOn(jQueryFake, "blockUI");
        spyOn(jQueryFake, "unblockUI");
        spyOn(runningBetlistView, "cacheDOMElements").and.callThrough();
        spyOn(runningBetlistView, "bindFilter").and.callThrough();
        spyOn(runningBetlistView, "registerEvents").and.callThrough();
    });
    it("Inderit MemberBetListView_Always_RunningBetListView is MemberBetListView", function () {
        // assert
        expect(runningBetlistView instanceof MemberBetListView).toBeTruthy();
    });
    describe("_init", function () {
        beforeEach(function () {
            // arrange
            spyOn(runningBetlistView, "onChangeGetAllCheckbox");
            // act
            runningBetlistView.init();
        });
        it("_Call cacheDOMElements", function () {
            // assert
            expect(runningBetlistView.cacheDOMElements).toHaveBeenCalled();
        });
        it("_Call bindFilter", function () {
            // assert
            expect(runningBetlistView.bindFilter).toHaveBeenCalled();
        });
        it("_Call bindFilter", function () {
            // assert
            expect(runningBetlistView.bindFilter).toHaveBeenCalled();
        });
        it("_triger GetAll checkbox change event", function () {
            // act
            runningBetlistView.$getallCheckbox.trigger("change");
            // assert
            expect(runningBetlistView.onChangeGetAllCheckbox).toHaveBeenCalled();
        });
    });
    describe("_cacheDOMElements", function () {
        beforeEach(function () {
            // arrange
            spyOn(MemberBetListView.prototype, "cacheElements");
            // act
            runningBetlistView.cacheDOMElements();
        });
        it("_Call super.cacheElements", function () {
            // assert
            expect(MemberBetListView.prototype.cacheElements).toHaveBeenCalled();
        });
        it("_Cache all element", function () {
            // assert
            expect(runningBetlistView.$fromDate).toBeDefined();
            expect(runningBetlistView.$toDate).toBeDefined();
            expect(runningBetlistView.$getallCheckbox).toBeDefined();
        });
    });
    describe("_registerEvents", function () {
        beforeEach(function () {
            // arrange
            spyOn(MemberBetListView.prototype, "registerEvents");
            spyOn(runningBetlistView, "onChangeGetAllCheckbox");
            runningBetlistView.cacheDOMElements();
            // act
            runningBetlistView.registerEvents();
        });
        it("_Call super.registerEvents", function () {
            // assert
            expect(MemberBetListView.prototype.registerEvents).toHaveBeenCalled();
        });
        it("_Bind onChangeGetAllCheckbox", function () {
            // act
            runningBetlistView.$getallCheckbox.trigger("change");
            // assert
            expect(runningBetlistView.onChangeGetAllCheckbox).toHaveBeenCalled();
        });
    });
    describe("_bindFilter", function () {
        beforeEach(function () {
            // arrange
            spyOn(MemberBetListView.prototype, "bindFilter");
            // act
            runningBetlistView.bindFilter();
        });
        it("_Call super.bindFilter", function () {
            // assert
            expect(MemberBetListView.prototype.bindFilter).toHaveBeenCalled();
        });
        it("_Cache element", function () {
            expect(runningBetlistView.$dateRangeSubmitButton).toBeDefined();
        });
    });
    describe("_onChangeGetAllCheckbox", function () {
        describe("_GetAll is checked", function () {
            beforeEach(function () {
                // arrange
                runningBetlistView.init();
                // act
                runningBetlistView.onChangeGetAllCheckbox();
            });
            it("_Disabed fromDate", function () {
                // assert
                expect(runningBetlistView.$fromDate.attr("disabled")).toBeDefined();
            });
            it("_Disabed toDate", function () {
                // assert
                expect(runningBetlistView.$toDate.attr("disabled")).toBeDefined();
            });
            it("_Disabled DateRangeSubmitButton", function () {
                // assert
                expect(runningBetlistView.$dateRangeSubmitButton.attr("disabled")).toBeDefined();
            });
        });
        describe("_GetAll is unchecked", function () {
            beforeEach(function () {
                //arrange
                loadFixtures("running-betlist-with-uncheck-getall-template.html");
                runningBetlistView.init();
                // act
                runningBetlistView.onChangeGetAllCheckbox();
            });
            it("_Enabed fromDate", function () {
                // assert
                expect(runningBetlistView.$fromDate.attr("disabled")).toBeUndefined();
            });
            it("_Enabed toDate", function () {
                // assert
                expect(runningBetlistView.$toDate.attr("disabled")).toBeUndefined();
            });
            it("_Enabled DateRangeSubmitButton", function () {
                // assert
                expect(runningBetlistView.$dateRangeSubmitButton.attr("disabled")).toBeUndefined();
            });
        });
    });
});
//# sourceMappingURL=running-betlist-test.js.map