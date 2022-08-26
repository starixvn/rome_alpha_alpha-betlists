/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/single-selection-dropdown.ts" />
"use strict";
var MemberBetListView = /** @class */ (function () {
    function MemberBetListView($, dateRangePickerComponent, productMultiSelectionComponent, betlistTypeComponent, windowLocation) {
        var _this = this;
        this.init = function () {
            _this.cacheElements();
            _this.bindFilter();
            _this.registerEvents();
        };
        this.changeBetlistType = function () {
            var url = _this.betlistTypeComponent.$dropdown.find("option:selected").data("url");
            _this.windowLocation.href = url;
        };
        this.onSubmitEvent = function (e) {
            _this.$.blockUI();
            var params = _this.getParams();
            if (!params.IsAnyProductSelected) {
                _this.showValidaProductError();
                _this.$.unblockUI();
                e.preventDefault();
                return;
            }
            _this.$isFilterVisible.val(String(_this.screenSmallThan768() ? false : params.IsFilterVisible));
        };
        this.onClickIconFilter = function () {
            var filter = _this.$betlistForm;
            var currentHideState = filter.hasClass("hide");
            if (currentHideState) {
                _this.$pageTitle.removeClass("hide-filter");
                _this.$betlistForm.removeClass("hide");
            }
            else {
                _this.$pageTitle.addClass("hide-filter");
                _this.$betlistForm.addClass("hide");
            }
        };
        this.getParams = function () {
            var selectedProducts = _this.productMultiSelectionComponent.getSelectedItems();
            var custId = _this.$custId.val();
            return {
                CustId: custId,
                IsAnyProductSelected: selectedProducts.length > 0,
                IsFilterVisible: !_this.$betlistForm.is(":hidden")
            };
        };
        this.showValidaProductError = function () { return _this.$multiSelectProductButton.addClass("error"); };
        this.screenSmallThan768 = function () {
            var currentWindowWidth = screen.width || _this.$(window).width();
            return currentWindowWidth < 768;
        };
        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.productMultiSelectionComponent = productMultiSelectionComponent;
        this.betlistTypeComponent = betlistTypeComponent;
        this.windowLocation = windowLocation;
    }
    MemberBetListView.prototype.cacheElements = function () {
        this.$betlistForm = this.$("#betlist-form");
        this.$pageTitle = this.$(".bl_title");
        this.$custId = this.$("#CustId");
        this.$isFilterVisible = this.$("#IsFilterVisible");
    };
    ;
    MemberBetListView.prototype.bindFilter = function () {
        var betlistTypeComponentOption = new SingleSelectionDropdownOption("dropdown-betlist-type", "BetlistType", this.changeBetlistType);
        this.betlistTypeComponent.render(betlistTypeComponentOption);
        this.productMultiSelectionComponent.render();
        this.$multiSelectProductButton = this.$("#dropdown-product").parent().find(".multiselect.btn");
        this.dateRangePickerComponent.render();
    };
    ;
    MemberBetListView.prototype.registerEvents = function () {
        this.$betlistForm.submit(this.onSubmitEvent);
        this.$(".icon-filter").on("click", this.onClickIconFilter);
    };
    return MemberBetListView;
}());
//# sourceMappingURL=member-betlist.js.map