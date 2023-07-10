"use strict";

class UsernameTagsInputOption {
    constructor(
        public selectorId = "tagsinput",
        public limit = 100,
        public limitText = "Max: # username(s)",
        public duplicatedUsernameText = "Username already exists",
        public width = "auto",
        public height = "auto",
        public delimiter = [","]) {
        this.selectorId = selectorId;
        this.limit = limit;
        this.limitText = limitText;
        this.duplicatedUsernameText = duplicatedUsernameText;
        this.width = width;
        this.height = height;
        this.delimiter = delimiter;
    }
}

class UsernameTagsInputComponent {
    private tagsInputOption: UsernameTagsInputOption;
    private tagsInputContainer: JQuery;
    private tagLimit: JQuery;
    private realInput: JQuery;

    render(option: UsernameTagsInputOption) {
        this.tagsInputOption = option;
        this.cacheDOMElements();
        this.bindEvents();
    }

    private cacheDOMElements() {
        this.tagsInputContainer = $(`#${this.tagsInputOption.selectorId}_tagsinput`);
        this.tagLimit = this.tagsInputContainer.find(`#${this.tagsInputOption.selectorId}_taglimit`);
        this.realInput = this.tagsInputContainer.find(`#${this.tagsInputOption.selectorId}_input`);

        this.tagsInputContainer.css({
            width: this.tagsInputOption.width,
            "min-height": this.tagsInputOption.height
        });
        this.tagLimit.text(this.tagsInputOption.limitText.replace("#", this.tagsInputOption.limit.toString()));
    }

    private bindEvents() {
        this.realInput.data("pasted", false);
        this.realInput.on("paste", () => this.realInput.data("pasted", true));

        // If a user pastes the text check if it shouldn't be splitted into tags
        this.realInput.on("input", () => {
            if (!this.realInput.data("pasted")) {
                return;
            }

            this.realInput.data("pasted", false);

            const value = $.trim(this.realInput.val());
            const tagsList = this.splitIntoTags(value);
            tagsList.length > 1 && this.addTagsList(tagsList);
        });

        this.realInput.on("focus", () => this.tagsInputContainer.addClass("focus"));

        this.realInput.blur(() => {
            this.tagsInputContainer.removeClass("focus");
            this.addTag(this.realInput.val());
        });

        // If a user types a delimiter create a new tag
        this.realInput.keypress((event) => {
            if (this.checkDelimiter(event)) {
                event.preventDefault();
                this.addTag(this.realInput.val());
            }
        });
    }

    private addTag(value: string) {
        value = $.trim(value);
        this.validateTag(value) && this.addTagsList([value]);
    }

    private addTagsList(tagsList) {
        const usernames = this.getUsernames();
        const usernamesLength = usernames.length;

        if (this.tagsInputOption.limit > usernamesLength) {
            tagsList = tagsList.slice(0, this.tagsInputOption.limit - usernamesLength);
            const uniqueTagsList = [];
            tagsList.forEach((item) => {
                if (this.tagExist(usernames, item)) {
                    this.appendTags([{ Username: item, ErrCode: 1, ErrMsg: this.tagsInputOption.duplicatedUsernameText }]);
                } else {
                    uniqueTagsList.push(item);
                }
            });

            $.post("CheckUsernames", { usernames: uniqueTagsList.join() }, (results) => this.appendTags(results));
        }
    }

    private tagExist(tagsList, value: string) {
        const valueLowerCase = value.toLowerCase();

        return tagsList.find((tag) => tag.toLowerCase() === valueLowerCase);
    }

    private appendTags(results) {
        results.forEach((result) => {
            this.tagsInputContainer.append(
                $("<span>", {
                    class: `tag${result.ErrCode ? " error" : ""}`,
                    title: result.ErrMsg,
                    "data-custid": result.CustId,
                    "data-toggle": "tooltip",
                    "data-placement": "top"
                }).append(
                    $("<span>", { class: "tag-text" }).text(result.Username),
                    $("<button>", { type: "button", class: "tag-remove" }).click((e) => {
                        const tag = $(e.target).parent();
                        tag.hasClass("error") && tag.tooltip("destroy");
                        tag.remove();
                        this.updateTagLimitText();
                    })
                ));
        });

        this.tagsInputContainer.find(".error:last").tooltip("destroy").tooltip();
        this.updateTagLimitText();
        this.realInput.val("").focus();
    }

    private updateTagLimitText(tagsListLength = undefined) {
        tagsListLength = tagsListLength || this.tagsInputContainer.find(".tag-text").length;
        const availableTags = this.tagsInputOption.limit - tagsListLength;
        this.tagLimit
            .text(this.tagsInputOption.limitText.replace("#", availableTags.toString()))
            .toggleClass("error", tagsListLength === this.tagsInputOption.limit);
    }

    private checkDelimiter(event) {
        const eventWhich = event.which;
        if (eventWhich === 13) {
            return true;
        }

        for (let i = 0, max = this.tagsInputOption.delimiter.length; i < max; i++) {
            if (eventWhich === this.tagsInputOption.delimiter[i].charCodeAt(0)) {
                return true;
            }
        }

        return false;
    }

    private splitIntoTags(value: string) {
        if (value.length) {
            let text = value;
            const tmpDelimiter = "∞";

            this.tagsInputOption.delimiter.forEach(function (delimiter) {
                text = text.split(delimiter).join(tmpDelimiter);
            });

            // filter to remove empty tag
            const tagsList = text.split(tmpDelimiter).filter(item => $.trim(item));

            // remove duplicate tag
            const uniqueTagsList = [];
            tagsList.forEach((item) => this.tagExist(uniqueTagsList, item) || uniqueTagsList.push(item));

            return uniqueTagsList;
        }

        return [];
    }

    private validateTag(value: string) {
        const valueLength = value.length;
        if (!valueLength) {
            return false;
        }

        for (let i = 0, max = this.tagsInputOption.delimiter.length; i < max; i++) {
            if (value.indexOf(this.tagsInputOption.delimiter[i]) > -1) {
                return false;
            }
        }

        return true;
    }

    private getUsernames() {
        const tagsList = [];
        this.tagsInputContainer.find(".tag-text").each(function () {
            tagsList.push(this.innerText);
        });

        return tagsList;
    }

    getCustIds() {
        const tagsList = [];
        this.tagsInputContainer.find(".tag:not(.error)").each(function () {
            tagsList.push($(this).data("custid"));
        });

        return tagsList;
    }
}