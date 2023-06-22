"use strict";
var DropdownListComponent = /** @class */ (function () {
    function DropdownListComponent() {
    }
    DropdownListComponent.prototype.render = function (selectorId, options) {
        if (options === void 0) { options = {}; }
        this.dropdownList = $("#" + selectorId);
        var multiple = this.dropdownList.attr("multiple");
        var defaults = { maxHeight: 300 };
        if (multiple) {
            defaults = $.extend(defaults, {
                numberDisplayed: 1,
                nonSelectedText: this.dropdownList.data("label-select"),
                nSelectedText: " " + this.dropdownList.data("label-selected"),
                allSelectedText: this.dropdownList.data("label-all"),
                includeSelectAllOption: true
            });
        }
        this.dropdownList.multiselect($.extend(defaults, options));
        return this.dropdownList;
    };
    ;
    DropdownListComponent.prototype.getSelected = function () {
        var selected = [];
        this.dropdownList.find("option:selected").each(function (_, item) {
            selected.push(item.value);
        });
        return selected;
    };
    DropdownListComponent.prototype.isSelectAll = function () {
        return this.dropdownList.next().find(".multiselect-container .multiselect-all :checkbox").is(":checked");
    };
    return DropdownListComponent;
}());
//# sourceMappingURL=dropdown-list.js.map