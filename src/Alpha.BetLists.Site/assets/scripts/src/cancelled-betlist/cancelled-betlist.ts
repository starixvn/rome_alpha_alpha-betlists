"use strict";

class CancelledBetListView {
    $: JQueryStatic;
    $constants: CancelledBetListConstants;
    $betlistForm: JQuery;
    $exportExcelForm: JQuery;
    $pageTitle: JQuery;
    $buttonProduct: JQuery;
    $buttonStatus: JQuery;
    $isFilterVisible: JQuery;
    dateRangePickerComponent: DateRangePickerComponent;
    productMultiSelectionComponent: ProductMultiSelectionComponent;
    statusMultiSelectionComponent: StatusMultiSelectionComponent;
    pagerComponent: PagerComponent;
    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        productMultiSelectionComponent: ProductMultiSelectionComponent,
        statusMultiSelectionComponent: StatusMultiSelectionComponent,
        pagerComponent: PagerComponent) {
        this.$ = $;
        this.$constants = new CancelledBetListConstants();
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.productMultiSelectionComponent = productMultiSelectionComponent;
        this.statusMultiSelectionComponent = statusMultiSelectionComponent;
        this.pagerComponent = pagerComponent;
    }

    public init = () => {
        this.cacheElements();
        this.registerEvents();

        this.dateRangePickerComponent.render(new DateRangePickerOption("daterange-picker", "betlist-form"));
        this.productMultiSelectionComponent.render(
            new ProductMultiSelectionOption("dropdown-product", true, "product-ids"));
        this.statusMultiSelectionComponent.render(
            new StatusMultiSelectionOption("dropdown-status", true, "status-ids"));
        this.$buttonProduct = this.$("#dropdown-product").parent().find(".multiselect.btn");
        this.$buttonStatus = this.$("#dropdown-status").parent().find(".multiselect.btn");
        this.pagerComponent.pageList = this.pagerComponent.smallPageList;
        this.pagerComponent.render();
    }
    
    public registerEvents() {
        this.$(this.$constants.betListFilterId).on("click", this.onClickIconFilter);
        this.$("#icon-excel").on("click", this.onExportExcel);
        this.$betlistForm.submit(this.onSubmitReport);
        this.$("#submit-BetList-Form").on("click", this.resetPaging);
    }

    resetPaging = () => {        
        this.$("#PageIndex").val(1);
    };

    onSubmitReport = (e: any) => {
        let this$ = this.$;
        this$.blockUI();

        let selectedProducts = this.productMultiSelectionComponent.getSelectedItems();
        if (selectedProducts.length <= 0) {
            this.$buttonProduct.addClass("error");
            this$.unblockUI();
            e.preventDefault();

            return;
        }

        let selectedStatus = this.statusMultiSelectionComponent.getSelectedItems();
        if (selectedStatus.length <= 0) {
            this.$buttonStatus.addClass("error");
            this$.unblockUI();
            e.preventDefault();

            return;
        }

        var isFilterVisible = !this.$betlistForm.is(":hidden");
        this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : isFilterVisible));
    }

    onExportExcel = () => {
        this.$exportExcelForm.submit();
        this.$.unblockUI();
    }

    onClickIconFilter = () => {
        let filter = this.$betlistForm;
        let isHidden = filter.hasClass("hide");

        if (isHidden) {
            this.$pageTitle.removeClass("hide-filter");
            this.$betlistForm.removeClass("hide");
        } else {
            this.$pageTitle.addClass("hide-filter");
            this.$betlistForm.addClass("hide");
        }
    }

    public cacheElements() {
        this.$betlistForm = this.$("#betlist-form");
        this.$pageTitle = this.$(".bl_title");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$isFilterVisible = this.$("#IsFilterVisible");
    };
}

$(function () {
    const cancelledBet = new CancelledBetListView(
        $,
        new DateRangePickerComponent($),
        new ProductMultiSelectionComponent($),
        new StatusMultiSelectionComponent($),
        new PagerComponent($));

    cancelledBet.init();
})