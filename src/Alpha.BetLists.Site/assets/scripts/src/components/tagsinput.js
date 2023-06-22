"use strict";
var TagsInputOption = /** @class */ (function () {
    function TagsInputOption(selectorId, placeholder, limit, limitText, maxChars, minChars, width, height, delimiter, allowDuplicate, validationPattern) {
        if (selectorId === void 0) { selectorId = "tagsinput"; }
        if (placeholder === void 0) { placeholder = "Input tag (Separate by comma, tab or enter)"; }
        if (limit === void 0) { limit = null; }
        if (limitText === void 0) { limitText = "Max: # tag(s)"; }
        if (maxChars === void 0) { maxChars = null; }
        if (minChars === void 0) { minChars = 0; }
        if (width === void 0) { width = "auto"; }
        if (height === void 0) { height = "75px"; }
        if (delimiter === void 0) { delimiter = [","]; }
        if (allowDuplicate === void 0) { allowDuplicate = false; }
        if (validationPattern === void 0) { validationPattern = null; }
        this.selectorId = selectorId;
        this.placeholder = placeholder;
        this.limit = limit;
        this.limitText = limitText;
        this.maxChars = maxChars;
        this.minChars = minChars;
        this.width = width;
        this.height = height;
        this.delimiter = delimiter;
        this.allowDuplicate = allowDuplicate;
        this.validationPattern = validationPattern;
        this.selectorId = selectorId;
        this.placeholder = placeholder;
        this.limit = limit;
        this.limitText = limitText;
        this.maxChars = maxChars;
        this.minChars = minChars;
        this.width = width;
        this.height = height;
        this.delimiter = delimiter;
        this.allowDuplicate = allowDuplicate;
        this.validationPattern = validationPattern;
    }
    return TagsInputOption;
}());
var TagsInputComponent = /** @class */ (function () {
    function TagsInputComponent() {
    }
    TagsInputComponent.prototype.render = function (option) {
        this.tagsInputOption = option;
        this.realInput = $("#" + option.selectorId).hide();
        this.buildTagsInput();
        this.registerFakeInputEvents();
        return this.realInput;
    };
    TagsInputComponent.prototype.buildTagsInput = function () {
        var _this = this;
        var tagsInputId = this.realInput.attr("id") || Math.random().toString(36).substring(2);
        var tagsInputContainerId = tagsInputId + "_tagsinput";
        var tagLimitId = tagsInputId + "_taglimit";
        var fakeInputId = tagsInputId + "_tag";
        this.realInput.after([
            '<div class="tagsinput" id="',
            tagsInputContainerId,
            '" style="',
            this.tagsInputOption.width ? "width:" + this.tagsInputOption.width + ";" : "",
            this.tagsInputOption.height ? "min-height:" + this.tagsInputOption.height : "",
            '">',
            this.buildTagLimit(tagLimitId),
            this.buildTagAdd(fakeInputId),
            "</div>"
        ].join(""));
        this.tagsInputContainer = $("#" + tagsInputContainerId);
        this.tagLimit = this.tagsInputContainer.find("#" + tagLimitId);
        this.fakeInput = this.tagsInputContainer.find("#" + fakeInputId);
        var value = $.trim(this.realInput.val());
        var tags = this.splitIntoTags(value);
        if (tags.length) {
            this.realInput.val("");
            tags.forEach(function (tag) { return _this.addTag(tag, false); });
        }
    };
    TagsInputComponent.prototype.buildTagLimit = function (tagLimitId) {
        return this.tagsInputOption.limit
            ? [
                '<span class="tag-limit" id="',
                tagLimitId,
                '">',
                this.tagsInputOption.limitText.replace("#", this.tagsInputOption.limit),
                "</span>"
            ].join("")
            : "";
    };
    TagsInputComponent.prototype.buildTagAdd = function (fakeInputId) {
        return [
            '<input class="tag-add" id="',
            fakeInputId,
            '" placeholder="',
            this.tagsInputOption.placeholder,
            '"',
            this.tagsInputOption.maxChars ? " maxlength=\"" + this.tagsInputOption.maxChars + "\"" : "",
            ' autocomplete="off"/>'
        ].join("");
    };
    TagsInputComponent.prototype.registerFakeInputEvents = function () {
        var _this = this;
        this.fakeInput.data("pasted", false);
        this.fakeInput.on("paste", function () { return _this.fakeInput.data("pasted", true); });
        // If a user pastes the text check if it shouldn't be splitted into tags
        this.fakeInput.on("input", function () {
            if (!_this.fakeInput.data("pasted")) {
                return;
            }
            _this.fakeInput.data("pasted", false);
            var value = $.trim(_this.fakeInput.val());
            var tags = _this.splitIntoTags(value);
            if (tags.length > 1) {
                tags.forEach(function (tag) { return _this.addTag(tag); });
            }
        });
        this.fakeInput.on("focus", function () {
            _this.tagsInputContainer.addClass("focus");
            !_this.fakeInput.val().length && _this.fakeInput.removeClass("error");
        });
        this.fakeInput.blur(function () {
            _this.tagsInputContainer.removeClass("focus");
            _this.addTag(_this.fakeInput.val());
        });
        // If a user types a delimiter create a new tag
        this.fakeInput.keypress(function (event) {
            if (_this.checkDelimiter(event)) {
                event.preventDefault();
                _this.addTag(_this.fakeInput.val());
            }
        });
        // Removes the error class when user changes the value of the fake input
        this.fakeInput.keydown(function (event) {
            var which = event.which;
            // enter, alt, shift, esc, ctrl and arrows keys are ignored
            var ignoredKeyCodes = [13, 37, 38, 39, 40, 27, 16, 17, 18, 225];
            if (!ignoredKeyCodes.find(function (code) { return code === which; })) {
                _this.fakeInput.removeClass("error");
            }
        });
    };
    TagsInputComponent.prototype.addTag = function (value, isFocus) {
        var _this = this;
        if (isFocus === void 0) { isFocus = true; }
        value = $.trim(value);
        if (!this.validateTag(value)) {
            this.fakeInput.addClass("error");
            return false;
        }
        var delimiter = this.getDelimiter();
        var realInputValue = this.realInput.val();
        var tagsList = realInputValue.length ? realInputValue.split(delimiter) : [];
        if (!this.validateTagsList(tagsList) ||
            (!this.tagsInputOption.allowDuplicate && this.tagExist(tagsList, value))) {
            this.fakeInput.addClass("error");
            return false;
        }
        this.tagsInputContainer.append($("<span>", { class: "tag" }).append($("<span>", { class: "tag-text" }).text(value), $("<button>", { type: "button", class: "tag-remove" }).click(function (e) { return _this.removeTag($(e.target).parent()); })));
        tagsList.push(value);
        this.realInput.val(tagsList.join(delimiter));
        this.updateTagLimitText(tagsList);
        this.fakeInput.val("");
        isFocus ? this.fakeInput.focus() : this.fakeInput.blur();
    };
    TagsInputComponent.prototype.tagExist = function (tagsList, value) {
        var valueLowerCase = value.toLowerCase();
        return tagsList.find(function (tag) { return tag.toLowerCase() === valueLowerCase; });
    };
    TagsInputComponent.prototype.removeTag = function (tag) {
        var valueLowerCase = tag.find(".tag-text").text().toLowerCase();
        var delimiter = this.getDelimiter();
        var tagsList = this.realInput.val().split(delimiter);
        tagsList = tagsList.filter(function (tag) { return tag.toLowerCase() !== valueLowerCase; });
        this.updateTagLimitText(tagsList);
        this.realInput.val(tagsList.join(delimiter));
        tag.remove();
    };
    TagsInputComponent.prototype.updateTagLimitText = function (tagsList) {
        if (this.tagLimit.length) {
            var tagsListLength = tagsList.length;
            var availableTags = this.tagsInputOption.limit - tagsListLength;
            this.tagLimit
                .text(this.tagsInputOption.limitText.replace("#", availableTags))
                .toggleClass("error", tagsListLength === this.tagsInputOption.limit);
        }
    };
    TagsInputComponent.prototype.getDelimiter = function () {
        return this.tagsInputOption.delimiter[0];
    };
    TagsInputComponent.prototype.checkDelimiter = function (event) {
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
    TagsInputComponent.prototype.splitIntoTags = function (value) {
        if (value.length) {
            var text_1 = value;
            var tmpDelimiter_1 = "∞";
            this.tagsInputOption.delimiter.forEach(function (delimiter) {
                text_1 = text_1.split(delimiter).join(tmpDelimiter_1);
            });
            return text_1.split(tmpDelimiter_1);
        }
        return [];
    };
    TagsInputComponent.prototype.validateTag = function (value) {
        var valueLength = value.length;
        if (!valueLength) {
            return false;
        }
        if (valueLength < this.tagsInputOption.minChars) {
            return false;
        }
        if (this.tagsInputOption.maxChars && valueLength > this.tagsInputOption.maxChars) {
            return false;
        }
        if (this.tagsInputOption.validationPattern && !this.tagsInputOption.validationPattern.test(value)) {
            return false;
        }
        for (var i = 0, max = this.tagsInputOption.delimiter.length; i < max; i++) {
            if (value.indexOf(this.tagsInputOption.delimiter[i]) > -1) {
                return false;
            }
        }
        return true;
    };
    TagsInputComponent.prototype.validateTagsList = function (tagsList) {
        if (this.tagsInputOption.limit && tagsList.length > this.tagsInputOption.limit) {
            return false;
        }
        return true;
    };
    TagsInputComponent.prototype.getValue = function () {
        return this.realInput.val();
    };
    return TagsInputComponent;
}());
//# sourceMappingURL=tagsinput.js.map