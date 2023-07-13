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
    }
    AutoRefreshComponent.prototype.init = function (option) {
        var _this = this;
        this.autoRefreshOption = option;
        var autoRefresh = $("#auto-refresh");
        this.labelStatusText = autoRefresh.find("#lbl-status-text")
            .data("running", "0")
            .text(this.autoRefreshOption.offText);
        this.labelCountDown = autoRefresh.find("#lbl-count-down");
        autoRefresh.find("#btn-status").click(function () { return _this.labelStatusText.data("running") === "1" ? _this.stop() : _this.start(); });
        autoRefresh.find(".auto-refresh-menu-item").click(function (e) {
            var second = $(e.target).text();
            _this.labelCountDown.text(second).data("original", second);
        });
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
"use strict";
var MonitoringBetsCopyHtml = /** @class */ (function () {
    function MonitoringBetsCopyHtml(clipboard) {
        this.clipboard = clipboard;
    }
    MonitoringBetsCopyHtml.prototype.init = function () {
        var _this = this;
        $("#icon-copy").tooltip();
        var clipboard = new this.clipboard("#icon-copy", {
            text: function () { return _this._copyTableHtml($("#tbl-betlist")); }
        });
        if (window.document.queryCommandSupported && window.document.queryCommandSupported("copy")) {
            clipboard.on("success", function (e) {
                var $this = $(e.trigger);
                $this.attr("data-original-title", $("#hidden-CopiedText").val());
                $this.tooltip("show");
                setTimeout(function () {
                    $this.tooltip("hide");
                    $this.attr("data-original-title", $("#hidden-CopyText").val());
                }, 3000);
            }).on("error", function (e) {
                $(e.trigger).remove();
                console.error("Action:", e.action);
                console.error("Trigger:", e.trigger);
            });
        }
    };
    MonitoringBetsCopyHtml.prototype._copyTableHtml = function (tableReport) {
        var html = ['<table>'];
        this._appendTableHeader(tableReport, html);
        this._appendTableBody(tableReport, html);
        html.push('</table>');
        return html.join("");
    };
    MonitoringBetsCopyHtml.prototype._appendTableHeader = function (tableReport, html) {
        html.push('<thead><tr>');
        tableReport.find("thead tr:first th").each(function () {
            var $this = $(this);
            html.push('<th>', this.innerText, '</th>');
        });
        html.push('</tr></thead>');
    };
    MonitoringBetsCopyHtml.prototype._appendTableBody = function (tableReport, html) {
        html.push('<tbody>');
        tableReport.find("tbody tr").each(function () {
            html.push('<tr>');
            $(this).find("td").each(function () {
                html.push('<td>', this.innerText, '</td>');
            });
            html.push('</tr>');
        });
        html.push('</tbody>');
    };
    return MonitoringBetsCopyHtml;
}());
//# sourceMappingURL=monitoring-bets-copyhtml.js.map
"use strict";
var MonitoringBetsView = /** @class */ (function () {
    function MonitoringBetsView(autoRefreshComponent, monitoringBetsCopyHtml) {
        this.autoRefreshComponent = autoRefreshComponent;
        this.monitoringBetsCopyHtml = monitoringBetsCopyHtml;
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
        //this.monitoringBetsCopyHtml.init();
    };
    return MonitoringBetsView;
}());
$(function () {
    new MonitoringBetsView(new AutoRefreshComponent(), new MonitoringBetsCopyHtml(Clipboard)).init();
});
//# sourceMappingURL=monitoring-bets.js.map