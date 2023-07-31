"use strict";
var AJAXHelper = /** @class */ (function () {
    function AJAXHelper() {
    }
    AJAXHelper.getJSONWithoutBlockUI = function (url, onSuccess) {
        var _this = this;
        $.getJSON(url, onSuccess).fail(function (jqXHR) { return _this.handleOnFail(jqXHR); });
    };
    AJAXHelper.postJSON = function (url, formData, onSuccess, onComplete) {
        var _this = this;
        $.blockUI();
        return $.ajax({
            url: url,
            headers: {
                __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(),
                'X-Requested-With': 'XMLHttpRequest'
            },
            dataType: "json",
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
        if (jqXHR.statusText === "abort") {
            return;
        }
        if (jqXHR) {
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
            .removeClass("msg-error msg-success hide")
            .addClass(result.ErrCode ? "msg-error" : "msg-success");
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
                selectAllText: this.dropdownList.data("label-selectall"),
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
    DropdownListComponent.prototype.enable = function () {
        this.dropdownList.multiselect("enable");
    };
    DropdownListComponent.prototype.disable = function () {
        this.dropdownList.multiselect("disable");
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
        sportsAllLeagues.length && sports.push({ sport: sportsAllLeagues.join(), league: "0" });
        return sports;
    };
    return SportLeagueMatchTreeViewComponent;
}());
//# sourceMappingURL=sportleaguematch-treeview.js.map
"use strict";
var UsernameTagsInputOption = /** @class */ (function () {
    function UsernameTagsInputOption(selectorId, limit, limitText, duplicatedUsernameText, baseUrl, delimiter) {
        if (selectorId === void 0) { selectorId = "tagsinput"; }
        if (limit === void 0) { limit = 100; }
        if (limitText === void 0) { limitText = "Max: # member(s)"; }
        if (duplicatedUsernameText === void 0) { duplicatedUsernameText = "Member already exists"; }
        if (baseUrl === void 0) { baseUrl = ""; }
        if (delimiter === void 0) { delimiter = [","]; }
        this.selectorId = selectorId;
        this.limit = limit;
        this.limitText = limitText;
        this.duplicatedUsernameText = duplicatedUsernameText;
        this.baseUrl = baseUrl;
        this.delimiter = delimiter;
        this.selectorId = selectorId;
        this.limit = limit;
        this.limitText = limitText;
        this.duplicatedUsernameText = duplicatedUsernameText;
        this.baseUrl = baseUrl;
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
        var tagsInputContainer = $("#" + this.tagsInputOption.selectorId + "_tagsinput");
        this.tagLimit = tagsInputContainer.find("#" + this.tagsInputOption.selectorId + "_taglimit");
        this.realInput = tagsInputContainer.find("#" + this.tagsInputOption.selectorId + "_input");
        this.tagsBox = tagsInputContainer.find("#" + this.tagsInputOption.selectorId + "_tags");
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
            tagsList.length > 1 && _this.addTagsList(tagsList, false);
        });
        this.realInput.blur(function () {
            var visible = _this.realInput.next(".typeahead").is(":visible");
            !visible && _this.addTag(_this.realInput.val(), false);
        });
        // If a user types a delimiter create a new tag
        this.realInput.keypress(function (event) {
            if (_this.checkDelimiter(event)) {
                event.preventDefault();
                _this.addTag(_this.realInput.val(), false);
            }
        });
        this.initSuggestion();
    };
    UsernameTagsInputComponent.prototype.initSuggestion = function () {
        var _this = this;
        var urlAndQueryString = this.tagsInputOption.baseUrl + "/Get?suggestionquery=";
        var typeaheadOptions = {
            source: function (query, process) {
                return $.get(urlAndQueryString + query, process);
            },
            items: 10,
            minLength: 2,
            highlighter: function (displayText) {
                var parts = displayText.split("∞");
                return parts[0] + "<br/>" + parts[1];
            },
            displayText: function (item) {
                var nickname = item.Username2 ? " (" + item.Username2 + ")" : "";
                return "<strong>" + item.Username + nickname + "</strong>\u221E" + item.FirstName + " " + item.LastName;
            },
            autoSelect: false,
            afterSelect: function (item) {
                _this.realInput.val("");
                _this.addTag(item.Username, true);
            },
            delay: 200
        };
        this.realInput.typeahead(typeaheadOptions);
    };
    UsernameTagsInputComponent.prototype.addTag = function (value, fromSuggestion) {
        value = $.trim(value);
        this.validateTag(value) && this.addTagsList([value], fromSuggestion);
    };
    UsernameTagsInputComponent.prototype.addTagsList = function (tagsList, fromSuggestion) {
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
            var formData = { usernames: uniqueTagsList_1.join(), fromSuggestion: fromSuggestion };
            $.post(this.tagsInputOption.baseUrl + "/CheckUsernames", formData, function (results) { return _this.appendTags(results); });
        }
    };
    UsernameTagsInputComponent.prototype.tagExist = function (tagsList, value) {
        var valueLowerCase = value.toLowerCase();
        return tagsList.find(function (tag) { return tag.toLowerCase() === valueLowerCase; });
    };
    UsernameTagsInputComponent.prototype.appendTags = function (results) {
        var _this = this;
        results.forEach(function (result) {
            if (!result.ErrCode && _this.tagsBox.find(".tag[data-custid=" + result.CustId + "]").length) {
                result.ErrCode = 1;
                result.ErrMsg = _this.tagsInputOption.duplicatedUsernameText;
            }
            _this.tagsBox.append($("<span>", {
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
        this.tagsBox.find(".error:last").tooltip("destroy").tooltip();
        this.updateTagLimitText();
        this.realInput.val("").focus();
    };
    UsernameTagsInputComponent.prototype.updateTagLimitText = function (tagsListLength) {
        if (tagsListLength === void 0) { tagsListLength = undefined; }
        tagsListLength = tagsListLength || this.tagsBox.find(".tag-text").length;
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
        this.tagsBox.find(".tag-text").each(function () {
            tagsList.push(this.innerText);
        });
        return tagsList;
    };
    UsernameTagsInputComponent.prototype.getCustIds = function () {
        var tagsList = [];
        this.tagsBox.find(".tag:not(.error)").each(function () {
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
        this.betTypeDropdown.render("ddl-bettype", {
            onChange: function () { return TipsyHelper.clear($("#ddl-bettype").next(".btn-group")); }
        });
        this.liveStatusDropdown = new DropdownListComponent();
        this.liveStatusDropdown.render("ddl-live-status");
        this.sportGroupDropdown = new DropdownListComponent();
        this.sportGroupDropdown.render("ddl-sport-group", {
            onChange: function (select) { return _this.getSports(select.val()); }
        });
        this.sportLeagueMatchTreeview = new SportLeagueMatchTreeViewComponent();
        this.sportLeagueMatchTreeview.render(new SportLeagueMatchTreeViewOption("sportleaguematch-treeview", noInfoText, $("#hidden-SearchSportLeague").val(), $("#hidden-SportLeagueNotFound").val(), $("#hidden-SelectAll").val(), true, $("#hidden-ExpandAll").val(), $("#hidden-SelectedOnly").val()));
        this.myWatchlistMemberTreeview = new SportLeagueMatchTreeViewComponent();
        this.myWatchlistMemberTreeview.render(new SportLeagueMatchTreeViewOption("my-watchlist-members", noInfoText, $("#hidden-SearchMemberUsername").val(), $("#hidden-MemberUsernameNotFound").val(), $("#hidden-AllIncludedNewlyAddedMembers").val()));
        this.usernameTagsInput = new UsernameTagsInputComponent();
        this.usernameTagsInput.render(new UsernameTagsInputOption("username", 100, $("#hidden-MaxNoUsernames").val(), $("#hidden-UsernameAlreadyExists").val(), $("#hidden-UsernameSuggestionUrl").val()));
    };
    FilterConfigurationView.prototype.bindEvents = function () {
        var _this = this;
        this.stakeInput = $("#txt-stake").on("input", function () {
            this.value = alphaReports.forceInputDecimal(this.value);
        });
        var sportsContainer = $("#sports-container");
        $("#rdo-all-sports").change(function () {
            this.checked && sportsContainer.addClass("hide");
        });
        var rdoSpecificSports = $("#rdo-specific-sports").change(function (e) {
            if (rdoSpecificSports.is(":checked")) {
                sportsContainer.removeClass("hide");
                _this.getSports(_this.sportGroupDropdown.getSelected()[0]);
            }
        });
        var membersContainer = $("#members-container");
        $("#rdo-all-members").change(function () {
            this.checked && membersContainer.addClass("hide");
        });
        var rdoSpecificMembers = $("#rdo-specific-members").change(function () {
            if (rdoSpecificMembers.is(":checked")) {
                membersContainer.removeClass("hide");
                _this.getMyWatchlistMembers();
            }
        });
        var addFilterPopup = $("#add-filter-popup");
        this.filterNameInput = addFilterPopup.find("#txt-filterName");
        this.isDefaultCheckbox = addFilterPopup.find("#chk-isDefault");
        addFilterPopup
            .on("shown.bs.modal", function () { return _this.filterNameInput.focus(); })
            .on("hide.bs.modal", function () {
            _this.filterNameInput.val("");
            TipsyHelper.clear(_this.filterNameInput);
            _this.isDefaultCheckbox.prop("checked", false);
            _this.inlineMessagePopup.hide();
        });
        addFilterPopup.find("#btn-save-filter-popup").click(function () { return _this.handleSaveFilterPopup(function () { return Common.Utils.ReloadPageAfterTimeout(_this.getCurrentUrlWithoutQueryString()); }); });
        addFilterPopup.find("#btn-save-filter-apply-popup").click(function () { return _this.handleSaveFilterPopup(function () {
            var url = _this.getCurrentUrlWithoutQueryString();
            var mainPage = _this.getMainPageUrl(url);
            Common.Utils.ReloadPageAfterTimeout(mainPage + "?filterName=" + _this.addFilterFormData.filterName);
        }); });
        $(".button-apply.apply").click(function (e) {
            _this.addFilterFormData = _this.getFormData(rdoSpecificSports, rdoSpecificMembers);
            _this.addFilterFormData && _this.handleApplyFilter();
        });
        $(".button-save.save").click(function (e) {
            _this.addFilterFormData = _this.getFormData(rdoSpecificSports, rdoSpecificMembers);
            _this.addFilterFormData && addFilterPopup.modal("show");
        });
    };
    FilterConfigurationView.prototype.getSports = function (groupSport) {
        var _this = this;
        this.sportGroupDropdown.disable();
        this.sportLeagueMatchTreeview.addIconLoading();
        AJAXHelper.getJSONWithoutBlockUI("FilterBySports?groupSport=" + groupSport, function (data) {
            var listData = data.map(function (sport) {
                return {
                    value: sport.Id,
                    text: sport.Name,
                    items: sport.Leagues.map(function (league) {
                        return {
                            value: league.LeagueId,
                            text: league.LeagueName
                        };
                    })
                };
            });
            _this.sportLeagueMatchTreeview.renderTreeNodeList(listData);
            _this.sportGroupDropdown.enable();
        });
    };
    FilterConfigurationView.prototype.getMyWatchlistMembers = function () {
        var _this = this;
        this.myWatchlistMemberTreeview.addIconLoading();
        AJAXHelper.getJSONWithoutBlockUI("GetMyWatchlistMembers", function (data) {
            return _this.myWatchlistMemberTreeview.renderTreeNodeList(data.map(function (d) {
                return {
                    value: d.CustId,
                    text: d.Username + (d.Username2 ? " (" + d.Username2 + ")" : "")
                };
            }));
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
                var betTypeElement = $("#ddl-bettype").next(".btn-group");
                TipsyHelper.showError(betTypeElement, $("#hidden-BetTypesRequiredError").val());
                this.scrollTopInlineMessage(betTypeElement);
                return;
            }
        }
        var stake = $.trim(this.stakeInput.val().replace(/,/g, ""));
        if (!stake.length) {
            TipsyHelper.showError(this.stakeInput, $("#hidden-StakeRequiredError").val());
            return;
        }
        var liveStatus = this.liveStatusDropdown.getSelected()[0];
        var sports = [{ sport: "0", league: "0" }];
        if (rdoSpecificSports.is(":checked")) {
            var sportGroup = this.sportGroupDropdown.getSelected()[0];
            if (sportGroup === "0" && !this.sportLeagueMatchTreeview.isSelectAll() || sportGroup !== "0") {
                sports = this.sportLeagueMatchTreeview.getSportsLeagues();
                if (!sports.length) {
                    this.inlineMessage.showError($("#hidden-SportsRequiredError").val());
                    this.scrollTopInlineMessage();
                    return;
                }
            }
        }
        var filterByMembers = 2; // all members
        var inputList = "0";
        var myWatchlist = "0";
        if (rdoSpecificMembers.is(":checked")) {
            if (this.myWatchlistMemberTreeview.isSelectAll()) {
                filterByMembers = 1; // all my wachlist members
            }
            else {
                filterByMembers = 0;
                myWatchlist = this.myWatchlistMemberTreeview.getSelected().join();
            }
            inputList = this.usernameTagsInput.getCustIds().join();
            if (!myWatchlist.length && !inputList.length) {
                this.inlineMessage.showError($("#hidden-FilterByMembersRequiredError").val());
                this.scrollTopInlineMessage();
                return;
            }
        }
        this.inlineMessage.hide();
        return {
            betType: betType,
            stake: stake,
            liveStatus: liveStatus,
            sports: sports,
            filterByMembers: filterByMembers,
            inputList: inputList,
            myWatchlist: myWatchlist
        };
    };
    FilterConfigurationView.prototype.getFormDataPopup = function () {
        var filterName = $.trim(this.filterNameInput.val());
        if (!filterName.length) {
            TipsyHelper.showError(this.filterNameInput, $("#hidden-FilterNameRequiredError").val());
            return;
        }
        var isDefault = this.isDefaultCheckbox.is(":checked");
        return {
            filterName: filterName,
            isDefault: isDefault
        };
    };
    FilterConfigurationView.prototype.handleApplyFilter = function () {
        var _this = this;
        this.addFilterFormData.filterName = "Default";
        AJAXHelper.postJSON("ApplyFilter", this.addFilterFormData, function (result) {
            if (result.ErrCode) {
                _this.inlineMessage.showError(result.ErrMsg);
            }
            else {
                var url = _this.getCurrentUrlWithoutQueryString();
                window.location.href = _this.getMainPageUrl(url);
                _this.addFilterFormData = null;
            }
        });
    };
    FilterConfigurationView.prototype.handleSaveFilterPopup = function (reloadPageCallback) {
        var _this = this;
        var formDataPopup = this.getFormDataPopup();
        if (formDataPopup) {
            this.addFilterFormData = $.extend(this.addFilterFormData, formDataPopup);
            AJAXHelper.postJSON("AddFilter", this.addFilterFormData, function (result) {
                _this.inlineMessagePopup.show(result);
                if (!result.ErrCode) {
                    reloadPageCallback();
                    _this.addFilterFormData = null;
                }
            });
        }
    };
    FilterConfigurationView.prototype.getCurrentUrlWithoutQueryString = function () {
        return window.location.href.split("?")[0];
    };
    FilterConfigurationView.prototype.getMainPageUrl = function (currentUrl) {
        return currentUrl.replace(/FilterConfiguration/i, "");
    };
    FilterConfigurationView.prototype.scrollTopInlineMessage = function ($element) {
        if ($element === void 0) { $element = undefined; }
        $("html, body").scrollTop(($element || this.inlineMessage.getInlineMessage()).offset().top);
    };
    return FilterConfigurationView;
}());
$(function () {
    new FilterConfigurationView().init();
});
//# sourceMappingURL=filter-configuration.js.map