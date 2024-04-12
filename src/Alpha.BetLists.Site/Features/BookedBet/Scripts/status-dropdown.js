"use strict";
var StatusDropdownOption = /** @class */ (function () {
    function StatusDropdownOption(listData, onDropdownHidden, selectorStatusId) {
        if (onDropdownHidden === void 0) { onDropdownHidden = Function(); }
        if (selectorStatusId === void 0) { selectorStatusId = "dropdown-status"; }
        this.listData = listData;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorStatusId = selectorStatusId;
        this.listData = listData;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorStatusId = selectorStatusId;
    }
    return StatusDropdownOption;
}());
var StatusDropdownComponent = /** @class */ (function () {
    function StatusDropdownComponent() {
        this.onlyShowHistoricStatusIds = [];
    }
    StatusDropdownComponent.prototype.render = function (options) {
        this.renderDropdown(options);
        $("#" + options.selectorStatusId).closest(".form-group").removeClass("hide");
    };
    StatusDropdownComponent.prototype.renderDropdown = function (options) {
        var self = this;
        var list = options.listData.map(function (item) {
            return self.buildCheckboxItem(item, {
                productId: item.Value
            });
        });
        var dropdown = $("#" + options.selectorStatusId);
        this.dropdownStatus = dropdown.fanexMultiSelect({
            selectAllText: dropdown.data("label-selectall"),
            selectedText: dropdown.data("label-selected"),
            noneSelectedText: dropdown.data("label-noneselected"),
            customSelectAllText: dropdown.data("label-all") + " (#)",
            showFilter: true,
            filterPlaceHolder: dropdown.data("label-filterplaceholder"),
            noItemFoundText: dropdown.data("label-noitemfound"),
            minWidth: 250,
            maxHeight: 250,
            showTooltip: false,
            listData: list,
            onLeave: function () {
                options.onDropdownHidden(self.getCheckedValues());
            }
        });
    };
    StatusDropdownComponent.prototype.buildCheckboxItem = function (item, attributes) {
        if (attributes === void 0) { attributes = undefined; }
        return {
            value: item.Value,
            text: item.Text,
            checked: item.Selected,
            attributes: attributes
        };
    };
    StatusDropdownComponent.prototype.getCheckedValues = function () {
        var ids = {
            StatusIds: this.dropdownStatus.fanexMultiSelect("getCheckedValues")
        };
        return ids;
    };
    StatusDropdownComponent.prototype.getUncheckedValues = function () {
        return {
            unselectedStatusIds: this.dropdownStatus.fanexMultiSelect("getUncheckedValues")
        };
    };
    StatusDropdownComponent.prototype.showHistoricProducts = function () {
        this.onlyShowHistoricStatusIds.length;
    };
    StatusDropdownComponent.prototype.hideHistoricProducts = function () {
        this.onlyShowHistoricStatusIds.length;
    };
    return StatusDropdownComponent;
}());
//# sourceMappingURL=status-dropdown.js.map