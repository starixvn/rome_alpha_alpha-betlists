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
"use strict";
var ProductMultiSelectionOption = /** @class */ (function () {
    function ProductMultiSelectionOption(dropdownId, validateBeforeSubmit, productsInputId, onChange) {
        if (dropdownId === void 0) { dropdownId = "dropdown-product"; }
        if (validateBeforeSubmit === void 0) { validateBeforeSubmit = true; }
        if (productsInputId === void 0) { productsInputId = "product-ids"; }
        if (onChange === void 0) { onChange = null; }
        this.dropdownId = dropdownId;
        this.validateBeforeSubmit = validateBeforeSubmit;
        this.productsInputId = productsInputId;
        this.onChange = onChange;
    }
    return ProductMultiSelectionOption;
}());
var ProductMultiSelectionComponent = /** @class */ (function () {
    function ProductMultiSelectionComponent($) {
        var _this = this;
        this.options = new ProductMultiSelectionOption();
        this.render = function (options) {
            if (options === void 0) { options = new ProductMultiSelectionOption(); }
            _this.options = _this.$.extend({}, _this.options, options);
            _this.$dropdownProduct = _this.$("#" + _this.options.dropdownId);
            var hasAnyProduct = _this.convertAttributeToBool(_this.$dropdownProduct.data("has-any-product"));
            _this.$hiddenInput = _this.$("#" + _this.options.productsInputId);
            var numberDisplayedProductsOnFilter = 1;
            _this.$dropdownProduct.multiselect({
                maxHeight: 300,
                numberDisplayed: numberDisplayedProductsOnFilter,
                nonSelectedText: hasAnyProduct
                    ? _this.$dropdownProduct.data("label-select-products")
                    : _this.$dropdownProduct.data("label-there-is-no-product"),
                nSelectedText: " " + _this.$dropdownProduct.data("label-selected"),
                allSelectedText: _this.$dropdownProduct.data("label-all-products"),
                includeSelectAllOption: hasAnyProduct,
                onChange: _this.onChange,
                onSelectAll: _this.onChange,
                language: _this.$dropdownProduct.data("language"),
                enableCollapsibleOptGroups: true,
                optionClass: _this.optionClass
            });
            _this.$dropdownProduct.next().find(".multiselect-container li").removeClass("hidden");
            _this.$multiSelectButton = _this.$dropdownProduct.parent().find(".multiselect.btn");
            return _this.$dropdownProduct;
        };
        this.onChange = function (option, checked) {
            _this.validate();
            _this.$hiddenInput.val(_this.getSelectedItems().join(","));
            if (_this.options.onChange) {
                _this.options.onChange();
            }
        };
        this.optionClass = function (element) {
            if (_this.$(element).data("isnested") === 1) {
                return "multiselect-nested-item";
            }
        };
        this.validate = function () {
            if (_this.options.validateBeforeSubmit
                && _this.$multiSelectButton
                && _this.$dropdownProduct.find("option:selected").length
                && _this.$multiSelectButton.hasClass("error")) {
                _this.$multiSelectButton.removeClass("error");
            }
        };
        this.getSelectedItems = function () {
            var selected = [];
            var selectedOptions = _this.$dropdownProduct.find("option:selected");
            var this$ = _this.$;
            selectedOptions.each(function () {
                selected.push([this$(this).val()]);
            });
            return selected;
        };
        this.convertAttributeToBool = function (attrValue) {
            return !!attrValue && attrValue.toString().toLowerCase() === "true";
        };
        this.$ = $;
    }
    return ProductMultiSelectionComponent;
}());
//# sourceMappingURL=productmultiselection.js.map
"use strict";
var SingleSelectionDropdownOption = /** @class */ (function () {
    function SingleSelectionDropdownOption(dropdownId, inputId, onChange) {
        if (dropdownId === void 0) { dropdownId = "dropdown-single"; }
        if (inputId === void 0) { inputId = "UserSelectedId"; }
        if (onChange === void 0) { onChange = null; }
        this.dropdownId = dropdownId;
        this.inputId = inputId;
        this.onChange = onChange;
    }
    return SingleSelectionDropdownOption;
}());
var SingleSelectionDropdownComponent = /** @class */ (function () {
    function SingleSelectionDropdownComponent($) {
        var _this = this;
        this.options = new SingleSelectionDropdownOption();
        this.render = function (options) {
            if (options === void 0) { options = new SingleSelectionDropdownOption(); }
            _this.options = _this.$.extend({}, _this.options, options);
            var $dropdown = _this.$("#" + _this.options.dropdownId);
            _this.$dropdown = $dropdown;
            var $hiddenInput = _this.$("#" + _this.options.inputId);
            _this.$hiddenInput = $hiddenInput;
            var self = _this;
            _this.selectedValue = self.getSelectedItem();
            _this.$dropdown.multiselect({
                multiple: false,
                maxHeight: 300,
                numberDisplayed: 1,
                includeSelectAllOption: false,
                onChange: _this.onchange
            });
        };
        this.getSelectedItem = function () {
            var selected = [];
            var selectedOptions = _this.$dropdown.find("option:selected");
            var this$ = _this.$;
            selectedOptions.each(function () {
                selected.push([this$(this).val()]);
            });
            return selected.length ? selected[0].toString() : "";
        };
        this.updateSelectedValue = function () {
            _this.$hiddenInput.val(_this.selectedValue);
        };
        this.onchange = function () {
            _this.selectedValue = _this.getSelectedItem();
            if (_this.options.onChange) {
                _this.options.onChange();
            }
        };
        this.$ = $;
    }
    return SingleSelectionDropdownComponent;
}());
//# sourceMappingURL=single-selection-dropdown.js.map
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
            if (url.indexOf(_this.topWindow.menuBar.domElements.reportDomain) !== -1 && url.indexOf('(S(') !== -1) {
                url = url.replace(_this.topWindow.menuBar.domElements.sessionId, _this.topWindow.menuBar.domElements.reportSessionId);
            }
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
        this.topWindow = top;
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