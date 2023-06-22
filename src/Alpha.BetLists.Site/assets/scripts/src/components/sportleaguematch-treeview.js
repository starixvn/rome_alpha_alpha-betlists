"use strict";
var SportLeagueMatchTreeViewOption = /** @class */ (function () {
    function SportLeagueMatchTreeViewOption(selectorId, filterPlaceholder, filterNotFoundText, selectAllText, noDataText, expandGroupsByDefault, maxHeight) {
        if (selectorId === void 0) { selectorId = "sportleaguematch-treeview"; }
        if (filterPlaceholder === void 0) { filterPlaceholder = "Search Sport/ League/ Match"; }
        if (filterNotFoundText === void 0) { filterNotFoundText = "No Sport/ League/ Match found"; }
        if (selectAllText === void 0) { selectAllText = "Select All"; }
        if (noDataText === void 0) { noDataText = "No information is available"; }
        if (expandGroupsByDefault === void 0) { expandGroupsByDefault = true; }
        if (maxHeight === void 0) { maxHeight = 400; }
        this.selectorId = selectorId;
        this.filterPlaceholder = filterPlaceholder;
        this.filterNotFoundText = filterNotFoundText;
        this.selectAllText = selectAllText;
        this.noDataText = noDataText;
        this.expandGroupsByDefault = expandGroupsByDefault;
        this.maxHeight = maxHeight;
        this.selectorId = selectorId;
        this.filterPlaceholder = filterPlaceholder;
        this.filterNotFoundText = filterNotFoundText;
        this.selectAllText = selectAllText;
        this.noDataText = noDataText;
        this.expandGroupsByDefault = expandGroupsByDefault;
        this.maxHeight = maxHeight;
    }
    return SportLeagueMatchTreeViewOption;
}());
var SportLeagueMatchTreeViewComponent = /** @class */ (function () {
    function SportLeagueMatchTreeViewComponent() {
    }
    SportLeagueMatchTreeViewComponent.prototype.render = function (option) {
        this.treeviewOption = option;
        var selector = $("#" + this.treeviewOption.selectorId)
            .addClass("treeview")
            .html(this.renderTreeviewNoData());
        this.filterInput = selector.find("#filter-input-" + this.treeviewOption.selectorId);
        this.notFoundText = selector.find("#not-found-" + this.treeviewOption.selectorId);
        this.selectAllContainer = selector.find(".selectall-box");
        this.selectAll = selector.find("#select-all-" + this.treeviewOption.selectorId);
        this.checkboxListContainer = selector.find(".checkbox-list");
        this.checkboxList = this.checkboxListContainer.find(":checkbox");
        this.bindPersistentEvents(selector);
        this.disableFilerAndSelectAll(true);
    };
    SportLeagueMatchTreeViewComponent.prototype.renderTreeviewNoData = function () {
        var html = [];
        this.appendFilter(html);
        this.appendSelectAll(html);
        html.push('<ul class="checkbox-list" style="max-height:', this.treeviewOption.maxHeight, 'px">', this.renderNoData(), '</ul>');
        return html.join("");
    };
    SportLeagueMatchTreeViewComponent.prototype.appendFilter = function (html) {
        var filterId = "filter-input-" + this.treeviewOption.selectorId;
        var iconDeleteId = "icon-delete-" + this.treeviewOption.selectorId;
        var notFoundTextId = "not-found-" + this.treeviewOption.selectorId;
        html.push('<div class="filter-box">', '<input type="text" id="', filterId, '" placeholder="', this.treeviewOption.filterPlaceholder, '" autocomplete="off"/><span class="icon-close" id="', iconDeleteId, '"></span><div id="', notFoundTextId, '" class="not-found">', this.treeviewOption.filterNotFoundText, '</div></div>');
    };
    SportLeagueMatchTreeViewComponent.prototype.appendSelectAll = function (html) {
        var selectAllId = "select-all-" + this.treeviewOption.selectorId;
        var collapseExpandAllId = "icon-dropdown-all-" + this.treeviewOption.selectorId;
        html.push('<div class="selectall-box">');
        if (this.enableCollapseExpandAll()) {
            html.push('<span id="', collapseExpandAllId, '" class="icon-dropdown-all', this.treeviewOption.expandGroupsByDefault ? " expand" : "", '"></span>');
        }
        html.push('<label class="node-label"><input type="checkbox" id="', selectAllId, '"/>', this.treeviewOption.selectAllText, '</label></div>');
    };
    SportLeagueMatchTreeViewComponent.prototype.enableCollapseExpandAll = function () {
        //const listData = this.treeviewOption.listData;
        //for (let i = 0, max = listData.length; i < max; i++) {
        //    if (listData[i].items && listData[i].items.length > 0) {
        //        return true;
        //    }
        //}
        return false;
    };
    SportLeagueMatchTreeViewComponent.prototype.renderNoData = function () {
        return '<li><label class="node-label">' + this.treeviewOption.noDataText + '</label></li>';
    };
    SportLeagueMatchTreeViewComponent.prototype.bindPersistentEvents = function (selector) {
        var _this = this;
        this.filterInput.keyup(function (e) {
            e.preventDefault();
            e.which === 13 && _this.handleFiltering(_this.filterInput.val().toLowerCase());
        });
        selector.find("#icon-delete-" + this.treeviewOption.selectorId).click(function () {
            if (_this.filterInput.val().length) {
                _this.filterInput.val("");
                _this.handleFiltering();
            }
        });
        this.selectAll.click(function () {
            _this.checkboxList.prop("checked", _this.selectAll.is(":checked"));
        });
    };
    SportLeagueMatchTreeViewComponent.prototype.handleFiltering = function (filterValue) {
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
                    treeNode.removeClass("filter-hidden");
                    notFound = false;
                }
                else {
                    treeNode.addClass("filter-hidden");
                }
            });
        }
        else {
            this.checkboxList.each(function (_, item) {
                $(item).closest(".tree-node").removeClass("filter-hidden");
            });
            notFound = false;
        }
        this.notFoundText.toggleClass("show", notFound);
        this.selectAllContainer.toggleClass("filter-hidden", notFound);
    };
    SportLeagueMatchTreeViewComponent.prototype.showParentTreeNode = function (checkbox) {
        checkbox.parents(".subtree").each(function (_, item) {
            var subtree = $(item);
            //subtree.prev().find(":checkbox").removeClass("filter-hidden");
            subtree.parent().removeClass("filter-hidden");
        });
    };
    SportLeagueMatchTreeViewComponent.prototype.appendTreeNode = function (html, data) {
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
    SportLeagueMatchTreeViewComponent.prototype.renderCheckbox = function (data, isHasChild) {
        var title = data.text || "";
        var attributes = data.attributes || {};
        var html = [
            '<label class="node-label"><input type="checkbox" data-text="',
            title.toLowerCase(),
            '" value="',
            data.value,
            '"',
            isHasChild ? ' class="checkbox-subtree"' : "",
            data.checked ? " checked" : ""
        ];
        Object.keys(attributes).forEach(function (key) {
            html.push(' ', key, '="', attributes[key], '"');
        });
        html.push('/><span>', title, '</span>');
        data.subText && html.push('<span>', data.subText, '</span>');
        html.push('</label>');
        return html.join("");
    };
    SportLeagueMatchTreeViewComponent.prototype.bindEphemeralEvents = function () {
        var _this = this;
        this.checkboxList.off().click(function (e) {
            _this.toggleCheckbox($(e.target));
            _this.updateSelectAll();
        });
        var iconDropdownAll = this.selectAllContainer.find("#icon-dropdown-all-" + this.treeviewOption.selectorId);
        var iconDropdownList = this.checkboxListContainer.find(".icon-dropdown");
        iconDropdownAll.off().click(function (e) {
            var icon = $(e.target);
            var isExpand = icon.hasClass("expand");
            iconDropdownList.each(function (_, item) { return _this.collapseExpand($(item), !isExpand); });
            icon.toggleClass("expand", !isExpand);
        });
        iconDropdownList.off().click(function (e) {
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
    SportLeagueMatchTreeViewComponent.prototype.collapseExpand = function (icon, isExpand) {
        icon.next().next().toggleClass("expand", isExpand);
        icon.toggleClass("expand", isExpand);
    };
    SportLeagueMatchTreeViewComponent.prototype.toggleCheckbox = function (checkbox) {
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
    SportLeagueMatchTreeViewComponent.prototype.updateSelectAll = function () {
        var allCheckboxes = this.checkboxList.filter(":not(.filter-hidden,.checkbox-subtree)");
        this.updateCheckboxStatus(this.selectAll, allCheckboxes);
    };
    SportLeagueMatchTreeViewComponent.prototype.updateCheckboxStatus = function (checkbox, allCheckboxes) {
        var allCheckboxesLength = allCheckboxes.length;
        var checkedCheckboxesLength = allCheckboxes.filter(":checked").length;
        var isSelectAll = checkedCheckboxesLength > 0 && checkedCheckboxesLength === allCheckboxesLength;
        checkbox.prop({
            indeterminate: !isSelectAll && checkedCheckboxesLength > 0,
            checked: isSelectAll
        });
    };
    SportLeagueMatchTreeViewComponent.prototype.disableFilerAndSelectAll = function (disable) {
        this.filterInput.prop("disabled", disable);
        this.selectAll.prop("disabled", disable);
        var selectAllLabel = this.selectAll.parent();
        if (disable) {
            selectAllLabel.attr("disabled", "disabled");
        }
        else {
            selectAllLabel.removeAttr("disabled");
        }
    };
    SportLeagueMatchTreeViewComponent.prototype.addIconLoading = function () {
        this.checkboxListContainer.html('<li><span class="icon-loading"></span></li>');
    };
    SportLeagueMatchTreeViewComponent.prototype.renderTreeNodeList = function (listData) {
        var _this = this;
        var html = [];
        var hasListData = listData.length > 0;
        if (hasListData) {
            listData.forEach(function (item) { return _this.appendTreeNode(html, item); });
        }
        else {
            html.push(this.renderNoData());
        }
        this.checkboxListContainer.html(html.join(""));
        this.checkboxList = this.checkboxListContainer.find(":checkbox");
        if (hasListData) {
            this.bindEphemeralEvents();
            this.updateSelectAll();
        }
        this.disableFilerAndSelectAll(!hasListData);
    };
    SportLeagueMatchTreeViewComponent.prototype.getSelected = function () {
        var selected = [];
        this.checkboxList.filter(":checked").each(function (_, item) {
            selected.push(item.value);
        });
        return selected;
    };
    SportLeagueMatchTreeViewComponent.prototype.isSelectAll = function () {
        return this.selectAll.is(":checked");
    };
    return SportLeagueMatchTreeViewComponent;
}());
//# sourceMappingURL=sportleaguematch-treeview.js.map