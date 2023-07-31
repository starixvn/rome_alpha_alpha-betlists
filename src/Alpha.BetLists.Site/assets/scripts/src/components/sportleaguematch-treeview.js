"use strict";
var SportLeagueMatchTreeViewOption = /** @class */ (function () {
    function SportLeagueMatchTreeViewOption(selectorId, noDataText, filterPlaceholder, filterNotFoundText, selectAllText, enableExpandAll, expandAllText, selectedOnlyText, expandGroupsByDefault, maxHeight) {
        if (selectorId === void 0) { selectorId = "sportleaguematch-treeview"; }
        if (noDataText === void 0) { noDataText = "No information is available"; }
        if (filterPlaceholder === void 0) { filterPlaceholder = "Search Sport/ League"; }
        if (filterNotFoundText === void 0) { filterNotFoundText = "No Sport/ League found"; }
        if (selectAllText === void 0) { selectAllText = "Select All"; }
        if (enableExpandAll === void 0) { enableExpandAll = false; }
        if (expandAllText === void 0) { expandAllText = "Expand All"; }
        if (selectedOnlyText === void 0) { selectedOnlyText = "Selected Only"; }
        if (expandGroupsByDefault === void 0) { expandGroupsByDefault = false; }
        if (maxHeight === void 0) { maxHeight = 400; }
        this.selectorId = selectorId;
        this.noDataText = noDataText;
        this.filterPlaceholder = filterPlaceholder;
        this.filterNotFoundText = filterNotFoundText;
        this.selectAllText = selectAllText;
        this.enableExpandAll = enableExpandAll;
        this.expandAllText = expandAllText;
        this.selectedOnlyText = selectedOnlyText;
        this.expandGroupsByDefault = expandGroupsByDefault;
        this.maxHeight = maxHeight;
        this.selectorId = selectorId;
        this.noDataText = noDataText;
        this.filterPlaceholder = filterPlaceholder;
        this.filterNotFoundText = filterNotFoundText;
        this.selectAllText = selectAllText;
        this.enableExpandAll = enableExpandAll;
        this.expandAllText = expandAllText;
        this.selectedOnlyText = selectedOnlyText;
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
        if (this.treeviewOption.enableExpandAll) {
            this.expandAll = selector.find("#expand-all-" + this.treeviewOption.selectorId);
            this.selectedOnly = selector.find("#selected-only-" + this.treeviewOption.selectorId);
        }
        this.checkboxListContainer = selector.find(".checkbox-list");
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
        html.push('<div class="selectall-box">', '<label class="node-label"><input type="checkbox" id="', selectAllId, '"/>', this.treeviewOption.selectAllText, '</label>');
        if (this.treeviewOption.enableExpandAll) {
            var collapseExpandAllId = "expand-all-" + this.treeviewOption.selectorId;
            var selectedOnlyId = "selected-only-" + this.treeviewOption.selectorId;
            html.push('<label class="node-label"><input type="checkbox" id="', collapseExpandAllId, '"/>', this.treeviewOption.expandAllText, '</label>', '<label class="node-label"><input type="checkbox" id="', selectedOnlyId, '"/>', this.treeviewOption.selectedOnlyText, '</label>');
        }
        html.push('</div>');
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
            var checked = _this.selectAll.is(":checked");
            _this.checkboxListContainer.find(":checkbox").prop({
                indeterminate: false,
                checked: checked
            });
        });
        if (this.treeviewOption.enableExpandAll) {
            this.expandAll.click(function () {
                var checked = _this.expandAll.is(":checked");
                _this.checkboxListContainer
                    .find(".icon-dropdown")
                    .each(function (_, item) { return _this.collapseExpand($(item), checked); });
            });
            this.selectedOnly.click(function () {
                if (_this.selectedOnly.is(":checked")) {
                    _this.checkboxListContainer
                        .find(":checkbox:not(:checked)")
                        .filter(function () { return !this.indeterminate; })
                        .closest(".tree-node")
                        .addClass("selected-only-hidden");
                }
                else {
                    _this.checkboxListContainer.find(".tree-node.selected-only-hidden").removeClass("selected-only-hidden");
                }
            });
        }
    };
    SportLeagueMatchTreeViewComponent.prototype.handleFiltering = function (filterValue) {
        var _this = this;
        if (filterValue === void 0) { filterValue = ""; }
        var notFound = true;
        var treeNodeList = this.checkboxListContainer.find(".tree-node");
        if (filterValue.length) {
            treeNodeList.each(function (_, item) {
                var treeNode = $(item);
                var text = treeNode.data("text");
                if (text.indexOf(filterValue) !== -1) {
                    _this.showParentTreeNode(treeNode);
                    treeNode.removeClass("filter-hidden");
                    notFound = false;
                }
                else {
                    treeNode.addClass("filter-hidden");
                }
            });
        }
        else {
            treeNodeList.removeClass("filter-hidden");
            notFound = false;
        }
        this.notFoundText.toggleClass("show", notFound);
        this.selectAllContainer.toggleClass("filter-hidden", notFound);
    };
    SportLeagueMatchTreeViewComponent.prototype.showParentTreeNode = function (treeNode) {
        treeNode.parents(".subtree").each(function (_, item) {
            $(item).parent().removeClass("filter-hidden");
        });
    };
    SportLeagueMatchTreeViewComponent.prototype.appendTreeNode = function (html, data, level) {
        var _this = this;
        var isHasChild = data.items && data.items.length > 0;
        html.push('<li class="tree-node" data-text="', data.text.toLowerCase(), '">');
        if (isHasChild) {
            html.push('<span class="icon-dropdown', this.treeviewOption.expandGroupsByDefault ? " expand" : "", '"></span>', this.renderCheckbox(data, isHasChild, level), '<ul class="subtree', this.treeviewOption.expandGroupsByDefault ? " expand" : "", '">');
            level++;
            data.items.forEach(function (item) { return html.push(_this.appendTreeNode(html, item, level)); });
            html.push("</ul>");
        }
        else {
            html.push(this.renderCheckbox(data, isHasChild, level));
        }
        html.push("</li>");
    };
    SportLeagueMatchTreeViewComponent.prototype.renderCheckbox = function (data, isHasChild, level) {
        return [
            '<label class="node-label"><input type="checkbox" value="',
            data.value,
            '"',
            isHasChild ? ' class="checkbox-subtree"' : "",
            ' data-level="',
            level,
            '"/>',
            data.text,
            data.subText ? "<span>" + data.subText + "</span>" : "",
            '</label>'
        ].join("");
    };
    SportLeagueMatchTreeViewComponent.prototype.bindEphemeralEvents = function () {
        var _this = this;
        this.checkboxListContainer.find(":checkbox").off().click(function (e) {
            _this.toggleCheckbox($(e.target));
            _this.updateSelectAll();
        });
        var iconDropdownList = this.checkboxListContainer.find(".icon-dropdown");
        iconDropdownList.off().click(function (e) {
            var icon = $(e.target);
            var isExpand = icon.hasClass("expand");
            _this.collapseExpand(icon, !isExpand);
            _this.treeviewOption.enableExpandAll &&
                _this.expandAll.prop("checked", !isExpand && iconDropdownList.filter(":not(.expand)").length === 0);
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
        var allCheckboxes = this.checkboxListContainer.find(":checkbox:not(.checkbox-subtree)");
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
        var checkboxes = [this.selectAll];
        this.treeviewOption.enableExpandAll && checkboxes.push(this.expandAll, this.selectedOnly);
        this.disableLabelAndCheckbox(checkboxes, disable);
    };
    SportLeagueMatchTreeViewComponent.prototype.disableLabelAndCheckbox = function (checkboxes, disable) {
        checkboxes.forEach(function (item) {
            item.prop("disabled", disable);
            if (disable) {
                item.parent().attr("disabled", "disabled");
            }
            else {
                item.parent().removeAttr("disabled");
            }
        });
    };
    SportLeagueMatchTreeViewComponent.prototype.addIconLoading = function () {
        this.checkboxListContainer.html('<li><span class="lds-spinner"><span></span><span></span><span></span><span></span><span></span><span></span><span></span><span></span><span></span></span></li>');
        this.disableFilerAndSelectAll(true);
    };
    SportLeagueMatchTreeViewComponent.prototype.renderTreeNodeList = function (listData) {
        var _this = this;
        var html = [];
        var hasListData = listData.length > 0;
        if (hasListData) {
            listData.forEach(function (item) { return _this.appendTreeNode(html, item, 0); });
        }
        else {
            html.push(this.renderNoData());
        }
        this.checkboxListContainer.html(html.join(""));
        if (hasListData) {
            this.bindEphemeralEvents();
            this.updateSelectAll();
        }
        this.disableFilerAndSelectAll(!hasListData);
    };
    SportLeagueMatchTreeViewComponent.prototype.getSelectedMembers = function () {
        var custIds = [];
        var usernames = [];
        this.checkboxListContainer.find(":checkbox:checked").each(function (_, item) {
            custIds.push(item.value);
            usernames.push($(item).parent(".node-label").text());
        });
        return { custIds: custIds, usernames: usernames };
    };
    SportLeagueMatchTreeViewComponent.prototype.isSelectAll = function () {
        return this.selectAll.is(":checked");
    };
    SportLeagueMatchTreeViewComponent.prototype.getSportsLeagues = function () {
        var sportsAllLeagues = [];
        var sports = [];
        this.checkboxListContainer.find(":checkbox[data-level=0]").each(function (_, sportElement) {
            if (sportElement.checked) {
                sportsAllLeagues.push(sportElement.value);
            }
            else if (sportElement.indeterminate) {
                $(sportElement).parent().next(".subtree").find(":checkbox:checked").each(function (_, leagueElement) {
                    sports.push({ sport: sportElement.value, league: leagueElement.value });
                });
            }
        });
        sportsAllLeagues.length && sports.push({ sport: sportsAllLeagues.join(), league: "0" });
        return sports;
    };
    return SportLeagueMatchTreeViewComponent;
}());
//# sourceMappingURL=sportleaguematch-treeview.js.map