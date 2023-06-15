"use strict";
var VisibleColumnDropdownOption = /** @class */ (function () {
    function VisibleColumnDropdownOption(visibleColumnKey, table, selectorId) {
        if (selectorId === void 0) { selectorId = "dropdown-column"; }
        this.visibleColumnKey = visibleColumnKey;
        this.table = table;
        this.selectorId = selectorId;
        this.visibleColumnKey = visibleColumnKey;
        this.table = table;
        this.selectorId = selectorId;
    }
    return VisibleColumnDropdownOption;
}());
var VisibleColumnDropdownComponent = /** @class */ (function () {
    function VisibleColumnDropdownComponent() {
    }
    VisibleColumnDropdownComponent.prototype.render = function (options) {
        var _this = this;
        var isCheckAll = false;
        var visibleColumnObject = {};
        var visibleColumns = alphaReports.localStorage.get(options.visibleColumnKey);
        if (typeof visibleColumns !== "undefined") {
            var visibleColumnArray = visibleColumns.split(",");
            visibleColumnArray.forEach(function (item) {
                visibleColumnObject[item] = true;
            });
        }
        else {
            isCheckAll = true;
        }
        var columnList = [];
        options.table.find("thead tr th[data-column]").each(function () {
            var $this = $(this);
            var column = $this.data("column");
            columnList.push({
                value: column,
                text: $this.text(),
                checked: isCheckAll || visibleColumnObject.hasOwnProperty(column)
            });
        });
        var dropdown = $("#" + options.selectorId);
        this.dropdownColumn = dropdown.fanexMultiSelect({
            checkAllText: dropdown.data("label-selectall"),
            selectedText: "# " + dropdown.data("label-selected"),
            noneSelectedText: dropdown.data("label-noneselected"),
            useCustomCheckAllText: true,
            customCheckAllText: dropdown.data("label-all") + " (#)",
            minWidth: 220,
            showTooltip: false,
            listData: columnList,
            onLeave: function () { return _this.onDropdownHidden(options.visibleColumnKey, options.table); }
        });
        this.onDropdownHidden(options.visibleColumnKey, options.table);
    };
    VisibleColumnDropdownComponent.prototype.onDropdownHidden = function (visibleColumnKey, table) {
        var selectedColumnsValue = [];
        var columns = this.getColumns();
        for (var _i = 0, _a = Object.entries(columns); _i < _a.length; _i++) {
            var _b = _a[_i], key = _b[0], value = _b[1];
            if (value) {
                selectedColumnsValue.push(key);
                table.find("[data-column=\"" + key + "\"]").show();
            }
            else {
                table.find("[data-column=\"" + key + "\"]").hide();
            }
        }
        alphaReports.localStorage.set(visibleColumnKey, selectedColumnsValue.join());
    };
    VisibleColumnDropdownComponent.prototype.getColumns = function () {
        return this.dropdownColumn.fanexMultiSelect("getAllValues");
    };
    return VisibleColumnDropdownComponent;
}());
//# sourceMappingURL=visible-columns-dropdown.js.map