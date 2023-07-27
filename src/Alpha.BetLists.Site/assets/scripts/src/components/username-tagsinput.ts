"use strict";

class UsernameTagsInputOption {
    constructor(
        public selectorId = "tagsinput",
        public limit = 100,
        public limitText = "Max: # member(s)",
        public duplicatedUsernameText = "Member already exists",
        public baseUrl = "",
        public delimiter = [","]) {
        this.selectorId = selectorId;
        this.limit = limit;
        this.limitText = limitText;
        this.duplicatedUsernameText = duplicatedUsernameText;
        this.baseUrl = baseUrl;
        this.delimiter = delimiter;
    }
}

class UsernameTagsInputComponent {
    private tagsInputOption: UsernameTagsInputOption;
    private tagLimit: JQuery;
    private realInput: JQuery;
    private tagsBox: JQuery;

    render(option: UsernameTagsInputOption) {
        this.tagsInputOption = option;
        this.cacheDOMElements();
        this.bindEvents();
    }

    private cacheDOMElements() {
        const tagsInputContainer = $(`#${this.tagsInputOption.selectorId}_tagsinput`);
        this.tagLimit = tagsInputContainer.find(`#${this.tagsInputOption.selectorId}_taglimit`);
        this.realInput = tagsInputContainer.find(`#${this.tagsInputOption.selectorId}_input`);
        this.tagsBox = tagsInputContainer.find(`#${this.tagsInputOption.selectorId}_tags`);

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
            tagsList.length > 1 && this.addTagsList(tagsList, false);
        });

        this.realInput.blur(() => {
            const visible = this.realInput.next(".typeahead").is(":visible");
            !visible && this.addTag(this.realInput.val(), false);
        });

        // If a user types a delimiter create a new tag
        this.realInput.keypress((event) => {
            if (this.checkDelimiter(event)) {
                event.preventDefault();
                this.addTag(this.realInput.val(), false);
            }
        });

        this.initSuggestion();
    }

    private initSuggestion() {
        const urlAndQueryString = `${this.tagsInputOption.baseUrl}/Get?suggestionquery=`;

        const typeaheadOptions = {
            source: function (query: string, process) {
                return $.get(urlAndQueryString + query, process);
            },
            items: 10,
            minLength: 2,
            highlighter: function (displayText: string) {
                const parts = displayText.split("∞");
                return `${parts[0]}<br/>${parts[1]}`;
            },
            displayText: function (item) {
                const nickname = item.Username2 ? ` (${item.Username2})` : "";
                return `<strong>${item.Username}${nickname}</strong>∞${item.FirstName} ${item.LastName}`;
            },
            autoSelect: false,
            afterSelect: (item) => {
                this.realInput.val("");
                this.addTag(item.Username, true);
            },
            delay: 200
        };

        this.realInput.typeahead(typeaheadOptions);
    }

    private addTag(value: string, fromSuggestion: boolean) {
        value = $.trim(value);
        this.validateTag(value) && this.addTagsList([value], fromSuggestion);
    }

    private addTagsList(tagsList, fromSuggestion: boolean) {
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

            const formData = { usernames: uniqueTagsList.join(), fromSuggestion };
            $.post(`${this.tagsInputOption.baseUrl}/CheckUsernames`, formData, (results) => this.appendTags(results));
        }
    }

    private tagExist(tagsList, value: string) {
        const valueLowerCase = value.toLowerCase();

        return tagsList.find((tag) => tag.toLowerCase() === valueLowerCase);
    }

    private appendTags(results) {
        results.forEach((result) => {
            if (!result.ErrCode && this.tagsBox.find(`.tag[data-custid=${result.CustId}]`).length) {
                result.ErrCode = 1;
                result.ErrMsg = this.tagsInputOption.duplicatedUsernameText;
            }

            this.tagsBox.append(
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

        this.tagsBox.find(".error:last").tooltip("destroy").tooltip();
        this.updateTagLimitText();
        this.realInput.val("").focus();
    }

    private updateTagLimitText(tagsListLength = undefined) {
        tagsListLength = tagsListLength || this.tagsBox.find(".tag-text").length;
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
        this.tagsBox.find(".tag-text").each(function () {
            tagsList.push(this.innerText);
        });

        return tagsList;
    }

    getCustIds() {
        const tagsList = [];
        this.tagsBox.find(".tag:not(.error)").each(function () {
            tagsList.push($(this).data("custid"));
        });

        return tagsList;
    }
}