/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/single-selection-dropdown.ts" />

"use strict";

class RunningBetListView extends MemberBetListView {
    $: JQueryStatic;

    $getallCheckbox: JQuery;
    $fromDate: JQuery;
    $toDate: JQuery;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        productMultiSelectionComponent: ProductMultiSelectionComponent,
        betlistTypeComponent: SingleSelectionDropdownComponent,
        windowLocation: any) {
        super($, dateRangePickerComponent, productMultiSelectionComponent, betlistTypeComponent, windowLocation);

        this.$ = $;
    }

    init = () => {
        this.$.blockUI();

        this.cacheDOMElements();
        this.bindFilter();
        this.registerEvents();
        this.$getallCheckbox.trigger("change");

        this.$.unblockUI();
    };

    cacheDOMElements = () => {
        super.cacheElements();
        this.$fromDate = this.$("#from-date");
        this.$toDate = this.$("#to-date");
        this.$getallCheckbox = this.$("#GetAll");
    };

    changeBetlistType = () => {
        var url = this.betlistTypeComponent.$dropdown.find("option:selected").data("url");
        this.windowLocation.href = url;
    };

    public registerEvents() {
        super.registerEvents();
        this.$getallCheckbox.change(this.onChangeGetAllCheckbox);
    }

    onChangeGetAllCheckbox = () => {
        let checkbox = this.$getallCheckbox;
        let checked = checkbox.is(":checked");

        if (checked) {
            this.$fromDate.prop("disabled", true);
            this.$toDate.prop("disabled", true);
        }
        else {
            this.$fromDate.prop("disabled", false);
            this.$toDate.prop("disabled", false);
        }
    }
}

$(function () {
    const runningBetListView = new RunningBetListView(
        $,
        new DateRangePickerComponent($),
        new ProductMultiSelectionComponent($),
        new SingleSelectionDropdownComponent($),
        window.location);

    runningBetListView.init();
})