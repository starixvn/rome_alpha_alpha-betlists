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