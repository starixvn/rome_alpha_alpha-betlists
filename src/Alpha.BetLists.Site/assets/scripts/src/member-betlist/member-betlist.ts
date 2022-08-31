/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/single-selection-dropdown.ts" />

"use strict";

class MemberBetListView {
    $: JQueryStatic;

    $multiSelectProductButton: JQuery;
    $userSelectedDateRangeOption: JQuery;
    $betlistForm: JQuery;
    $pageTitle: JQuery;
    $isFilterVisible: JQuery;
    $custId: JQuery;

    windowLocation: any;
    dateRangePickerComponent: DateRangePickerComponent;
    productMultiSelectionComponent: ProductMultiSelectionComponent;
    betlistTypeComponent: SingleSelectionDropdownComponent;
    topWindow: any;

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
        this.topWindow = top;
    }

    init = () => {
        this.cacheElements();
        this.bindFilter();
        this.registerEvents();
    };

    public cacheElements() {
        this.$betlistForm = this.$("#betlist-form");

        this.$pageTitle = this.$(".bl_title");
        this.$custId = this.$("#CustId");

        this.$isFilterVisible = this.$("#IsFilterVisible");
    };

    public bindFilter() {
        var betlistTypeComponentOption = new SingleSelectionDropdownOption("dropdown-betlist-type", "BetlistType", this.changeBetlistType);
        this.betlistTypeComponent.render(betlistTypeComponentOption);

        this.productMultiSelectionComponent.render();
        this.$multiSelectProductButton = this.$("#dropdown-product").parent().find(".multiselect.btn");

        this.dateRangePickerComponent.render();
    };

    changeBetlistType = () => {
        var url = this.betlistTypeComponent.$dropdown.find("option:selected").data("url");
        if (url.indexOf(this.topWindow.menuBar.domElements.reportDomain) !== -1 && url.indexOf('(S(') !== -1) {
            url = url.replace(this.topWindow.menuBar.domElements.sessionId, this.topWindow.menuBar.domElements.reportSessionId);
        }

        this.windowLocation.href = url;
    };

    public registerEvents() {
        this.$betlistForm.submit(this.onSubmitEvent);
        this.$(".icon-filter").on("click", this.onClickIconFilter);
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

    getParams = () => {
        let selectedProducts = this.productMultiSelectionComponent.getSelectedItems();
        let custId = this.$custId.val();
        return {
            CustId: custId,
            IsAnyProductSelected: selectedProducts.length > 0,
            IsFilterVisible: !this.$betlistForm.is(":hidden")
        };
    };

    showValidaProductError = () => this.$multiSelectProductButton.addClass("error");

    screenSmallThan768 = () => {
        var currentWindowWidth = screen.width || this.$(window).width();

        return currentWindowWidth < 768;
    };
}