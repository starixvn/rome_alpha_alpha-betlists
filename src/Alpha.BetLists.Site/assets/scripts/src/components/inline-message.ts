"use strict";

class InlineMessageComponent {
    inlineMessage: JQuery;

    constructor(selectorId = "inline-message") {
        this.inlineMessage = $(`#${selectorId}`);
    }

    public getInlineMessage() {
        return this.inlineMessage;
    }

    public show(result) {
        this.inlineMessage
            .html(result.ErrMsg)
            .removeClass("msg-error msg-success hide")
            .addClass(result.ErrCode ? "msg-error" : "msg-success");
    }

    public showError(message: string) {
        this.show({ ErrCode: 1, ErrMsg: message });
    }

    public showSuccess(message: string) {
        this.show({ ErrCode: 0, ErrMsg: message });
    }

    public hide() {
        this.inlineMessage.addClass("hide").html("");
    }
}