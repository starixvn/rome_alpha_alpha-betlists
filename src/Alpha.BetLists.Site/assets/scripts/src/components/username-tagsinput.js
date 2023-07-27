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