/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/productmultiselection.ts" />

"use strict";

class OutstandingView {
    $: JQueryStatic;
    $tableBetlist: JQuery;

    $multiSelectButton: JQuery;
    $userSelectedDateRangeOption: JQuery;
    $betlistForm: JQuery;
    $pageTitle: JQuery;
    $submitButton: JQuery;
    $isFilterVisible: JQuery;

    $getallCheckbox: JQuery;
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
        this.$getallCheckbox.trigger("change");

        this.$.unblockUI();
    };

    cacheDOMElements = () => {
        this.$tableBetlist = this.$(".tbl-betlist");
        this.$betlistForm = this.$("#betlist-form");

        this.$pageTitle = this.$(".bl_title");
        this.$fromDate = this.$("#from-date");
        this.$toDate = this.$("#to-date");
        this.$custId = this.$("#CustId");

        this.$getallCheckbox = this.$("#GetAll");
        this.$isFilterVisible = this.$("#IsFilterVisible");
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
        this.$(".icon-filter").on("click", this.onClickIconFilter);
        this.$getallCheckbox.change(this.onChangeGetAllCheckbox);
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

        this.$isFilterVisible.val(String(this.screenSmallThan768() ? false : params.IsFilterVisible));
    };

    onClickIconFilter = () => {
        let filter = this.$betlistForm;
        let currentHideState = filter.hasClass("hide");

        if (currentHideState) {
            this.$pageTitle.removeClass("hide-filter");
            this.$betlistForm.removeClass("hide");
        } else {
            this.$pageTitle.addClass("hide-filter");
            this.$betlistForm.addClass("hide");
        }
    };

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

    getParams = () => {
        let selectedProducts = this.productMultiSelectionComponent.getSelectedItems();
        let custId = this.$custId.val();
        return {
            CustId: custId,
            IsAnyProductSelected: selectedProducts.length > 0,
            IsFilterVisible: !this.$betlistForm.is(":hidden")
        };
    };

    showValidaProductError = () => this.$multiSelectButton.addClass("error");

    screenSmallThan768 = () => {
        var currentWindowWidth = screen.width || this.$(window).width();

        return currentWindowWidth < 768;
    };
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