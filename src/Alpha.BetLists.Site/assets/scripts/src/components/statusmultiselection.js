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