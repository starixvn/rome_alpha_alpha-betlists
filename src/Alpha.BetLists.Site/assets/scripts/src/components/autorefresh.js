"use strict";
var defaultValue = 60;
var AutoRefreshOption = /** @class */ (function () {
    function AutoRefreshOption(onText, offText, callback) {
        if (onText === void 0) { onText = "ON"; }
        if (offText === void 0) { offText = "OFF"; }
        if (callback === void 0) { callback = Function(); }
        this.onText = onText;
        this.offText = offText;
        this.callback = callback;
        this.onText = onText;
        this.offText = offText;
        this.callback = callback;
    }
    return AutoRefreshOption;
}());
var AutoRefreshComponent = /** @class */ (function () {
    function AutoRefreshComponent() {
    }
    AutoRefreshComponent.prototype.init = function (option) {
        var _this = this;
        this.autoRefreshOption = option;
        var autoRefresh = $("#auto-refresh");
        this.labelStatusText = autoRefresh.find("#lbl-status-text")
            .data("running", "0")
            .text(this.autoRefreshOption.onText);
        this.labelCountDown = autoRefresh.find("#lbl-count-down");
        this.labelCountDown.text(defaultValue).data("original", defaultValue);
        autoRefresh.find("#btn-status").click(function () { return _this.labelStatusText.data("running") === "1" ? _this.stop() : _this.start(); });
        autoRefresh.find(".auto-refresh-menu-item").click(function (e) {
            var second = $(e.target).text();
            _this.labelCountDown.text(second).data("original", second);
        });
        this.start();
    };
    AutoRefreshComponent.prototype.start = function () {
        var _this = this;
        clearTimeout(this.timeout);
        this.labelStatusText
            .data("running", "1")
            .text(this.autoRefreshOption.onText);
        this.labelCountDown
            .removeClass("icon-loading")
            .text(this.labelCountDown.data("original"));
        this.timeout = setTimeout(function () { return _this.decrement(); }, 1000);
    };
    AutoRefreshComponent.prototype.stop = function () {
        clearTimeout(this.timeout);
        this.labelStatusText
            .data("running", "0")
            .text(this.autoRefreshOption.offText);
        this.labelCountDown
            .removeClass("icon-loading")
            .text(this.labelCountDown.data("original"));
    };
    AutoRefreshComponent.prototype.decrement = function () {
        var _this = this;
        var value = +this.labelCountDown.text();
        if (value === 1) {
            clearTimeout(this.timeout);
            this.labelCountDown
                .text("")
                .addClass("icon-loading");
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