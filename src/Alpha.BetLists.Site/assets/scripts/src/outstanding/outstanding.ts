/// <reference path="../components/core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/productmultiselection.ts" />

"use strict";

class OutstandingView {
    $: JQueryStatic;
    $tableBetlist: JQuery;

    $multiSelectButton: JQuery;
    $userSelectedDateRangeOption: JQuery;
    $betlistForm: JQuery;
    $submitButton: JQuery;
    $fromDate: JQuery;
    $toDate: JQuery;
    $custId: JQuery;

    windowLocation: any;
    dateRangePickerComponent: DateRangePickerComponent;
    productMultiSelectionComponent: ProductMultiSelectionComponent;
    betlistTypeComponent: SingleSelectionDropdownComponent;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        productMultiSelectionComponent: ProductMultiSelectionComponent,
        betlistTypeComponent: SingleSelectionDropdownComponent,
        windowLocation: any
    ) {
        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.productMultiSelectionComponent = productMultiSelectionComponent;
        this.betlistTypeComponent = betlistTypeComponent;
        this.windowLocation = windowLocation;
    }

    init = () => {
        this.$.blockUI();

        this.cacheDOMElements();
        this.bindFilter();
        this.registerEvents();

        this.$.unblockUI();
    };

    cacheDOMElements = () => {
        this.$tableBetlist = this.$(".tbl-betlist");
        this.$betlistForm = this.$("#betlist-form");
        this.$fromDate = this.$("#from-date");
        this.$toDate = this.$("#to-date");
        this.$custId = this.$("#cust-id");
    };

    bindFilter = () => {
        var betlistTypeComponentOption = new SingleSelectionDropdownOption("dropdown-betlist-type", "BetlistType", this.changeBetlistType);
        this.betlistTypeComponent.render(betlistTypeComponentOption);

        this.productMultiSelectionComponent.render();
        this.$multiSelectButton = this.$("#dropdown-product").parent().find(".multiselect.btn");

        this.dateRangePickerComponent.render();
    };

    changeBetlistType = () => {
        var url = this.betlistTypeComponent.$dropdown.find("option:selected").data("url");
        this.windowLocation.href = url;
    };

    public registerEvents() {
        this.$betlistForm.submit(this.onSubmitEvent);
    }

    onSubmitEvent = (e: any) => {
        this.$.blockUI();

        let params = this.getParams();
        if (!params.IsAnyProductSelected) {
            this.showValidaProductError();
            this.$.unblockUI();
            e.preventDefault();

            return;
        }
    };

    getParams = () => {
        let selectedProducts = this.productMultiSelectionComponent.getSelectedItems();
        let custId = this.$custId.val();
        return {
            CustId: custId,
            IsAnyProductSelected: selectedProducts.length > 0,
        };
    };

    showValidaProductError = () => this.$multiSelectButton.addClass("error");
}

$(function () {
    const outstandingView = new OutstandingView(
        $,
        new DateRangePickerComponent($),
        new ProductMultiSelectionComponent($),
        new SingleSelectionDropdownComponent($),
        window.location);

    outstandingView.init();
})