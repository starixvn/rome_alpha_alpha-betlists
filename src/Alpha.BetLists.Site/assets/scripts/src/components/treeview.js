"use strict";
var TreeViewOption = /** @class */ (function () {
    function TreeViewOption(selectorId, listData, filterPlaceholder, filterNotFoundText, filterBehavior, selectAllText, expandGroupsByDefault, maxHeight) {
        if (selectorId === void 0) { selectorId = "treeview"; }
        if (listData === void 0) { listData = []; }
        if (filterPlaceholder === void 0) { filterPlaceholder = "Search..."; }
        if (filterNotFoundText === void 0) { filterNotFoundText = "No item found"; }
        if (filterBehavior === void 0) { filterBehavior = "hide"; }
        if (selectAllText === void 0) { selectAllText = "Select All"; }
        if (expandGroupsByDefault === void 0) { expandGroupsByDefault = true; }
        if (maxHeight === void 0) { maxHeight = 400; }
        this.selectorId = selectorId;
        this.listData = listData;
        this.filterPlaceholder = filterPlaceholder;
        this.filterNotFoundText = filterNotFoundText;
        this.filterBehavior = filterBehavior;
        this.selectAllText = selectAllText;
        this.expandGroupsByDefault = expandGroupsByDefault;
        this.maxHeight = maxHeight;
        this.selectorId = selectorId;
        this.listData = listData;
        this.filterPlaceholder = filterPlaceholder;
        this.filterNotFoundText = filterNotFoundText;
        this.selectAllText = selectAllText;
        this.expandGroupsByDefault = expandGroupsByDefault;
        this.maxHeight = maxHeight;
    }
    return TreeViewOption;
}());
var TreeViewComponent = /** @class */ (function () {
    function TreeViewComponent() {
    }
    TreeViewComponent.prototype.render = function (option) {
        this.treeviewOption = option;
        var selector = $("#" + option.selectorId).addClass("treeview");
        var listDataLength = this.treeviewOption.listData.length;
        listDataLength && selector.html(this.renderTreeview());
        this.noDataText = selector.find("#not-found-" + this.treeviewOption.selectorId);
        this.selectAllContainer = selector.find(".selectall-box");
        this.selectAll = this.selectAllContainer.find("#select-all-" + option.selectorId);
        this.checkboxList = selector.find(".checkbox-list :checkbox");
        this.registerEvents(selector);
        listDataLength && this.updateSelectAll();
    };
    TreeViewComponent.prototype.renderTreeview = function () {
        var _this = this;
        var html = [];
        this.appendFilter(html);
        this.appendSelectAll(html);
        // render checkbox list
        html.push('<ul class="checkbox-list" style="max-height:', this.treeviewOption.maxHeight, 'px">');
        this.treeviewOption.listData.forEach(function (item) { return html.push(_this.appendTreeNode(html, item)); });
        html.push("</ul>");
        return html.join("");
    };
    TreeViewComponent.prototype.appendFilter = function (html) {
        var filterId = "filter-input-" + this.treeviewOption.selectorId;
        var iconDeleteId = "icon-delete-" + this.treeviewOption.selectorId;
        var noDataTextId = "not-found-" + this.treeviewOption.selectorId;
        html.push('<div class="filter-box">', '<input type="text" id="', filterId, '" placeholder="', this.treeviewOption.filterPlaceholder, '" autocomplete="off"/><span class="icon-close" id="', iconDeleteId, '"></span><div id="', noDataTextId, '" class="not-found">', this.treeviewOption.filterNotFoundText, "</div></div>");
    };
    TreeViewComponent.prototype.appendSelectAll = function (html) {
        var selectAllId = "select-all-" + this.treeviewOption.selectorId;
        var collapseExpandAllId = "icon-dropdown-all-" + this.treeviewOption.selectorId;
        html.push('<div class="selectall-box">');
        if (this.enableCollapseExpandAll()) {
            html.push('<span id="', collapseExpandAllId, '" class="icon-dropdown-all', this.treeviewOption.expandGroupsByDefault ? " expand" : "", '"></span>');
        }
        html.push('<label class="node-label"><input type="checkbox" id="', selectAllId, '"/>', this.treeviewOption.selectAllText, "</label></div>");
    };
    TreeViewComponent.prototype.enableCollapseExpandAll = function () {
        var listData = this.treeviewOption.listData;
        for (var i = 0, max = listData.length; i < max; i++) {
            if (listData[i].items && listData[i].items.length > 0) {
                return true;
            }
        }
        return false;
    };
    TreeViewComponent.prototype.appendTreeNode = function (html, data) {
        var _this = this;
        var value = data.value || "";
        if (value) {
            var isHasChild = data.items && data.items.length > 0;
            html.push('<li class="tree-node">');
            if (isHasChild) {
                html.push('<span class="icon-dropdown', this.treeviewOption.expandGroupsByDefault ? " expand" : "", '"></span>', this.renderCheckbox(data, isHasChild), '<ul class="subtree', this.treeviewOption.expandGroupsByDefault ? " expand" : "", '">');
                data.items.forEach(function (item) { return html.push(_this.appendTreeNode(html, item)); });
                html.push("</ul>");
            }
            else {
                html.push(this.renderCheckbox(data, isHasChild));
            }
            html.push("</li>");
        }
    };
    TreeViewComponent.prototype.renderCheckbox = function (data, isHasChild) {
        var html = [];
        var title = data.text || "";
        html.push('<label class="node-label"><input type="checkbox" data-text="', title.toLowerCase(), '"', isHasChild ? ' class="checkbox-subtree"' : "", data.checked ? " checked" : "");
        html.push("/>", title, "</label>");
        return html.join("");
    };
    TreeViewComponent.prototype.registerEvents = function (selector) {
        var _this = this;
        this.selectAll.click(function () {
            _this.toggleSelectAll();
            _this.updateSelectAll();
        });
        this.checkboxList.click(function (e) {
            _this.toggleCheckbox($(e.target));
            _this.updateSelectAll();
        });
        this.registerFilterEvent(selector);
        this.registerCollapseExpandEvents(selector);
    };
    TreeViewComponent.prototype.registerFilterEvent = function (selector) {
        var filterContainer = selector.find(".filter-box");
        var filterInput = filterContainer.find("#filter-input-" + this.treeviewOption.selectorId);
        var noDataText = filterContainer.find("#not-found-" + this.treeviewOption.selectorId);
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
        var handleFilter = this.treeviewOption.filterBehavior === "hide"
            ? this.handleFilterHideItems.bind(this)
            : this.handleFilterHighlightItems.bind(this);
        filterInput.keyup(function (e) {
            e.preventDefault();
            e.which === 13 && handleFilter(filterInput.val().toLowerCase());
        });
        filterContainer.find("#icon-delete-" + this.treeviewOption.selectorId).click(function () {
            if (filterInput.val().length) {
                filterInput.val("");
                handleFilter();
            }
        });
    };
    TreeViewComponent.prototype.handleFilterHideItems = function (filterValue) {
        var _this = this;
        if (filterValue === void 0) { filterValue = ""; }
        var notFound = true;
        if (filterValue.length) {
            this.checkboxList.each(function (_, item) {
                var checkbox = $(item);
                var treeNode = checkbox.closest(".tree-node");
                var text = checkbox.data("text");
                if (text.indexOf(filterValue) !== -1) {
                    _this.showParentTreeNode(checkbox);
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
        this.updateSelectAll();
    };
    TreeViewComponent.prototype.showParentTreeNode = function (checkbox) {
        checkbox.parents(".subtree").each(function (_, item) {
            var subtree = $(item);
            subtree.prev().find(":checkbox").removeClass("filter-hidden");
            subtree.parent().removeClass("filter-hidden");
        });
    };
    TreeViewComponent.prototype.handleFilterHighlightItems = function (filterValue) {
        if (filterValue === void 0) { filterValue = ""; }
        if (filterValue.length) {
            this.checkboxList.each(function (_, item) {
                var checkbox = $(item);
                var text = checkbox.data("text");
                checkbox.parent().toggleClass("highlight-text", text.indexOf(filterValue) !== -1);
            });
        }
        else {
            this.checkboxList.each(function (_, item) {
                var checkbox = $(item);
                checkbox.parent().removeClass("highlight-text");
            });
        }
    };
    TreeViewComponent.prototype.registerCollapseExpandEvents = function (selector) {
        var _this = this;
        var iconDropdownAll = selector.find("#icon-dropdown-all-" + this.treeviewOption.selectorId);
        var iconDropdownList = selector.find(".checkbox-list .icon-dropdown");
        iconDropdownAll.click(function (e) {
            var icon = $(e.target);
            var isExpand = icon.hasClass("expand");
            iconDropdownList.each(function (_, item) { return _this.collapseExpand($(item), !isExpand); });
            icon.toggleClass("expand", !isExpand);
        });
        iconDropdownList.click(function (e) {
            var icon = $(e.target);
            var isExpand = icon.hasClass("expand");
            _this.collapseExpand(icon, !isExpand);
            //if (isExpand) {
            //    if (!iconDropdownList.filter(":not(.expand)").length) {
            //        iconDropdownAll.addClass("expand");
            //    }
            //} else {
            //    if (!iconDropdownList.filter(".expand").length) {
            //        iconDropdownAll.removeClass("expand");
            //    }
            //}
        });
    };
    TreeViewComponent.prototype.collapseExpand = function (icon, isExpand) {
        icon.next().next().toggleClass("expand", isExpand);
        icon.toggleClass("expand", isExpand);
    };
    TreeViewComponent.prototype.toggleSelectAll = function () {
        var checked = this.selectAll.is(":checked");
        this.checkboxList.filter(":not(.filter-hidden)").each(function (_, checkbox) {
            checkbox.checked = checked;
            // const $cb = checkbox;
            // $cb.hasClass('checkbox-subtree') && this._toggleCheckbox($cb);
        });
    };
    TreeViewComponent.prototype.toggleCheckbox = function (checkbox) {
        var _this = this;
        var checked = checkbox.is(":checked");
        // check/uncheck children
        checkbox.hasClass("checkbox-subtree") &&
            checkbox.parent().next(".subtree").find(":checkbox").prop("checked", checked);
        // check/uncheck parents
        checkbox.parents(".subtree").each(function (_, item) {
            var subtree = $(item);
            var subtreeCheckbox = subtree.prev().find(":checkbox");
            var subtreeCheckboxes = subtree.find(":checkbox");
            _this.updateCheckboxStatus(subtreeCheckbox, subtreeCheckboxes);
        });
    };
    TreeViewComponent.prototype.updateSelectAll = function () {
        var allCheckboxes = this.checkboxList.filter(":not(.filter-hidden,.checkbox-subtree)");
        this.updateCheckboxStatus(this.selectAll, allCheckboxes);
    };
    TreeViewComponent.prototype.updateCheckboxStatus = function (checkbox, allCheckboxes) {
        var allCheckboxesLength = allCheckboxes.length;
        var checkedCheckboxesLength = allCheckboxes.filter(":checked").length;
        var isSelectAll = checkedCheckboxesLength > 0 && checkedCheckboxesLength === allCheckboxesLength;
        checkbox.prop({
            indeterminate: !isSelectAll && checkedCheckboxesLength > 0,
            checked: isSelectAll
        });
    };
    TreeViewComponent.prototype.getSelected = function () {
        var selected = [];
        this.checkboxList.filter(":checked").each(function (_, item) {
            selected.push(item.value);
        });
        return selected;
    };
    TreeViewComponent.prototype.isSelectAll = function () {
        return this.selectAll.is(":checked");
    };
    return TreeViewComponent;
}());
//# sourceMappingURL=treeview.js.map