"use strict";

interface JQuery {
    nexDatePicker(option: any): void;
}

class DateRangePickerOption {
    constructor(
        public selectorId: string = "daterange-picker",
        public betlistFormId: string = "betlist-form",
        public onChange: Function = null) {
        this.selectorId = selectorId;
        this.betlistFormId = betlistFormId;
        this.onChange = onChange;
    }
}

class DateRangePickerComponent {
    $: JQueryStatic;

    options = new DateRangePickerOption();
    quickChooseType = {
        specificDates: 7
    };
    isCompleteInitDateRangePicker = false;
    $betlistForm: JQuery;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render = (options: DateRangePickerOption = new DateRangePickerOption()) => {
        this.options = this.$.extend({}, this.options, options);

        let dateRangePicker = this.$("#" + this.options.selectorId);
        let currentDate = dateRangePicker.data("currentdate");
        let minDate = dateRangePicker.data("mindate");
        let maxDate = dateRangePicker.data("maxdate");
        this.$betlistForm = this.$("#" + this.options.betlistFormId);
        let shortDateWithSlashFormat = "MM/DD/YYYY";

        dateRangePicker.nexDatePicker({
            datePickerFormat: {
                dateFormat: shortDateWithSlashFormat
            },
            selectedItem: dateRangePicker.data("selecteddaterange"),
            minDate: new Date(minDate),
            maxDate: maxDate === "" ? null : maxDate,
            currentDate: new Date(currentDate),
            onSelectDatePicker: () => {
                if (this.options.onChange) {
                    this.options.onChange();
                }
            },
            onQuickChooseChange: (selectValue: number) => {
                if (this.isCompleteInitDateRangePicker && selectValue !== this.quickChooseType.specificDates) {
                    this.$betlistForm.submit();
                }
            }
        });

        this.isCompleteInitDateRangePicker = true;

        return dateRangePicker;
    };
}