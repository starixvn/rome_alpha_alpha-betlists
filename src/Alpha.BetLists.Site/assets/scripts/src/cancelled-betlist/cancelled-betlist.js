"use strict";
var CancelledBetListView = /** @class */ (function () {
    function CancelledBetListView($, dateRangePickerComponent, productMultiSelectionComponent, statusMultiSelectionComponent, pagerComponent) {
        var _this = this;
        this.init = function () {
            _this.cacheElements();
            _this.registerEvents();
            _this.dateRangePickerComponent.render(new DateRangePickerOption("daterange-picker", "betlist-form"));
            _this.productMultiSelectionComponent.render(new ProductMultiSelectionOption("dropdown-product", true, "product-ids"));
            _this.statusMultiSelectionComponent.render(new StatusMultiSelectionOption("dropdown-status", true, "status-ids"));
            _this.$buttonProduct = _this.$("#dropdown-product").parent().find(".multiselect.btn");
            _this.$buttonStatus = _this.$("#dropdown-status").parent().find(".multiselect.btn");
            _this.pagerComponent.pageList = _this.pagerComponent.smallPageList;
            _this.pagerComponent.render();
        };
        this.resetPaging = function () {
            _this.$("#PageIndex").val(1);
        };
        this.onSubmitReport = function (e) {
            var this$ = _this.$;
            this$.blockUI();
            var selectedProducts = _this.productMultiSelectionComponent.getSelectedItems();
            if (selectedProducts.length <= 0) {
                _this.$buttonProduct.addClass("error");
                this$.unblockUI();
                e.preventDefault();
                return;
            }
            var selectedStatus = _this.statusMultiSelectionComponent.getSelectedItems();
            if (selectedStatus.length <= 0) {
                _this.$buttonStatus.addClass("error");
                this$.unblockUI();
                e.preventDefault();
                return;
            }
            var isFilterVisible = !_this.$betlistForm.is(":hidden");
            _this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : isFilterVisible));
        };
        this.onExportExcel = function () {
            _this.$exportExcelForm.submit();
            _this.$.unblockUI();
        };
        this.onClickIconFilter = function () {
            var filter = _this.$betlistForm;
            var isHidden = filter.hasClass("hide");
            if (isHidden) {
                _this.$pageTitle.removeClass("hide-filter");
                _this.$betlistForm.removeClass("hide");
            }
            else {
                _this.$pageTitle.addClass("hide-filter");
                _this.$betlistForm.addClass("hide");
            }
        };
        this.$ = $;
        this.$constants = new CancelledBetListConstants();
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.productMultiSelectionComponent = productMultiSelectionComponent;
        this.statusMultiSelectionComponent = statusMultiSelectionComponent;
        this.pagerComponent = pagerComponent;
    }
    CancelledBetListView.prototype.registerEvents = function () {
        this.$(this.$constants.betListFilterId).on("click", this.onClickIconFilter);
        this.$("#icon-excel").on("click", this.onExportExcel);
        this.$betlistForm.submit(this.onSubmitReport);
        this.$("#submit-BetList-Form").on("click", this.resetPaging);
    };
    CancelledBetListView.prototype.cacheElements = function () {
        this.$betlistForm = this.$("#betlist-form");
        this.$pageTitle = this.$(".bl_title");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$isFilterVisible = this.$("#IsFilterVisible");
    };
    ;
    return CancelledBetListView;
}());
$(function () {
    var cancelledBet = new CancelledBetListView($, new DateRangePickerComponent($), new ProductMultiSelectionComponent($), new StatusMultiSelectionComponent($), new PagerComponent($));
    cancelledBet.init();
});
//# sourceMappingURL=cancelled-betlist.js.map