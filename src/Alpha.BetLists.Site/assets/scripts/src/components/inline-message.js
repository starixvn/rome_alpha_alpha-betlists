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