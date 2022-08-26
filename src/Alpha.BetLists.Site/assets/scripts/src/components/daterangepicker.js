"use strict";
var DateRangePickerOption = /** @class */ (function () {
    function DateRangePickerOption(selectorId, betlistFormId, onChange) {
        if (selectorId === void 0) { selectorId = "daterange-picker"; }
        if (betlistFormId === void 0) { betlistFormId = "betlist-form"; }
        if (onChange === void 0) { onChange = null; }
        this.selectorId = selectorId;
        this.betlistFormId = betlistFormId;
        this.onChange = onChange;
        this.selectorId = selectorId;
        this.betlistFormId = betlistFormId;
        this.onChange = onChange;
    }
    return DateRangePickerOption;
}());
var DateRangePickerComponent = /** @class */ (function () {
    function DateRangePickerComponent($) {
        var _this = this;
        this.options = new DateRangePickerOption();
        this.quickChooseType = {
            specificDates: 7
        };
        this.isCompleteInitDateRangePicker = false;
        this.render = function (options) {
            if (options === void 0) { options = new DateRangePickerOption(); }
            _this.options = _this.$.extend({}, _this.options, options);
            var dateRangePicker = _this.$("#" + _this.options.selectorId);
            var currentDate = dateRangePicker.data("currentdate");
            var minDate = dateRangePicker.data("mindate");
            var maxDate = dateRangePicker.data("maxdate");
            _this.$betlistForm = _this.$("#" + _this.options.betlistFormId);
            var shortDateWithSlashFormat = "MM/DD/YYYY";
            dateRangePicker.nexDatePicker({
                datePickerFormat: {
                    dateFormat: shortDateWithSlashFormat
                },
                selectedItem: dateRangePicker.data("selecteddaterange"),
                minDate: new Date(minDate),
                maxDate: maxDate === "" ? null : maxDate,
                currentDate: new Date(currentDate),
                onSelectDatePicker: function () {
                    if (_this.options.onChange) {
                        _this.options.onChange();
                    }
                },
                onQuickChooseChange: function (selectValue) {
                    if (_this.isCompleteInitDateRangePicker && selectValue !== _this.quickChooseType.specificDates) {
                        _this.$("#PageIndex").val(1);
                        _this.$betlistForm.submit();
                    }
                }
            });
            _this.isCompleteInitDateRangePicker = true;
            return dateRangePicker;
        };
        this.$ = $;
    }
    return DateRangePickerComponent;
}());
//# sourceMappingURL=daterangepicker.js.map