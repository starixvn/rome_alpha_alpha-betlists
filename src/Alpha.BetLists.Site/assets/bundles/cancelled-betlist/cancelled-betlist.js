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
var CancelledBetListConstants = /** @class */ (function () {
    function CancelledBetListConstants() {
        this.betListFilterId = "#betlist-filter";
        this.dropdownUsersClass = ".dropdown-users";
        this.betListFormId = "#betlist-form";
        this.pageTitleClass = ".bl_title";
    }
    return CancelledBetListConstants;
}());
//# sourceMappingURL=cancelled-betlist-constants.js.map
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
                selectAllText: _this.$dropdownProduct.data("label-select-all"),
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
var StatusMultiSelectionOption = /** @class */ (function () {
    function StatusMultiSelectionOption(dropdownId, validateBeforeSubmit, statusInputId, onChange) {
        if (dropdownId === void 0) { dropdownId = "dropdown-status"; }
        if (validateBeforeSubmit === void 0) { validateBeforeSubmit = true; }
        if (statusInputId === void 0) { statusInputId = "status-ids"; }
        if (onChange === void 0) { onChange = null; }
        this.dropdownId = dropdownId;
        this.validateBeforeSubmit = validateBeforeSubmit;
        this.statusInputId = statusInputId;
        this.onChange = onChange;
    }
    return StatusMultiSelectionOption;
}());
var StatusMultiSelectionComponent = /** @class */ (function () {
    function StatusMultiSelectionComponent($) {
        var _this = this;
        this.options = new StatusMultiSelectionOption();
        this.render = function (options) {
            if (options === void 0) { options = new StatusMultiSelectionOption(); }
            _this.options = _this.$.extend({}, _this.options, options);
            _this.$dropdownStatus = _this.$("#" + _this.options.dropdownId);
            var hasAnyProduct = _this.convertAttributeToBool(_this.$dropdownStatus.data("has-any-status"));
            _this.$hiddenInput = _this.$("#" + _this.options.statusInputId);
            var numberDisplayedStatusOnFilter = 1;
            _this.$dropdownStatus.multiselect({
                maxHeight: 300,
                numberDisplayed: numberDisplayedStatusOnFilter,
                nonSelectedText: hasAnyProduct
                    ? _this.$dropdownStatus.data("label-select-status")
                    : _this.$dropdownStatus.data("label-there-is-no-status"),
                nSelectedText: " " + _this.$dropdownStatus.data("label-selected"),
                allSelectedText: _this.$dropdownStatus.data("label-all-status"),
                includeSelectAllOption: hasAnyProduct,
                onChange: _this.onChange,
                onSelectAll: _this.onChange,
                language: _this.$dropdownStatus.data("language")
            });
            _this.$multiSelectButton = _this.$dropdownStatus.parent().find(".multiselect.btn");
            return _this.$dropdownStatus;
        };
        this.onChange = function (option, checked) {
            _this.validate();
            _this.$hiddenInput.val(_this.getSelectedItems().join(","));
            if (_this.options.onChange) {
                _this.options.onChange();
            }
        };
        this.validate = function () {
            if (_this.options.validateBeforeSubmit
                && _this.$multiSelectButton
                && _this.$dropdownStatus.find("option:selected").length
                && _this.$multiSelectButton.hasClass("error")) {
                _this.$multiSelectButton.removeClass("error");
            }
        };
        this.getSelectedItems = function () {
            var selected = [];
            var selectedOptions = _this.$dropdownStatus.find("option:selected");
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
    return StatusMultiSelectionComponent;
}());
//# sourceMappingURL=statusmultiselection.js.map
"use strict";
var PagerOption = /** @class */ (function () {
    function PagerOption(onSelectPage, pagerId, formId, pageIndexId, pageSizeId) {
        if (onSelectPage === void 0) { onSelectPage = null; }
        if (pagerId === void 0) { pagerId = "betlist-pager"; }
        if (formId === void 0) { formId = "betlist-form"; }
        if (pageIndexId === void 0) { pageIndexId = "PageIndex"; }
        if (pageSizeId === void 0) { pageSizeId = "PageSize"; }
        this.onSelectPage = onSelectPage;
        this.pagerId = pagerId;
        this.formId = formId;
        this.pageIndexId = pageIndexId;
        this.pageSizeId = pageSizeId;
    }
    return PagerOption;
}());
var PagerComponent = /** @class */ (function () {
    function PagerComponent($) {
        var _this = this;
        this.pageList = [1000, 2000, 3000, 5000];
        this.smallPageList = [10, 50, 100, 500];
        this.render = function (options) {
            if (options === void 0) { options = new PagerOption(); }
            var self = _this;
            var pagerOption = _this.$.extend({}, _this.options, options);
            var pager = _this.$("#" + pagerOption.pagerId);
            _this.$betlistForm = _this.$("#" + _this.options.formId);
            _this.totalRecords = pager.data("totalrecords");
            _this.pageSize = pager.data("pagesize");
            _this.pageIndex = pager.data("pageindex");
            _this.$pageIndex =
                _this.$("<input type=\"hidden\" id=\"" + pagerOption.pageIndexId + "\" name=\"" + pagerOption.pageIndexId + "\" value=\"" + _this.pageIndex + "\" />");
            _this.$pageSize =
                _this.$("<input type=\"hidden\" id=\"" + pagerOption.pageSizeId + "\" name=\"" + pagerOption.pageSizeId + "\" value=\"" + _this.pageSize + "\" />");
            _this.$betlistForm.append(_this.$pageIndex).append(_this.$pageSize);
            if (_this.isShow()) {
                pager.pagination({
                    pageList: _this.pageList,
                    total: _this.totalRecords,
                    pageSize: _this.pageSize,
                    pageNumber: _this.pageIndex,
                    showRefresh: false,
                    onSelectPage: function (pageNumber, pageSize) {
                        self.updateHiddenFields(pageNumber, pageSize);
                        self.onChange();
                    },
                    beforePageText: pager.data("lablepage"),
                    afterPageText: pager.data("labelof") + " {pages}",
                    displayMsg: pager.data("labeldisplayitems")
                });
                pager.removeClass("hide");
            }
            else {
                pager.addClass("hide");
            }
            return pager;
        };
        this.onChange = function () {
            _this.$betlistForm.trigger("submit");
        };
        this.updateHiddenFields = function (pageNumber, pageSize) {
            _this.$pageIndex.val(pageNumber);
            _this.$pageSize.val(pageSize);
        };
        this.isShow = function () {
            // total records less than min page size => do not need to show pager
            return _this.pageList[0] <= _this.totalRecords;
        };
        this.hasAllDataOnClient = function () {
            var result = _this.totalRecords <= _this.pageSize;
            return result;
        };
        this.options = new PagerOption(this.onChange);
        this.$ = $;
    }
    return PagerComponent;
}());
//# sourceMappingURL=pager.js.map