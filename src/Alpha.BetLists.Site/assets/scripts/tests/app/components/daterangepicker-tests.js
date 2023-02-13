var _this = this;
describe("DateRangePicker", function () {
    var dateRangePickerComponent;
    beforeEach(function () {
        jasmine.getFixtures().fixturesPath = "app/templates";
        loadFixtures("daterangepicker-template.html");
        dateRangePickerComponent = new DateRangePickerComponent($);
    });
    describe("_render", function () {
        beforeEach(function () {
            // arrange
            spyOn($.fn, "nexDatePicker");
            spyOn($.fn, "multiselect");
            // act
            dateRangePickerComponent.render();
        });
        it("_Binds nexDatePicker to date range element", function () {
            // assert
            expect($.fn.nexDatePicker).toHaveBeenCalledWith({
                datePickerFormat: {
                    dateFormat: "MM/DD/YYYY"
                },
                selectedItem: 0,
                minDate: new Date("01/05/2013"),
                maxDate: null,
                currentDate: new Date("01/05/2016"),
                onSelectDatePicker: jasmine.any(Function),
                onQuickChooseChange: jasmine.any(Function)
            });
        });
    });
    describe("_onSelectDatePicker", function () {
        var onChange = {
            event: new Function()
        };
        beforeEach(function () {
            // arrange
            spyOn(onChange, "event");
            dateRangePickerComponent.render(new DateRangePickerOption("daterange-picker", "betlist-form", onChange.event));
            // act
            _this.$("#dropdown-daterange").val("7").change();
        });
        it("_Calls Change Event", function () {
            // assert
            expect(onChange.event).toHaveBeenCalled();
        });
    });
    describe("_onQuickChooseChange", function () {
        var $betlistForm;
        beforeEach(function () {
            // arrange
            dateRangePickerComponent.render();
            $betlistForm = dateRangePickerComponent.$betlistForm;
            spyOn($betlistForm, "submit");
        });
        it("_NotCompleteRender;NotChooseSpecificDates => Not Call Submit", function () {
            // arrange
            dateRangePickerComponent.isCompleteInitDateRangePicker = false;
            // act
            _this.$("#dropdown-daterange").val("1").change();
            // assert
            expect($betlistForm.submit).not.toHaveBeenCalled();
        });
        it("_CompleteRender;NotChooseSpecificDates => Call Submit", function () {
            // act
            _this.$("#dropdown-daterange").val("1").change();
            // assert
            expect($betlistForm.submit).toHaveBeenCalled();
        });
        it("_CompleteRender_ChooseSpecificDates => Not Call Submit", function () {
            // act
            _this.$("#dropdown-daterange").val("7").change();
            // assert
            expect($betlistForm.submit).not.toHaveBeenCalled();
        });
    });
});
//# sourceMappingURL=daterangepicker-tests.js.map