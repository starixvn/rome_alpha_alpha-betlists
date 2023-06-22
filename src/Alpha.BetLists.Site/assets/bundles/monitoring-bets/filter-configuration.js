"use strict";
var InlineMessageComponent = /** @class */ (function () {
    function InlineMessageComponent() {
        this.inlineMessage = $("#inline-message");
    }
    InlineMessageComponent.prototype.show = function (result) {
        this.inlineMessage
            .html(result.ErrMsg)
            .addClass(result.ErrCode ? "msg-error" : "msg-success")
            .removeClass("hide");
    };
    InlineMessageComponent.prototype.showError = function (message) {
        this.show({ ErrCode: 1, ErrMsg: message });
    };
    InlineMessageComponent.prototype.showSuccess = function (message) {
        this.show({ ErrCode: 0, ErrMsg: message });
    };
    InlineMessageComponent.prototype.hide = function () {
        this.inlineMessage.addClass("hide").html("");
    };
    return InlineMessageComponent;
}());
//# sourceMappingURL=inline-message.js.map
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
"use strict";
var UsernameTagsInputOption = /** @class */ (function () {
    function UsernameTagsInputOption(selectorId, limit, limitText, duplicatedUsernameText, width, height, delimiter) {
        if (selectorId === void 0) { selectorId = "tagsinput"; }
        if (limit === void 0) { limit = 100; }
        if (limitText === void 0) { limitText = "Max: # tag(s)"; }
        if (duplicatedUsernameText === void 0) { duplicatedUsernameText = "Duplicated username"; }
        if (width === void 0) { width = "auto"; }
        if (height === void 0) { height = "auto"; }
        if (delimiter === void 0) { delimiter = [","]; }
        this.selectorId = selectorId;
        this.limit = limit;
        this.limitText = limitText;
        this.duplicatedUsernameText = duplicatedUsernameText;
        this.width = width;
        this.height = height;
        this.delimiter = delimiter;
        this.selectorId = selectorId;
        this.limit = limit;
        this.limitText = limitText;
        this.duplicatedUsernameText = duplicatedUsernameText;
        this.width = width;
        this.height = height;
        this.delimiter = delimiter;
    }
    return UsernameTagsInputOption;
}());
var UsernameTagsInputComponent = /** @class */ (function () {
    function UsernameTagsInputComponent() {
    }
    UsernameTagsInputComponent.prototype.render = function (option) {
        this.tagsInputOption = option;
        this.cacheDOMElements();
        this.bindEvents();
    };
    UsernameTagsInputComponent.prototype.cacheDOMElements = function () {
        this.tagsInputContainer = $("#" + this.tagsInputOption.selectorId + "_tagsinput");
        this.tagLimit = this.tagsInputContainer.find("#" + this.tagsInputOption.selectorId + "_taglimit");
        this.realInput = this.tagsInputContainer.find("#" + this.tagsInputOption.selectorId + "_input");
        this.tagsInputContainer.css({
            width: this.tagsInputOption.width,
            "min-height": this.tagsInputOption.height
        });
        this.tagLimit.text(this.tagsInputOption.limitText.replace("#", this.tagsInputOption.limit.toString()));
    };
    UsernameTagsInputComponent.prototype.bindEvents = function () {
        var _this = this;
        this.realInput.data("pasted", false);
        this.realInput.on("paste", function () { return _this.realInput.data("pasted", true); });
        // If a user pastes the text check if it shouldn't be splitted into tags
        this.realInput.on("input", function () {
            if (!_this.realInput.data("pasted")) {
                return;
            }
            _this.realInput.data("pasted", false);
            var value = $.trim(_this.realInput.val());
            var tagsList = _this.splitIntoTags(value);
            tagsList.length > 1 && _this.addTagsList(tagsList);
        });
        this.realInput.on("focus", function () { return _this.tagsInputContainer.addClass("focus"); });
        this.realInput.blur(function () {
            _this.tagsInputContainer.removeClass("focus");
            _this.addTag(_this.realInput.val());
        });
        // If a user types a delimiter create a new tag
        this.realInput.keypress(function (event) {
            if (_this.checkDelimiter(event)) {
                event.preventDefault();
                _this.addTag(_this.realInput.val());
            }
        });
    };
    UsernameTagsInputComponent.prototype.addTag = function (value) {
        value = $.trim(value);
        this.validateTag(value) && this.addTagsList([value]);
    };
    UsernameTagsInputComponent.prototype.addTagsList = function (tagsList) {
        var _this = this;
        var usernames = this.getUsernames();
        var usernamesLength = usernames.length;
        if (this.tagsInputOption.limit > usernamesLength) {
            tagsList = tagsList.slice(0, this.tagsInputOption.limit - usernamesLength);
            var uniqueTagsList_1 = [];
            tagsList.forEach(function (item) {
                if (_this.tagExist(usernames, item)) {
                    _this.appendTags([{ Username: item, ErrCode: 1, ErrMsg: _this.tagsInputOption.duplicatedUsernameText }]);
                }
                else {
                    uniqueTagsList_1.push(item);
                }
            });
            $.post("CheckUsernames", { usernames: uniqueTagsList_1.join() }, function (results) { return _this.appendTags(results); });
        }
    };
    UsernameTagsInputComponent.prototype.tagExist = function (tagsList, value) {
        var valueLowerCase = value.toLowerCase();
        return tagsList.find(function (tag) { return tag.toLowerCase() === valueLowerCase; });
    };
    UsernameTagsInputComponent.prototype.appendTags = function (results) {
        var _this = this;
        results.forEach(function (result) {
            _this.tagsInputContainer.append($("<span>", {
                class: "tag" + (result.ErrCode ? " error" : ""),
                title: result.ErrMsg,
                "data-custid": result.CustId
            }).append($("<span>", { class: "tag-text" }).text(result.Username), $("<button>", { type: "button", class: "tag-remove" }).click(function (e) {
                $(e.target).parent().remove();
                _this.updateTagLimitText();
            })));
        });
        this.updateTagLimitText();
        this.realInput.val("").focus();
    };
    UsernameTagsInputComponent.prototype.updateTagLimitText = function (tagsListLength) {
        if (tagsListLength === void 0) { tagsListLength = undefined; }
        tagsListLength = tagsListLength || this.tagsInputContainer.find(".tag-text").length;
        var availableTags = this.tagsInputOption.limit - tagsListLength;
        this.tagLimit
            .text(this.tagsInputOption.limitText.replace("#", availableTags.toString()))
            .toggleClass("error", tagsListLength === this.tagsInputOption.limit);
    };
    UsernameTagsInputComponent.prototype.checkDelimiter = function (event) {
        var eventWhich = event.which;
        if (eventWhich === 13) {
            return true;
        }
        for (var i = 0, max = this.tagsInputOption.delimiter.length; i < max; i++) {
            if (eventWhich === this.tagsInputOption.delimiter[i].charCodeAt(0)) {
                return true;
            }
        }
        return false;
    };
    UsernameTagsInputComponent.prototype.splitIntoTags = function (value) {
        var _this = this;
        if (value.length) {
            var text_1 = value;
            var tmpDelimiter_1 = "∞";
            this.tagsInputOption.delimiter.forEach(function (delimiter) {
                text_1 = text_1.split(delimiter).join(tmpDelimiter_1);
            });
            // filter to remove empty tag
            var tagsList = text_1.split(tmpDelimiter_1).filter(function (item) { return $.trim(item); });
            // remove duplicate tag
            var uniqueTagsList_2 = [];
            tagsList.forEach(function (item) { return _this.tagExist(uniqueTagsList_2, item) || uniqueTagsList_2.push(item); });
            return uniqueTagsList_2;
        }
        return [];
    };
    UsernameTagsInputComponent.prototype.validateTag = function (value) {
        var valueLength = value.length;
        if (!valueLength) {
            return false;
        }
        for (var i = 0, max = this.tagsInputOption.delimiter.length; i < max; i++) {
            if (value.indexOf(this.tagsInputOption.delimiter[i]) > -1) {
                return false;
            }
        }
        return true;
    };
    UsernameTagsInputComponent.prototype.getUsernames = function () {
        var tagsList = [];
        this.tagsInputContainer.find(".tag-text").each(function () {
            tagsList.push(this.innerText);
        });
        return tagsList;
    };
    UsernameTagsInputComponent.prototype.getCustIds = function () {
        var tagsList = [];
        this.tagsInputContainer.find(".tag:not(.error)").each(function () {
            tagsList.push($(this).data("custid"));
        });
        return tagsList;
    };
    return UsernameTagsInputComponent;
}());
//# sourceMappingURL=username-tagsinput.js.map
"use strict";
var FilterConfigurationView = /** @class */ (function () {
    function FilterConfigurationView() {
    }
    FilterConfigurationView.prototype.init = function () {
        var welcomePopup = $("#welcome-popup");
        welcomePopup.length && welcomePopup.modal("show");
        this.cacheDOMElements();
        this.bindEvents();
    };
    FilterConfigurationView.prototype.cacheDOMElements = function () {
        var _this = this;
        this.inlineMessage = new InlineMessageComponent();
        this.bettypeDropdown = new DropdownListComponent();
        this.bettypeDropdown.render("ddl-bettypes");
        this.matchLiveIndicatorDropdown = new DropdownListComponent();
        this.matchLiveIndicatorDropdown.render("ddl-matchLiveIndicator", {
            onChange: function (select) { return _this.getSports(select.val(), _this.groupOfSportsDropdown.getSelected()[0]); }
        });
        this.groupOfSportsDropdown = new DropdownListComponent();
        this.groupOfSportsDropdown.render("ddl-groupOfSport", {
            onChange: function (select) { return _this.getSports(_this.matchLiveIndicatorDropdown.getSelected()[0], select.val()); }
        });
        this.sportLeagueMatchTreeview = new SportLeagueMatchTreeViewComponent();
        this.sportLeagueMatchTreeview.render(new SportLeagueMatchTreeViewOption("sportleaguematch-treeview"));
        this.monitoredWatchlistMemberTreeview = new SportLeagueMatchTreeViewComponent();
        this.monitoredWatchlistMemberTreeview.render(new SportLeagueMatchTreeViewOption("monitored-watchlist-members", "Search member username", "No member username found", "All (Include newly add ...)"));
        this.memberTagsInput = new UsernameTagsInputComponent();
        this.memberTagsInput.render(new UsernameTagsInputOption("username", 100, "Max: # username(s)", "Duplicated username"));
        this.stake = $("#txt-stake");
    };
    FilterConfigurationView.prototype.bindEvents = function () {
        var _this = this;
        this.stake.on("input", function () {
            this.value = alphaReports.forceInputDecimal(this.value);
        });
        var boxFilterBySports = $("#box-filterBySports");
        $("#rdo-allSports").change(function () {
            this.checked && boxFilterBySports.addClass("hide");
        });
        $("#rdo-specificSports").change(function (e) {
            if ($(e.target).is(":checked")) {
                boxFilterBySports.removeClass("hide");
                _this.getSports(_this.matchLiveIndicatorDropdown.getSelected()[0], _this.groupOfSportsDropdown.getSelected()[0]);
            }
        });
        var boxFilterByMembers = $("#box-filterByMembers");
        $("#rdo-allMembers").change(function () {
            this.checked && boxFilterByMembers.addClass("hide");
        });
        var rdoSpecificMembers = $("#rdo-specificMembers").change(function () {
            if (rdoSpecificMembers.is(":checked")) {
                boxFilterByMembers.removeClass("hide");
                _this.monitoredWatchlistMemberTreeview.addIconLoading();
                $.get("GetMonitoredWatchlistMembers", function (data) {
                    return _this.monitoredWatchlistMemberTreeview.renderTreeNodeList(data.map(function (d) {
                        return {
                            value: d.CustId,
                            text: d.Username
                        };
                    }));
                });
            }
        });
        $(".btn-reset").click(function () {
            _this.inlineMessage.hide();
            var formData = _this.getFormData(rdoSpecificMembers);
            console.dir(formData);
            if (formData) {
                _this.addFilterPopup = $("#add-filter-popup");
                _this.addFilterPopup.modal("show");
            }
        });
    };
    FilterConfigurationView.prototype.getSports = function (isLive, groupSport) {
        var _this = this;
        this.sportLeagueMatchTreeview.addIconLoading();
        $.get("FilterBySports?isLive=" + isLive + "&groupSport=" + groupSport, function (data) {
            var listData = [];
            data.forEach(function (sport) {
                var leagueList = [];
                sport.Leagues.forEach(function (league) {
                    leagueList.push({
                        value: league.Id,
                        text: league.Name,
                        attributes: {
                            "data-leagueid": league.Id
                        },
                        items: league.Matches.map(function (m) {
                            return {
                                value: m.MatchId,
                                text: m.MatchName,
                                subText: m.KickOffTimeStr,
                                attributes: {
                                    "data-homeid": m.HomeId,
                                    "data-awayid": m.AwayId,
                                    "data-isneutral": m.IsNeutral
                                }
                            };
                        })
                    });
                });
                listData.push({
                    value: sport.Id,
                    text: sport.Name,
                    attributes: {
                        "data-sportid": sport.Id
                    },
                    items: leagueList
                });
            });
            return _this.sportLeagueMatchTreeview.renderTreeNodeList(listData);
        });
    };
    FilterConfigurationView.prototype.getFormData = function (rdoSpecificMembers) {
        var betType = this.bettypeDropdown.isSelectAll() ? "0" : this.bettypeDropdown.getSelected().join();
        var stake = +this.stake.val();
        var isLive = this.matchLiveIndicatorDropdown.getSelected()[0] === "1";
        var sportTypeList = "0";
        if (!this.sportLeagueMatchTreeview.isSelectAll()) {
        }
        var isWatchList = 2;
        var inputList = "0";
        var watchList = "0";
        if (rdoSpecificMembers.is(":checked")) {
            if (this.monitoredWatchlistMemberTreeview.isSelectAll()) {
                isWatchList = 1;
            }
            else {
                isWatchList = 0;
                watchList = this.monitoredWatchlistMemberTreeview.getSelected().join();
            }
            inputList = this.memberTagsInput.getCustIds().join();
        }
        return {
            betType: betType,
            stake: stake,
            isLive: isLive,
            sportTypeList: sportTypeList,
            isWatchList: isWatchList,
            inputList: inputList,
            watchList: watchList
        };
    };
    return FilterConfigurationView;
}());
$(function () {
    new FilterConfigurationView().init();
});
//# sourceMappingURL=filter-configuration.js.map