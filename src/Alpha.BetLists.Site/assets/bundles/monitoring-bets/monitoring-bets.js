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
                    var cookielessSiteMainUrl = $("#CookielessSiteMainUrl");
                    var siteMainUrl = cookielessSiteMainUrl.length
                        ? cookielessSiteMainUrl.val()
                        : window.top["alpha"].urls.mainRootPath;
                    location.href = siteMainUrl + "partialum/index?" + $.param(jsonData);
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
    AutoRefreshComponent.prototype.enable = function () {
        this.autoRefresh.find("#btn-status").prop("disabled", false);
    };
    AutoRefreshComponent.prototype.disable = function () {
        this.autoRefresh.find("#btn-status").prop("disabled", true);
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
    SaveFilterView.prototype.init = function (element, getSelectedFilterParamsFunc) {
        var _this = this;
        if (getSelectedFilterParamsFunc === void 0) { getSelectedFilterParamsFunc = undefined; }
        if (element.length) {
            this.inlineMessagePopup = new InlineMessageComponent("inline-message-popup");
            var saveFilterPopup_1 = $("#save-filter-popup");
            this.filterNameInput = saveFilterPopup_1.find("#txt-filterName");
            this.isDefaultCheckbox = saveFilterPopup_1.find("#chk-isDefault");
            this.isNotifyCheckbox = saveFilterPopup_1.find("#chk-isNotify");
            saveFilterPopup_1
                .on("shown.bs.modal", function () { return _this.filterNameInput.focus(); })
                .on("hide.bs.modal", function () {
                _this.filterNameInput.val("");
                TipsyHelper.clear(_this.filterNameInput);
                _this.isDefaultCheckbox.prop("checked", false);
                _this.isNotifyCheckbox.prop("checked", false);
                _this.inlineMessagePopup.hide();
            });
            saveFilterPopup_1.find("#btn-save-filter-popup").click(function () { return _this.handleSaveFilterPopup(getSelectedFilterParamsFunc); });
            element.click(function () {
                saveFilterPopup_1.modal("show");
            });
        }
    };
    SaveFilterView.prototype.handleSaveFilterPopup = function (getSelectedFilterParamsFunc) {
        var _this = this;
        var formDataPopup = this.getFormDataPopup();
        if (formDataPopup) {
            AJAXHelper.post($("#hidden-SaveFilterUrl").val(), formDataPopup, function (result) {
                _this.inlineMessagePopup.show(result);
                if (result.ErrCode) {
                    $(".lnk-chatbot").click(function () { location.href = $("#chatbot-path").val(); });
                }
                else {
                    var url = undefined;
                    if (getSelectedFilterParamsFunc) {
                        var filterParam = getSelectedFilterParamsFunc();
                        filterParam.filterName = formDataPopup.filterName;
                        url = window.location.href.split("?")[0] + "?" + $.param(filterParam);
                    }
                    Common.Utils.ReloadPageAfterTimeout(url);
                }
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
        var isNotify = this.isNotifyCheckbox.is(":checked");
        return {
            filterName: filterName,
            isDefault: isDefault,
            isNotify: isNotify
        };
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
            $(this).find("td").each(function (index) {
                html.push('<td>', index === 1 ? "'" : "", this.innerText, '</td>');
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
        this.loadLastBets = function (isResetTransId) {
            if (isResetTransId) {
                _this.setTransId(0, 0);
            }
            _this.isCalling && _this.abortRequest();
            _this.isCalling = true;
            _this.ajaxRequest = AJAXHelper.post(_this.getBetsUrl, _this.getParam(), _this.successGetLastBets, _this.completeGetLastBets);
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
        this.completeGetLastBets = function () {
            _this.autoRefreshComponent.isOn() && _this.autoRefreshComponent.start();
        };
        this.autoRefreshComponent = autoRefreshComponent;
        this.monitoringBetsCopyHtml = monitoringBetsCopyHtml;
        this.saveFilterView = saveFilter;
    }
    MonitoringBetsView.prototype.init = function () {
        var _this = this;
        this.localStorage = alphaReports.localStorage;
        this.cacheElements();
        this.registerEvents();
        this.setTransIdCacheKeyByFilter($("#last-trans-id-cache-key-by-filter").val(), $("#latest-trans-id-cache-key-by-filter").val());
        this.setTransId($("#last-trans-id").val(), $("#latest-trans-id").val());
        this.monitoringBetsCopyHtml.init();
        this.saveFilterView.init(this.iconSaveFilter, function () { return _this.getSelectedFilterParams(); });
        this.$betListTableContainer.find('[data-toggle="tooltip"]').tooltip();
        var filterId = this.dropdownFilter.find(":selected").val();
        this.iconSaveFilter.toggleClass("hide", filterId !== "0");
        if (!filterId) {
            this.autoRefreshComponent.disable();
        }
        else if (this.autoRefreshComponent.isOn()) {
            this.autoRefreshComponent.start();
        }
        this.initFeedback();
    };
    MonitoringBetsView.prototype.cacheElements = function () {
        this.$msgError = $("#msg-error");
        this.$betListTableContainer = $("#bet-list-table-container");
        this.getBetsUrl = $("#get-bets-url").val();
        this.iconSaveFilter = $("#icon-save-filter");
    };
    MonitoringBetsView.prototype.registerEvents = function () {
        var _this = this;
        this.registerToggleFilter();
        this.dropdownFilter = $("#monitoring-bets-filter").select2({
            minimumResultsForSearch: -1,
            dropdownAutoWidth: true
        });
        this.dropdownFilter.on("change.select2", function () {
            _this.loadLastBets(true);
            _this.autoRefreshComponent.enable();
            _this.autoRefreshComponent.start();
            var filterId = _this.dropdownFilter.find(":selected").val();
            _this.iconSaveFilter.toggleClass("hide", filterId !== "0");
            Common.Utils.UpdateCurrentUrl(_this.getSelectedFilterParams());
        });
        this.dropdownMostRecentTicket = $("#most-recent-tickets").select2({
            minimumResultsForSearch: -1
        });
        this.dropdownMostRecentTicket.on("change.select2", function () {
            if (_this.dropdownFilter.find(":selected").val()) {
                _this.loadLastBets(true);
                Common.Utils.UpdateCurrentUrl(_this.getSelectedFilterParams());
            }
        });
        this.autoRefreshComponent.init(new AutoRefreshOption($("#hidden-OnText").val(), $("#hidden-OffText").val(), function () { return _this.loadLastBets(false); }, function (onOffStatus) { return Common.Utils.UpdateCurrentUrl(_this.getSelectedFilterParams(onOffStatus)); }));
        $(".navigation").click(function (e) {
            var $this = $(e.target);
            var url = $this.attr("href") + "?" + $.param(_this.getSelectedFilterParams());
            $this.attr("href", url);
        });
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
    MonitoringBetsView.prototype.initFeedback = function () {
        setTimeout(function () {
            var feedbackId = 4;
            var popupHeight = 450;
            if ($("#hidden-IsInternal").val() === "1") {
                var mainDomain = $("#MainDomain").val();
                mainDomain = mainDomain.indexOf("http") === -1
                    ? location.origin.split(":")[0] + ":" + mainDomain + "/site-main"
                    : mainDomain;
                window.top.postMessage({
                    event_id: "winlossdetailnew-feedback",
                    feedback_id: feedbackId,
                    popup_height: popupHeight
                }, mainDomain);
            }
            else {
                window.top.indexManager.initFeedbackFunction(feedbackId, undefined, popupHeight);
            }
        }, 1000);
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
        var selectedFilter = this.dropdownFilter.find(":selected");
        var FilterId = selectedFilter.val();
        var MemberList = selectedFilter.data("members");
        var BetTypeList = selectedFilter.data("bet-types");
        var SportTypeList = selectedFilter.data("sport-types");
        var LeagueList = selectedFilter.data("leagues");
        var EvenStatus = selectedFilter.data("event-status");
        var MinStake = selectedFilter.data("stake");
        var FilterByMembers = selectedFilter.data("filter-by-member");
        var LimitRowCount = this.dropdownMostRecentTicket.find(":selected").val();
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
    MonitoringBetsView.prototype.abortRequest = function () {
        this.ajaxRequest && this.ajaxRequest.abort();
    };
    MonitoringBetsView.prototype.showError = function () {
        this.$msgError.removeClass("hide");
        // protection level, avoid through too many exceptions on server
    };
    MonitoringBetsView.prototype.getSelectedFilterParams = function (autoStart) {
        if (autoStart === void 0) { autoStart = undefined; }
        return {
            filterName: $.trim(this.dropdownFilter.find(":selected").text()),
            limitRowCount: this.dropdownMostRecentTicket.find(":selected").val(),
            autoStart: autoStart || this.autoRefreshComponent.isOn()
        };
    };
    return MonitoringBetsView;
}());
$(function () {
    new MonitoringBetsView(new AutoRefreshComponent(), new MonitoringBetsCopyHtml(), new SaveFilterView()).init();
});
//# sourceMappingURL=monitoring-bets.js.map