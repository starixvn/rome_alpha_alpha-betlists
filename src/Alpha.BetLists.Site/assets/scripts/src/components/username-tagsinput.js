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