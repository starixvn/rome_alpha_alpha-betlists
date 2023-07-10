"use strict";
var AJAXHelper = /** @class */ (function () {
    function AJAXHelper() {
    }
    AJAXHelper.getJSONWithoutBlockUI = function (url, onSuccess) {
        var _this = this;
        $.getJSON(url, onSuccess).fail(function (jqXHR) { return _this.handleOnFail(jqXHR); });
    };
    AJAXHelper.post = function (url, formData, onSuccess, onComplete) {
        var _this = this;
        $.blockUI();
        return $.ajax({
            url: url,
            headers: {
                __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(),
                'X-Requested-With': 'XMLHttpRequest'
            },
            type: "POST",
            data: formData,
            success: function (result) {
                onSuccess(result);
                $.unblockUI();
            },
            error: function (jqXHR) { return _this.handleOnFail(jqXHR); },
            complete: onComplete
        });
    };
    AJAXHelper.handleOnFail = function (jqXHR) {
        $.unblockUI();
        if (jqXHR) {
            if (jqXHR.statusText === "abort") {
                return;
            }
            if (jqXHR.status === 401) {
                window.top["age"].SignOut();
            }
            else if (jqXHR.status === 500 && jqXHR.responseText) {
                var jsonData = JSON.parse(jqXHR.responseText);
                if (jsonData.UMFrom && jsonData.UMTo) {
                    location.href = window.top["alpha"].urls.mainRootPath + "PartialUM/Index?umfrom=" + jsonData.UMFrom + "&umto=" + jsonData.UMTo;
                }
            }
        }
    };
    AJAXHelper.get = function (url, formData, onSuccess, onComplete) {
        var _this = this;
        $.blockUI();
        return $.ajax({
            url: url,
            headers: {
                __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(),
                'X-Requested-With': 'XMLHttpRequest'
            },
            type: "GET",
            data: formData,
            success: function (result) {
                onSuccess(result);
                $.unblockUI();
            },
            error: function (jqXHR) { return _this.handleOnFail(jqXHR); },
            complete: onComplete
        });
    };
    return AJAXHelper;
}());
//# sourceMappingURL=ajax-helper.js.map
"use strict";
var InlineMessageComponent = /** @class */ (function () {
    function InlineMessageComponent(selectorId) {
        if (selectorId === void 0) { selectorId = "inline-message"; }
        this.inlineMessage = $("#" + selectorId);
    }
    InlineMessageComponent.prototype.getInlineMessage = function () {
        return this.inlineMessage;
    };
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
                _this.checkboxListContainer.find(":checkbox:not(:checked)")
                    .closest(".tree-node")
                    .toggleClass("hide", _this.selectedOnly.is(":checked"));
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
        this.checkboxListContainer.html('<li><span class="icon-loading"></span></li>');
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
    SportLeagueMatchTreeViewComponent.prototype.getSelected = function () {
        var selected = [];
        this.checkboxListContainer.find(":checkbox:checked").each(function (_, item) {
            selected.push(item.value);
        });
        return selected;
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
        sportsAllLeagues.length && sports.push({ sport: sportsAllLeagues.join() });
        return sports;
    };
    return SportLeagueMatchTreeViewComponent;
}());
//# sourceMappingURL=sportleaguematch-treeview.js.map
"use strict";
var UsernameTagsInputOption = /** @class */ (function () {
    function UsernameTagsInputOption(selectorId, limit, limitText, duplicatedUsernameText, width, height, delimiter) {
        if (selectorId === void 0) { selectorId = "tagsinput"; }
        if (limit === void 0) { limit = 100; }
        if (limitText === void 0) { limitText = "Max: # username(s)"; }
        if (duplicatedUsernameText === void 0) { duplicatedUsernameText = "Username already exists"; }
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
                "data-custid": result.CustId,
                "data-toggle": "tooltip",
                "data-placement": "top"
            }).append($("<span>", { class: "tag-text" }).text(result.Username), $("<button>", { type: "button", class: "tag-remove" }).click(function (e) {
                var tag = $(e.target).parent();
                tag.hasClass("error") && tag.tooltip("destroy");
                tag.remove();
                _this.updateTagLimitText();
            })));
        });
        this.tagsInputContainer.find(".error:last").tooltip("destroy").tooltip();
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
        this.initComponents();
        this.bindEvents();
    };
    FilterConfigurationView.prototype.initComponents = function () {
        var _this = this;
        var noInfoText = $("#hidden-NoInformationIsAvailable").val();
        this.inlineMessage = new InlineMessageComponent();
        this.inlineMessagePopup = new InlineMessageComponent("inline-message-popup");
        this.betTypeDropdown = new DropdownListComponent();
        this.betTypeDropdown.render("ddl-betType");
        this.eventStatusDropdown = new DropdownListComponent();
        this.eventStatusDropdown.render("ddl-eventStatus", {
            onChange: function (select) { return _this.getSports(select.val(), _this.sportGroupDropdown.getSelected()[0]); }
        });
        this.sportGroupDropdown = new DropdownListComponent();
        this.sportGroupDropdown.render("ddl-sportGroup", {
            onChange: function (select) { return _this.getSports(_this.eventStatusDropdown.getSelected()[0], select.val()); }
        });
        this.sportLeagueMatchTreeview = new SportLeagueMatchTreeViewComponent();
        this.sportLeagueMatchTreeview.render(new SportLeagueMatchTreeViewOption("sportleaguematch-treeview", noInfoText, $("#hidden-SearchSportLeague").val(), $("#hidden-SportLeagueNotFound").val(), $("#hidden-SelectAll").val(), true, $("#hidden-ExpandAll").val(), $("#hidden-SelectedOnly").val()));
        this.monitoredWatchlistMemberTreeview = new SportLeagueMatchTreeViewComponent();
        this.monitoredWatchlistMemberTreeview.render(new SportLeagueMatchTreeViewOption("monitored-watchlist-members", noInfoText, $("#hidden-SearchMemberUsername").val(), $("#hidden-MemberUsernameNotFound").val(), $("#hidden-AllIncludedNewlyAddedMembers").val()));
        this.usernameTagsInput = new UsernameTagsInputComponent();
        this.usernameTagsInput.render(new UsernameTagsInputOption("username", 100, $("#hidden-MaxNoUsernames").val(), $("#hidden-UsernameAlreadyExists").val()));
    };
    FilterConfigurationView.prototype.bindEvents = function () {
        var _this = this;
        this.stakeInput = $("#txt-stake").on("input", function () {
            this.value = alphaReports.forceInputDecimal(this.value);
        });
        var boxFilterBySports = $("#box-filterBySports");
        $("#rdo-allSports").change(function () {
            this.checked && boxFilterBySports.addClass("hide");
        });
        var rdoSpecificSports = $("#rdo-specificSports").change(function (e) {
            if ($(e.target).is(":checked")) {
                boxFilterBySports.removeClass("hide");
                _this.getSports(_this.eventStatusDropdown.getSelected()[0], _this.sportGroupDropdown.getSelected()[0]);
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
        var addFilterPopup = $("#add-filter-popup");
        addFilterPopup.on("hidden.bs.modal", function () {
            $("#txt-filterName").val("");
            $("#chk-isDefault").prop("checked", false);
        });
        addFilterPopup.find(".btn-submit").click(function () {
            var formDataPopup = _this.getFormDataPopup();
            if (formDataPopup) {
                _this.addFilterFormData = $.extend(_this.addFilterFormData, formDataPopup);
                AJAXHelper.postJSON("AddFilter", _this.addFilterFormData, function (result) {
                    _this.inlineMessagePopup.show(result);
                    if (!result.ErrCode) {
                        _this.addFilterFormData = null;
                        var url = window.location.href;
                        if (url.indexOf("?welcome=True") > -1) {
                            url = url.replace("?welcome=True", "");
                        }
                        Common.Utils.ReloadPageAfterTimeout(url);
                    }
                });
            }
        });
        $(".button-reset").click(function () {
            _this.addFilterFormData = _this.getFormData(rdoSpecificSports, rdoSpecificMembers);
            _this.addFilterFormData && addFilterPopup.modal("show");
        });
    };
    FilterConfigurationView.prototype.getSports = function (isLive, groupSport) {
        var _this = this;
        this.sportLeagueMatchTreeview.addIconLoading();
        $.get("FilterBySports?isLive=" + isLive + "&groupSport=" + groupSport, function (data) {
            var listData = data.map(function (sport) {
                return {
                    value: sport.Id,
                    text: sport.Name,
                    items: sport.Leagues.map(function (league) {
                        return {
                            value: league.Id,
                            text: league.Name
                        };
                    })
                };
            });
            return _this.sportLeagueMatchTreeview.renderTreeNodeList(listData);
        });
    };
    FilterConfigurationView.prototype.getFormData = function (rdoSpecificSports, rdoSpecificMembers) {
        var betType;
        if (this.betTypeDropdown.isSelectAll()) {
            betType = "0";
        }
        else {
            betType = this.betTypeDropdown.getSelected().join();
            if (!betType.length) {
                this.inlineMessage.showError($("#hidden-BetTypesRequiredError").val());
                this.scrollTopInlineMessage();
                return;
            }
        }
        var stake = $.trim(this.stakeInput.val().replace(/,/g, ""));
        if (!stake.length) {
            this.inlineMessage.showError($("#hidden-StakeRequiredError").val());
            this.stakeInput.focus();
            return;
        }
        var sports = [{ sport: 0 }];
        var isLive = false;
        if (rdoSpecificSports.is(":checked")) {
            isLive = this.eventStatusDropdown.getSelected()[0] === "1";
            if (!this.sportLeagueMatchTreeview.isSelectAll()) {
                sports = this.sportLeagueMatchTreeview.getSportsLeagues();
                if (!sports.length) {
                    this.inlineMessage.showError($("#hidden-SportsRequiredError").val());
                    this.scrollTopInlineMessage();
                    return;
                }
            }
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
            inputList = this.usernameTagsInput.getCustIds().join();
            if (!watchList.length && !inputList.length) {
                this.inlineMessage.showError($("#hidden-FilterByMembersRequiredError").val());
                this.scrollTopInlineMessage();
                return;
            }
        }
        this.inlineMessage.hide();
        return {
            betType: betType,
            stake: stake,
            isLive: isLive,
            sports: sports,
            isWatchList: isWatchList,
            inputList: inputList,
            watchList: watchList
        };
    };
    FilterConfigurationView.prototype.getFormDataPopup = function () {
        var filterNameElement = $("#txt-filterName");
        var filterName = $.trim(filterNameElement.val());
        if (!filterName.length) {
            this.inlineMessagePopup.showError($("#hidden-FilterNameRequiredError").val());
            return;
        }
        var isDefault = $("#chk-isDefault").is(":checked");
        this.inlineMessagePopup.hide();
        return {
            filterName: filterName,
            isDefault: isDefault
        };
    };
    FilterConfigurationView.prototype.scrollTopInlineMessage = function () {
        $("html, body").scrollTop(this.inlineMessage.getInlineMessage().offset().top);
    };
    return FilterConfigurationView;
}());
$(function () {
    new FilterConfigurationView().init();
});
//# sourceMappingURL=filter-configuration.js.map