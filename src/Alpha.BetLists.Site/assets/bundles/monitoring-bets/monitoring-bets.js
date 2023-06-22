"use strict";
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
        this.isRunning = false;
    }
    AutoRefreshComponent.prototype.init = function (option) {
        var _this = this;
        this.autoRefreshOption = option;
        var autoRefresh = $("#auto-refresh");
        this.labelStatusText = autoRefresh.find("#lbl-status-text");
        this.labelCountDown = autoRefresh.find("#lbl-count-down");
        autoRefresh.find("#btn-status").click(function () { return _this.isRunning ? _this.stop() : _this.start(); });
        autoRefresh.find(".auto-refresh-menu-item").click(function (e) {
            clearTimeout(_this.timeout);
            var menuItem = $(e.target);
            var second = menuItem.text();
            _this.labelCountDown.text(second).data("original", second);
            _this.timeout = setTimeout(function () { return _this.decrement(); }, 1000);
        });
    };
    AutoRefreshComponent.prototype.start = function () {
        var _this = this;
        clearTimeout(this.timeout);
        this.isRunning = true;
        this.labelStatusText.text(this.autoRefreshOption.onText);
        this.labelCountDown.text(this.labelCountDown.data("original"));
        this.timeout = setTimeout(function () { return _this.decrement(); }, 1000);
    };
    AutoRefreshComponent.prototype.stop = function () {
        clearTimeout(this.timeout);
        this.isRunning = false;
        this.labelStatusText.text(this.autoRefreshOption.offText);
        this.labelCountDown.text(this.labelCountDown.data("original"));
    };
    AutoRefreshComponent.prototype.decrement = function () {
        var _this = this;
        var value = +this.labelCountDown.text();
        if (value === 1) {
            clearTimeout(this.timeout);
            this.labelCountDown.text("...");
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
"use strict";
var MonitoringBetsView = /** @class */ (function () {
    function MonitoringBetsView(autoRefreshComponent) {
        this.autoRefreshComponent = autoRefreshComponent;
    }
    MonitoringBetsView.prototype.init = function () {
        this.dropdownFilter = $("#monitoring-bets-filter");
        this.dropdownFilter.multiselect({
            multiple: false,
            maxHeight: 300,
            numberDisplayed: 1,
            includeSelectAllOption: false
        });
        //this.dropdownFilter.find("option:selected:first").val();
        this.dropdownTop = $("#monitoring-bets-top");
        this.dropdownTop.multiselect({
            multiple: false,
            maxHeight: 300,
            numberDisplayed: 1,
            includeSelectAllOption: false
        });
        //this.dropdownTop.find("option:selected:first").val();
        var self = this;
        this.autoRefreshComponent.init(new AutoRefreshOption("ON", "OFF", function () {
            setTimeout(function () {
                console.log('get data from server');
                self.autoRefreshComponent.start();
            }, 3000);
        }));
    };
    return MonitoringBetsView;
}());
$(function () {
    new MonitoringBetsView(new AutoRefreshComponent()).init();
});
//# sourceMappingURL=monitoring-bets.js.map