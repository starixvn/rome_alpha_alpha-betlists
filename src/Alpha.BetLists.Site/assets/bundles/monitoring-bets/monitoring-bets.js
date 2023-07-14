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
        var _this = this;
        this.getBetsUrl = "";
        this.isCalling = false;
        this.pageSize = 10;
        this.timeToGetLastBets = 2000;
        this.timeToRetryGetLastBets = 600000;
        this.getLastBetsTimeOut = 0;
        this.lastTransIdCacheKeyByFilter = "";
        this.latestTransIdCacheKeyByFilter = "";
        this.cacheElements = function () {
            _this.$pageSize = $(".page-size");
            _this.$msgError = $("#msg-error");
            _this.$betListTableContainer = $("#bet-list-table-container");
            _this.getBetsUrl = $("#get-bets-url").val();
            _this.pageSize = parseInt($(".page-size.active").html());
            _this.dropdownFilter = $("#monitoring-bets-filter");
            _this.dropdownMostRecentTicket = $("#most-recent-tickets");
        };
        this.registerEvents = function () {
            var self = _this;
            _this.dropdownFilter.multiselect({
                multiple: false,
                maxHeight: 300,
                nonSelectedText: 'Select...',
                numberDisplayed: 1,
                includeSelectAllOption: false
            });
            _this.dropdownFilter.change(function () {
                _this.loadLastBets(false);
            });
            _this.dropdownMostRecentTicket.multiselect({
                multiple: false,
                maxHeight: 300,
                numberDisplayed: 1,
                includeSelectAllOption: false
            });
            _this.dropdownMostRecentTicket.change(function () {
                _this.loadLastBets(false);
            });
            _this.autoRefreshComponent.init(new AutoRefreshOption("ON", "OFF", function () {
                setTimeout(function () {
                    console.log('get data from server');
                    _this.loadLastBets(false);
                }, 3000);
            }));
        };
        this.loadLastBets = function (isAutoLoad) {
            if (!isAutoLoad) {
                _this.localStorage.set(_this.lastTransIdCacheKeyByFilter, 0);
                _this.localStorage.set(_this.latestTransIdCacheKeyByFilter, 0);
            }
            if (_this.isCalling) {
                _this.abortRequest();
            }
            _this.isCalling = true;
            _this.ajaxRequest = $.ajax({
                method: "GET",
                data: _this.getParam(),
                url: _this.getBetsUrl,
                success: _this.successGetLastBets,
                error: _this.errorRequest,
                complete: _this.completeGetLastBets
            });
        };
        this.abortRequest = function () {
            if (_this.ajaxRequest) {
                _this.ajaxRequest.abort();
            }
        };
        this.errorRequest = function (request) {
            if (request.statusText === "abort") {
                return;
            }
            if (request.status === 500 && request.responseText) {
                var jsonData = JSON.parse(request.responseText);
                if (jsonData.UMFrom && jsonData.UMTo) {
                    location.href = top["alpha"].urls.mainRootPath
                        + "PartialUM/Index?umfrom=" + jsonData.UMFrom
                        + "&umto=" + jsonData.UMTo;
                    return;
                }
            }
            _this.showError();
        };
        this.successGetLastBets = function (responseHtml) {
            if ((responseHtml.ErrorCode && responseHtml.ErrorCode !== "0") ||
                (responseHtml.ErrCode && responseHtml.ErrCode !== "0")) {
                _this.showError();
                return;
            }
            _this.$msgError.addClass("hide");
            if (!_this.isCalling) {
                return;
            }
            _this.$betListTableContainer.html(responseHtml);
            var lastTransId = _this.$betListTableContainer.find("#last-trans-id").val();
            _this.lastTransIdCacheKeyByFilter = _this.$betListTableContainer.find("#new-last-trans-id-by-filter").val();
            if (lastTransId > 0) {
                _this.localStorage.set(_this.lastTransIdCacheKeyByFilter, lastTransId);
            }
            _this.latestTransIdCacheKeyByFilter = _this.$betListTableContainer.find("#new-latest-trans-id-by-filter").val();
            var latestTransId = _this.$betListTableContainer.find("#latest-trans-id").val();
            _this.localStorage.set(_this.latestTransIdCacheKeyByFilter, latestTransId);
            _this.isCalling = false;
        };
        this.showError = function () {
            _this.$msgError.removeClass("hide");
            // protection level, avoid through too many exceptions on server
        };
        this.completeGetLastBets = function () {
            if (_this.autoRefreshComponent.labelStatusText.text() == _this.autoRefreshComponent.autoRefreshOption.onText)
                _this.autoRefreshComponent.start();
        };
        this.localStorage = {
            get: function (key) {
                return _this.store.enabled ? _this.store.get(key) : $.cookie(key);
            },
            set: function (key, value) {
                _this.store.enabled ? _this.store.set(key, value) : $.cookie(key, value, _this.localStorage.getCookieExpireOptions());
            },
            getCookieExpireOptions: function () {
                var cookieExpire = { expires: 365, path: window.location.pathname };
                if (_this.isIe()) {
                    cookieExpire = { expires: 365 };
                }
                return cookieExpire;
            }
        };
        this.isIe = function () {
            var ua = window.navigator.userAgent;
            var msie = ua.indexOf("MSIE ");
            return msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./);
        };
        this.autoRefreshComponent = autoRefreshComponent;
        this.monitoringBetsCopyHtml = monitoringBetsCopyHtml;
    }
    MonitoringBetsView.prototype.init = function () {
        this.store = store;
        this.cacheElements();
        this.registerEvents();
        this.setLastTransIdByFilter();
        this.loadLastBets(false);
        this.monitoringBetsCopyHtml.init();
    };
    MonitoringBetsView.prototype.setLastTransIdByFilter = function () {
        this.lastTransIdCacheKeyByFilter = $("#last-trans-id-cache-key-by-filter").val();
        this.latestTransIdCacheKeyByFilter = $("#latest-trans-id-cache-key-by-filter").val();
    };
    MonitoringBetsView.prototype.getParam = function () {
        var lastTransId = this.localStorage.get(this.lastTransIdCacheKeyByFilter);
        var latestTransId = this.localStorage.get(this.latestTransIdCacheKeyByFilter);
        var selectedFilter = this.dropdownFilter.find("option:selected:first");
        var FilterId = selectedFilter.val();
        var MemberList = selectedFilter.data("members");
        var BetTypeList = selectedFilter.data("bet-types");
        var SportTypeList = selectedFilter.data("sport-types");
        var LeagueList = selectedFilter.data("leagues");
        var EvenStatus = selectedFilter.data("event-status");
        var MinStake = selectedFilter.data("stake");
        var FilterByMember = selectedFilter.data("filter-by-member");
        var LimitRowCount = this.dropdownMostRecentTicket.find("option:selected:first").val();
        var param = {
            FilterId: FilterId,
            TransId: lastTransId ? lastTransId : "0",
            MemberList: MemberList,
            BetTypeList: BetTypeList,
            SportTypeList: SportTypeList,
            LeagueList: LeagueList,
            EvenStatus: EvenStatus,
            MinStake: MinStake,
            LimitRowCount: LimitRowCount,
            FilterByMember: FilterByMember,
            LatestTransId: latestTransId
        };
        return param;
    };
    return MonitoringBetsView;
}());
$(function () {
    new MonitoringBetsView(new AutoRefreshComponent(), new MonitoringBetsCopyHtml(Clipboard)).init();
});
//# sourceMappingURL=monitoring-bets.js.map