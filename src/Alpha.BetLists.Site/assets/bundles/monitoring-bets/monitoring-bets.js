"use strict";
var AJAXHelper = /** @class */ (function () {
    function AJAXHelper() {
    }
    AJAXHelper.getJSONWithoutBlockUI = function (url, onSuccess) {
        var _this = this;
        $.getJSON(url, onSuccess).fail(function (jqXHR) { return _this.handleOnFail(jqXHR); });
    };
    AJAXHelper.post = function (url, formData, onSuccess, onComplete) {
        var _this = this;
        $.blockUI();
        return $.ajax({
            url: url,
            headers: {
                __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(),
                'X-Requested-With': 'XMLHttpRequest'
            },
            type: "POST",
            data: formData,
            success: function (result) {
                onSuccess(result);
                $.unblockUI();
            },
            error: function (jqXHR) { return _this.handleOnFail(jqXHR); },
            complete: onComplete
        });
    };
    AJAXHelper.handleOnFail = function (jqXHR) {
        $.unblockUI();
        if (jqXHR) {
            if (jqXHR.statusText === "abort") {
                return;
            }
            if (jqXHR.status === 401) {
                window.top["age"].SignOut();
            }
            else if (jqXHR.status === 500 && jqXHR.responseText) {
                var jsonData = JSON.parse(jqXHR.responseText);
                if (jsonData.UMFrom && jsonData.UMTo) {
                    location.href = window.top["alpha"].urls.mainRootPath + "PartialUM/Index?umfrom=" + jsonData.UMFrom + "&umto=" + jsonData.UMTo;
                }
            }
        }
    };
    AJAXHelper.get = function (url, formData, onSuccess, onComplete) {
        var _this = this;
        $.blockUI();
        return $.ajax({
            url: url,
            headers: {
                __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(),
                'X-Requested-With': 'XMLHttpRequest'
            },
            type: "GET",
            data: formData,
            success: function (result) {
                onSuccess(result);
                $.unblockUI();
            },
            error: function (jqXHR) { return _this.handleOnFail(jqXHR); },
            complete: onComplete
        });
    };
    return AJAXHelper;
}());
//# sourceMappingURL=ajax-helper.js.map
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
        this.autoRefresh = $("#auto-refresh");
        this.labelStatusText = this.autoRefresh.find("#lbl-status-text")
            .data("running", "0")
            .text(this.autoRefreshOption.offText);
        this.labelCountDown = this.autoRefresh.find("#lbl-count-down");
        this.labelCountDown.text(defaultValue).data("original", defaultValue);
        this.autoRefresh.find("#btn-status").click(function () { return _this.labelStatusText.data("running") === "1" ? _this.stop() : _this.start(); });
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
"use strict";
var SaveFilterView = /** @class */ (function () {
    function SaveFilterView() {
    }
    SaveFilterView.prototype.init = function (element) {
        var _this = this;
        if (element.length) {
            var saveFilterPopup_1 = $("#save-filter-popup");
            this.inlineMessagePopup = new InlineMessageComponent("inline-message-popup");
            this.filterNameInput = saveFilterPopup_1.find("#txt-filterName");
            this.isDefaultCheckbox = saveFilterPopup_1.find("#chk-isDefault");
            saveFilterPopup_1
                .on("shown.bs.modal", function () { return _this.filterNameInput.focus(); })
                .on("hide.bs.modal", function () {
                _this.filterNameInput.val("");
                TipsyHelper.clear(_this.filterNameInput);
                _this.isDefaultCheckbox.prop("checked", false);
                _this.inlineMessagePopup.hide();
            });
            saveFilterPopup_1.find("#btn-save-filter-popup").click(function () { return _this.handleSaveFilterPopup(function () { return Common.Utils.ReloadPageAfterTimeout(_this.getCurrentUrlWithoutQueryString()); }); });
            /*saveFilterPopup.find("#btn-save-filter-apply-popup").click(
                () => this.handleSaveFilterPopup(() => {
                    const url = this.getCurrentUrlWithoutQueryString();
                    const mainPage = this.getMainPageUrl(url);
                    Common.Utils.ReloadPageAfterTimeout(`${mainPage}?filterName=${this.addFilterFormData.filterName}`);
                })
            );*/
            element.click(function () {
                saveFilterPopup_1.modal("show");
            });
        }
    };
    SaveFilterView.prototype.handleSaveFilterPopup = function (reloadPageCallback) {
        var _this = this;
        var formDataPopup = this.getFormDataPopup();
        if (formDataPopup) {
            AJAXHelper.post($("#hidden-SaveFilterUrl").val(), formDataPopup, function (result) {
                _this.inlineMessagePopup.show(result);
                !result.ErrCode && reloadPageCallback();
            });
        }
    };
    SaveFilterView.prototype.getFormDataPopup = function () {
        var filterName = $.trim(this.filterNameInput.val());
        if (!filterName.length) {
            TipsyHelper.showError(this.filterNameInput, $("#hidden-FilterNameRequiredError").val());
            return;
        }
        var isDefault = this.isDefaultCheckbox.is(":checked");
        return {
            filterName: filterName,
            isDefault: isDefault
        };
    };
    SaveFilterView.prototype.getCurrentUrlWithoutQueryString = function () {
        return window.location.href.split("?")[0];
    };
    return SaveFilterView;
}());
//# sourceMappingURL=save-filter.js.map
"use strict";
var MonitoringBetsCopyHtml = /** @class */ (function () {
    function MonitoringBetsCopyHtml() {
    }
    MonitoringBetsCopyHtml.prototype.init = function () {
        var _this = this;
        $("#icon-copy").tooltip();
        var clipboard = new Clipboard("#icon-copy", {
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
    function MonitoringBetsView(autoRefreshComponent, monitoringBetsCopyHtml, saveFilter) {
        var _this = this;
        this.getBetsUrl = "";
        this.isCalling = false;
        this.lastTransIdCacheKeyByFilter = "";
        this.latestTransIdCacheKeyByFilter = "";
        this.cacheElements = function () {
            _this.$msgError = $("#msg-error");
            _this.$betListTableContainer = $("#bet-list-table-container");
            _this.getBetsUrl = $("#get-bets-url").val();
            _this.dropdownFilter = $("#monitoring-bets-filter");
            _this.dropdownMostRecentTicket = $("#most-recent-tickets");
        };
        this.registerEvents = function () {
            _this.registerToggleFilter();
            _this.dropdownFilter.multiselect({
                maxHeight: 300,
                onChange: function () { return _this.loadLastBets(true); }
            });
            _this.dropdownMostRecentTicket.multiselect({
                onChange: function () { return _this.loadLastBets(true); }
            });
            _this.autoRefreshComponent.init(new AutoRefreshOption($("#hidden-OnText").val(), $("#hidden-OffText").val(), function () { return _this.loadLastBets(false); }));
        };
        this.loadLastBets = function (isResetTransId) {
            if (isResetTransId) {
                _this.setTransId(0, 0);
            }
            _this.isCalling && _this.abortRequest();
            _this.isCalling = true;
            _this.ajaxRequest = AJAXHelper.post(_this.getBetsUrl, _this.getParam(), _this.successGetLastBets, _this.completeGetLastBets);
        };
        this.abortRequest = function () {
            _this.ajaxRequest && _this.ajaxRequest.abort();
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
            _this.$betListTableContainer.find('[data-toggle="tooltip"]').tooltip();
            var lastTransIdCacheKeyByFilter = _this.$betListTableContainer.find("#new-last-trans-id-by-filter").val();
            var latestTransIdCacheKeyByFilter = _this.$betListTableContainer.find("#new-latest-trans-id-by-filter").val();
            _this.setTransIdCacheKeyByFilter(lastTransIdCacheKeyByFilter, latestTransIdCacheKeyByFilter);
            var lastTransId = _this.$betListTableContainer.find("#last-trans-id").val();
            var latestTransId = _this.$betListTableContainer.find("#latest-trans-id").val();
            _this.setTransId(lastTransId, latestTransId);
            _this.isCalling = false;
        };
        this.showError = function () {
            _this.$msgError.removeClass("hide");
            // protection level, avoid through too many exceptions on server
        };
        this.completeGetLastBets = function () {
            _this.autoRefreshComponent.isOn() && _this.autoRefreshComponent.start();
        };
        this.autoRefreshComponent = autoRefreshComponent;
        this.monitoringBetsCopyHtml = monitoringBetsCopyHtml;
        this.saveFilter = saveFilter;
    }
    MonitoringBetsView.prototype.init = function () {
        this.localStorage = alphaReports.localStorage;
        this.cacheElements();
        this.registerEvents();
        this.setTransIdCacheKeyByFilter($("#last-trans-id-cache-key-by-filter").val(), $("#latest-trans-id-cache-key-by-filter").val());
        this.setTransId($("#last-trans-id").val(), $("#latest-trans-id").val());
        this.autoRefreshComponent.start();
        this.monitoringBetsCopyHtml.init();
        this.saveFilter.init($("#icon-save"));
        this.$betListTableContainer.find('[data-toggle="tooltip"]').tooltip();
    };
    MonitoringBetsView.prototype.registerToggleFilter = function () {
        var filterContainer = $("#form-monitoring-bets");
        var iconFilter = $("#icon-filter").click(function () {
            var isHideFilter = iconFilter.hasClass("icon-arrow-up-drop-circle");
            if (isHideFilter) {
                iconFilter
                    .removeClass("icon-arrow-up-drop-circle")
                    .addClass("icon-arrow-down-drop-circle")
                    .attr("title", $("#hidden-ShowFilterText").val());
                filterContainer.addClass("hide");
            }
            else {
                iconFilter
                    .removeClass("icon-arrow-down-drop-circle")
                    .addClass("icon-arrow-up-drop-circle")
                    .attr("title", $("#hidden-HideFilterText").val());
                filterContainer.removeClass("hide");
            }
        });
    };
    MonitoringBetsView.prototype.setTransIdCacheKeyByFilter = function (lastTransId, lastestTransId) {
        this.lastTransIdCacheKeyByFilter = lastTransId;
        this.latestTransIdCacheKeyByFilter = lastestTransId;
    };
    MonitoringBetsView.prototype.setTransId = function (lastTransId, lastestTransId) {
        this.localStorage.set(this.lastTransIdCacheKeyByFilter, lastTransId);
        this.localStorage.set(this.latestTransIdCacheKeyByFilter, lastestTransId);
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
        var FilterByMembers = selectedFilter.data("filter-by-member");
        var LimitRowCount = this.dropdownMostRecentTicket.find("option:selected:first").val();
        return {
            FilterId: FilterId,
            TransId: lastTransId ? lastTransId : 0,
            MemberList: MemberList,
            BetTypeList: BetTypeList,
            SportTypeList: SportTypeList,
            LeagueList: LeagueList,
            EvenStatus: EvenStatus,
            MinStake: MinStake,
            LimitRowCount: LimitRowCount,
            FilterByMembers: FilterByMembers,
            LatestTransId: latestTransId
        };
    };
    return MonitoringBetsView;
}());
$(function () {
    new MonitoringBetsView(new AutoRefreshComponent(), new MonitoringBetsCopyHtml(), new SaveFilterView()).init();
});
//# sourceMappingURL=monitoring-bets.js.map