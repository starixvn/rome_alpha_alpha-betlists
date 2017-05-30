describe("RunningBetList", () => {
    const jQueryFake = $;
    const dateRangePickerComponent = new DateRangePickerComponent($);
    const productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    const betlistTypeComponent = new SingleSelectionDropdownComponent($);
    const windowLocation = {
        href: ""
    };

    let runningBetlistView: RunningBetListView,
        cacheDOMElements: jasmine.Spy,
        registerEvents: jasmine.Spy,
        blockUI: jasmine.Spy,
        unblockUI: jasmine.Spy;

    beforeEach(() => {
        jasmine.getFixtures().fixturesPath = "app/templates/running-betlist";
        loadFixtures("running-betlist-template.html");

        runningBetlistView = new RunningBetListView(
            jQueryFake,
            dateRangePickerComponent,
            productMultiSelectionComponent,
            betlistTypeComponent,
            windowLocation);

        spyOn(jQueryFake, "blockUI");
        spyOn(jQueryFake, "unblockUI");
        spyOn(runningBetlistView, "cacheDOMElements").and.callThrough();
        spyOn(runningBetlistView, "bindFilter").and.callThrough();
        spyOn(runningBetlistView, "registerEvents").and.callThrough();
    });

    it("Inderit MemberBetListView_Always_RunningBetListView is MemberBetListView", () => {
        // assert
        expect(runningBetlistView instanceof MemberBetListView).toBeTruthy();
    });

    describe("_init", () => {
        beforeEach(() => {
            // arrange
            spyOn(runningBetlistView, "onChangeGetAllCheckbox");

            // act
            runningBetlistView.init();
        });

        it("_Calls blockUI", () => {
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
        });

        it("_Call cacheDOMElements", () => {
            // assert
            expect(runningBetlistView.cacheDOMElements).toHaveBeenCalled();
        });

        it("_Call bindFilter", () => {
            // assert
            expect(runningBetlistView.bindFilter).toHaveBeenCalled();
        });

        it("_Call bindFilter", () => {
            // assert
            expect(runningBetlistView.bindFilter).toHaveBeenCalled();
        });

        it("_triger GetAll checkbox change event", () => {
            // act
            runningBetlistView.$getallCheckbox.trigger("change");

            // assert
            expect(runningBetlistView.onChangeGetAllCheckbox).toHaveBeenCalled();
        });

        it("_Calls unblockUI", () => {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });

    describe("_cacheDOMElements", () => {
        beforeEach(() => {
            // arrange
            spyOn(MemberBetListView.prototype, "cacheElements");

            // act
            runningBetlistView.cacheDOMElements();
        });

        it("_Call super.cacheElements", () => {
            // assert
            expect(MemberBetListView.prototype.cacheElements).toHaveBeenCalled();
        });

        it("_Cache all element", () => {
            // assert
            expect(runningBetlistView.$fromDate).toBeDefined();
            expect(runningBetlistView.$toDate).toBeDefined();
            expect(runningBetlistView.$getallCheckbox).toBeDefined();
        });
    });

    describe("_registerEvents", () => {
        beforeEach(() => {
            // arrange
            spyOn(MemberBetListView.prototype, "registerEvents");
            spyOn(runningBetlistView, "onChangeGetAllCheckbox");
            runningBetlistView.cacheDOMElements();

            // act
            runningBetlistView.registerEvents();
        });

        it("_Call super.registerEvents", () => {
            // assert
            expect(MemberBetListView.prototype.registerEvents).toHaveBeenCalled();
        });

        it("_Bind onChangeGetAllCheckbox", () => {
            // act
            runningBetlistView.$getallCheckbox.trigger("change");

            // assert
            expect(runningBetlistView.onChangeGetAllCheckbox).toHaveBeenCalled();
        });
    });

    describe("_bindFilter", () => {
        beforeEach(() => {
            // arrange
            spyOn(MemberBetListView.prototype, "bindFilter");

            // act
            runningBetlistView.bindFilter();
        });

        it("_Call super.bindFilter", () => {
            // assert
            expect(MemberBetListView.prototype.bindFilter).toHaveBeenCalled();
        });

        it("_Cache element", () => {
            expect(runningBetlistView.$dateRangeSubmitButton).toBeDefined();
        });
    });

    describe("_onChangeGetAllCheckbox", () => {
        describe("_GetAll is checked", () => {
            beforeEach(() => {
                // arrange
                runningBetlistView.init();

                // act
                runningBetlistView.onChangeGetAllCheckbox();
            });

            it("_Disabed fromDate", () => {
                // assert
                expect(runningBetlistView.$fromDate.attr("disabled")).toBeDefined();
            });

            it("_Disabed toDate", () => {
                // assert
                expect(runningBetlistView.$toDate.attr("disabled")).toBeDefined();
            });

            it("_Disabled DateRangeSubmitButton", () => {
                // assert
                expect(runningBetlistView.$dateRangeSubmitButton.attr("disabled")).toBeDefined();
            });
        })

        describe("_GetAll is unchecked", () => {
            beforeEach(() => {
                //arrange
                loadFixtures("running-betlist-with-uncheck-getall-template.html")
                runningBetlistView.init();

                // act
                runningBetlistView.onChangeGetAllCheckbox();
            });

            it("_Enabed fromDate", () => {
                // assert
                expect(runningBetlistView.$fromDate.attr("disabled")).toBeUndefined();
            });

            it("_Enabed toDate", () => {
                // assert
                expect(runningBetlistView.$toDate.attr("disabled")).toBeUndefined();
            });

            it("_Enabled DateRangeSubmitButton", () => {
                // assert
                expect(runningBetlistView.$dateRangeSubmitButton.attr("disabled")).toBeUndefined();
            });
        });
    });
});