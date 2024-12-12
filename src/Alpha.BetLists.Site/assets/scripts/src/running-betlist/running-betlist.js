/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/single-selection-dropdown.ts" />
"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var RunningBetListView = /** @class */ (function (_super) {
    __extends(RunningBetListView, _super);
    function RunningBetListView($, dateRangePickerComponent, productMultiSelectionComponent, betlistTypeComponent, windowLocation) {
        var _this = _super.call(this, $, dateRangePickerComponent, productMultiSelectionComponent, betlistTypeComponent, windowLocation) || this;
        _this.init = function () {
            _this.cacheDOMElements();
            _this.bindFilter();
            _this.registerEvents();
            _this.$getallCheckbox.trigger("change");
        };
        _this.cacheDOMElements = function () {
            _super.prototype.cacheElements.call(_this);
            _this.$fromDate = _this.$("#from-date");
            _this.$toDate = _this.$("#to-date");
            _this.$getallCheckbox = _this.$("#GetAll");
        };
        _this.onChangeGetAllCheckbox = function () {
            var checkbox = _this.$getallCheckbox;
            var checked = checkbox.is(":checked");
            if (checked) {
                _this.$fromDate.prop("disabled", true);
                _this.$toDate.prop("disabled", true);
                _this.$dateRangeSubmitButton.prop("disabled", true);
            }
            else {
                _this.$fromDate.prop("disabled", false);
                _this.$toDate.prop("disabled", false);
                _this.$dateRangeSubmitButton.prop("disabled", false);
            }
        };
        _this.$ = $;
        return _this;
    }
    RunningBetListView.prototype.bindFilter = function () {
        _super.prototype.bindFilter.call(this);
        this.$dateRangeSubmitButton = this.$("#dropdown-daterange").parent().find(".btn-submit");
    };
    ;
    RunningBetListView.prototype.registerEvents = function () {
        _super.prototype.registerEvents.call(this);
        this.$getallCheckbox.change(this.onChangeGetAllCheckbox);
    };
    return RunningBetListView;
}(MemberBetListView));
$(function () {
    var runningBetListView = new RunningBetListView($, new DateRangePickerComponent($), new ProductMultiSelectionComponent($), new SingleSelectionDropdownComponent($), window.location);
    runningBetListView.init();
});
//# sourceMappingURL=running-betlist.js.map