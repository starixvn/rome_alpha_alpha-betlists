"use strict";
var SimpleTreeViewComponent = /** @class */ (function () {
    function SimpleTreeViewComponent() {
    }
    SimpleTreeViewComponent.prototype.render = function (selectorId) {
        if (selectorId === void 0) { selectorId = "simple-treeview"; }
        var selector = $("#" + selectorId);
        this.noDataText = selector.find("#not-found-" + selectorId);
        this.selectAllContainer = selector.find(".selectall-box");
        this.selectAll = this.selectAllContainer.find("#select-all-" + selectorId);
        this.checkboxList = selector.find(".checkbox-list :checkbox");
        this.registerEvents(selector, selectorId);
    };
    SimpleTreeViewComponent.prototype.registerEvents = function (selector, selectorId) {
        var _this = this;
        this.selectAll.click(function () { return _this.checkboxList.prop("checked", _this.selectAll.is(":checked")); });
        this.checkboxList.click(function (e) {
            var allCheckboxesLength = _this.checkboxList.length;
            var checkedCheckboxesLength = _this.checkboxList.filter(":checked").length;
            var isSelectAll = checkedCheckboxesLength > 0 && checkedCheckboxesLength === allCheckboxesLength;
            _this.selectAll.prop({
                indeterminate: !isSelectAll && checkedCheckboxesLength > 0,
                checked: isSelectAll
            });
        });
        this.registerFilterEvent(selector, selectorId);
    };
    SimpleTreeViewComponent.prototype.registerFilterEvent = function (selector, selectorId) {
        var _this = this;
        var filterContainer = selector.find(".filter-box");
        var filterInput = filterContainer.find("#filter-input-" + selectorId);
        var noDataText = filterContainer.find("#not-found-" + selectorId);
        var selectAllContainer = this.selectAll.closest(".select-all-box");
        var handleFilterNotFound = function (notFound) {
            if (notFound) {
                noDataText.addClass("show");
                selectAllContainer.addClass("filter-hidden");
            }
            else {
                noDataText.removeClass("show");
                selectAllContainer.removeClass("filter-hidden");
            }
        };
        filterInput.keyup(function (e) {
            e.preventDefault();
            e.which === 13 && _this.handleFilterHideItems(filterInput.val().toLowerCase());
        });
        filterContainer.find("#icon-delete-" + selectorId).click(function () {
            if (filterInput.val().length) {
                filterInput.val("");
                _this.handleFilterHideItems();
            }
        });
    };
    SimpleTreeViewComponent.prototype.handleFilterHideItems = function (filterValue) {
        if (filterValue === void 0) { filterValue = ""; }
        var notFound = true;
        if (filterValue.length) {
            this.checkboxList.each(function (_, item) {
                var checkbox = $(item);
                var treeNode = checkbox.closest(".tree-node");
                var text = checkbox.data("text");
                if (text.indexOf(filterValue) !== -1) {
                    checkbox.removeClass("filter-hidden");
                    treeNode.removeClass("filter-hidden");
                    notFound = false;
                }
                else {
                    checkbox.addClass("filter-hidden");
                    treeNode.addClass("filter-hidden");
                }
            });
        }
        else {
            this.checkboxList.each(function (_, item) {
                var checkbox = $(item).removeClass("filter-hidden");
                var treeNode = checkbox.closest(".tree-node").removeClass("filter-hidden");
            });
            notFound = false;
        }
        this.noDataText.toggleClass("show", notFound);
        this.selectAllContainer.toggleClass("filter-hidden", notFound);
    };
    SimpleTreeViewComponent.prototype.getSelected = function () {
        var selected = [];
        this.checkboxList.filter(":checked").each(function (_, item) {
            selected.push(item.value);
        });
        return selected;
    };
    SimpleTreeViewComponent.prototype.isSelectAll = function () {
        return this.selectAll.is(":checked");
    };
    return SimpleTreeViewComponent;
}());
//# sourceMappingURL=simple-treeview.js.map