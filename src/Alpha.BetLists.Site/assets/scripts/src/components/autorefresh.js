"use strict";
var AutoRefreshOption = /** @class */ (function () {
    function AutoRefreshOption(onText, offText, callback, onOffStatus) {
        if (onText === void 0) { onText = "ON"; }
        if (offText === void 0) { offText = "OFF"; }
        if (callback === void 0) { callback = Function(); }
        if (onOffStatus === void 0) { onOffStatus = Function(); }
        this.onText = onText;
        this.offText = offText;
        this.callback = callback;
        this.onOffStatus = onOffStatus;
        this.onText = onText;
        this.offText = offText;
        this.callback = callback;
        this.onOffStatus = onOffStatus;
    }
    return AutoRefreshOption;
}());
var AutoRefreshComponent = /** @class */ (function () {
    function AutoRefreshComponent() {
    }
    AutoRefreshComponent.prototype.init = function (option) {
        var _this = this;
        this.autoRefreshOption = option;
        this.autoRefresh = $("#auto-refresh");
        this.labelStatusText = this.autoRefresh.find("#lbl-status-text")
            .data("running", "0")
            .text(this.autoRefreshOption.offText);
        this.labelCountDown = this.autoRefresh.find("#lbl-count-down");
        this.autoRefresh.find("#btn-status").click(function () {
            var running = _this.labelStatusText.data("running") === "1";
            running ? _this.stop() : _this.start();
            option.onOffStatus(!running);
        });
        this.autoRefresh.find(".auto-refresh-menu-item").click(function (e) {
            var second = $(e.target).text();
            _this.labelCountDown.text(second).data("original", second);
        });
    };
    AutoRefreshComponent.prototype.setIsLoading = function () {
        this.labelCountDown.text("").addClass("show-loading");
    };
    AutoRefreshComponent.prototype.isOn = function () {
        return !this.autoRefresh.hasClass("off");
    };
    AutoRefreshComponent.prototype.start = function () {
        var _this = this;
        clearTimeout(this.timeout);
        this.autoRefresh.removeClass("off");
        this.labelStatusText
            .data("running", "1")
            .text(this.autoRefreshOption.onText);
        this.labelCountDown
            .removeClass("show-loading")
            .text(this.labelCountDown.data("original"));
        this.timeout = setTimeout(function () { return _this.decrement(); }, 1000);
    };
    AutoRefreshComponent.prototype.stop = function () {
        clearTimeout(this.timeout);
        this.autoRefresh.addClass("off");
        this.labelStatusText
            .data("running", "0")
            .text(this.autoRefreshOption.offText);
        this.labelCountDown
            .removeClass("show-loading")
            .text(this.labelCountDown.data("original"));
    };
    AutoRefreshComponent.prototype.decrement = function () {
        var _this = this;
        var value = +this.labelCountDown.text();
        if (value === 1) {
            clearTimeout(this.timeout);
            this.labelCountDown
                .text("")
                .addClass("show-loading");
            this.autoRefreshOption.callback();
        }
        else {
            this.labelCountDown.text(--value);
            this.timeout = setTimeout(function () { return _this.decrement(); }, 1000);
        }
    };
    return AutoRefreshComponent;
}());
//# sourceMappingURL=autorefresh.js.map