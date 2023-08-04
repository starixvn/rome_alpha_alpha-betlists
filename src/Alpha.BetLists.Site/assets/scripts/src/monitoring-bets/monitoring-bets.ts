"use strict";
declare var store: any;

class MonitoringBetsView {
    private iconSaveFilter: JQuery;
    private dropdownFilter: JQuery;
    private dropdownMostRecentTicket: JQuery;
    private autoRefreshComponent: AutoRefreshComponent;
    private monitoringBetsCopyHtml: MonitoringBetsCopyHtml;
    private saveFilterView: SaveFilterView;

    localStorage: any;
    $msgError: JQuery;
    $betListTableContainer: JQuery;

    getBetsUrl = "";
    isCalling = false;
    ajaxRequest: JQueryXHR;

    lastTransIdCacheKeyByFilter = "";
    latestTransIdCacheKeyByFilter = "";

    constructor(
        autoRefreshComponent: AutoRefreshComponent,
        monitoringBetsCopyHtml: MonitoringBetsCopyHtml,
        saveFilter: SaveFilterView) {
        this.autoRefreshComponent = autoRefreshComponent;
        this.monitoringBetsCopyHtml = monitoringBetsCopyHtml;
        this.saveFilterView = saveFilter;
    }

    init() {
        this.localStorage = alphaReports.localStorage;
        
        this.cacheElements();
        this.registerEvents();
        this.setTransIdCacheKeyByFilter($("#last-trans-id-cache-key-by-filter").val(), $("#latest-trans-id-cache-key-by-filter").val());
        this.setTransId($("#last-trans-id").val(), $("#latest-trans-id").val());
        
        this.monitoringBetsCopyHtml.init();
        this.saveFilterView.init(this.iconSaveFilter, () => this.getSelectedFilterParams());
        this.$betListTableContainer.find('[data-toggle="tooltip"]').tooltip();

        const filterId = this.dropdownFilter.find("option:selected:first").val();
        this.iconSaveFilter.toggleClass("hide", filterId !== "0");
        if (filterId === "-1") {
            this.autoRefreshComponent.disable();
        } else if (this.autoRefreshComponent.isOn()) {
            this.autoRefreshComponent.start();
        }
    }

    private cacheElements = () => {
        this.$msgError = $("#msg-error");
        this.$betListTableContainer = $("#bet-list-table-container");
        this.getBetsUrl = $("#get-bets-url").val();

        this.iconSaveFilter = $("#icon-save-filter");
        this.dropdownFilter = $("#monitoring-bets-filter");
        this.dropdownMostRecentTicket = $("#most-recent-tickets");
    };

    private registerEvents = () => {
        this.registerToggleFilter();

        this.dropdownFilter.multiselect({
            maxHeight: 300,
            onChange: (selectedOption) => {
                const filterId = selectedOption.val();
                if (filterId === "-1") {
                    this.autoRefreshComponent.stop();
                    this.autoRefreshComponent.disable();
                } else {
                    this.loadLastBets(true)
                    this.autoRefreshComponent.enable();
                }

                this.iconSaveFilter.toggleClass("hide", filterId !== "0");
                Common.Utils.UpdateCurrentUrl(this.getSelectedFilterParams());
            }
        });

        this.dropdownMostRecentTicket.multiselect({
            onChange: () => {
                if (this.dropdownFilter.find("option:selected:first").val() !== "-1") {
                    this.loadLastBets(true);
                    Common.Utils.UpdateCurrentUrl(this.getSelectedFilterParams());
                }
            }
        });

        this.autoRefreshComponent.init(new AutoRefreshOption(
            $("#hidden-OnText").val(),
            $("#hidden-OffText").val(),
            () => this.loadLastBets(false),
            (onOffStatus) => Common.Utils.UpdateCurrentUrl(this.getSelectedFilterParams(onOffStatus))));

        $(".navigation").click((e) => {
            const $this = $(e.target);
            const url = `${$this.attr("href")}?${$.param(this.getSelectedFilterParams())}`;
            $this.attr("href", url);
        });
    };

    private registerToggleFilter() {
        const filterContainer = $("#form-monitoring-bets");
        const iconFilter = $("#icon-filter").click(function () {
            const isHideFilter = iconFilter.hasClass("icon-arrow-up-drop-circle");
            if (isHideFilter) {
                iconFilter
                    .removeClass("icon-arrow-up-drop-circle")
                    .addClass("icon-arrow-down-drop-circle")
                    .attr("title", $("#hidden-ShowFilterText").val());
                filterContainer.addClass("hide");
            } else {
                iconFilter
                    .removeClass("icon-arrow-down-drop-circle")
                    .addClass("icon-arrow-up-drop-circle")
                    .attr("title", $("#hidden-HideFilterText").val());
                filterContainer.removeClass("hide");
            }
        });
    }

    private setTransIdCacheKeyByFilter(lastTransId, lastestTransId) {
        this.lastTransIdCacheKeyByFilter = lastTransId;
        this.latestTransIdCacheKeyByFilter = lastestTransId;
    }

    private setTransId(lastTransId, lastestTransId) {
        this.localStorage.set(this.lastTransIdCacheKeyByFilter, lastTransId);
        this.localStorage.set(this.latestTransIdCacheKeyByFilter, lastestTransId);
    }

    private loadLastBets = (isResetTransId: boolean) => {
        if (isResetTransId) {
            this.setTransId(0, 0);
        }

        this.isCalling && this.abortRequest();

        this.isCalling = true;
        this.ajaxRequest = AJAXHelper.post(this.getBetsUrl, this.getParam(), this.successGetLastBets, this.completeGetLastBets);
    };

    private getParam() {
        let lastTransId = this.localStorage.get(this.lastTransIdCacheKeyByFilter);
        let latestTransId = this.localStorage.get(this.latestTransIdCacheKeyByFilter);
        const selectedFilter = this.dropdownFilter.find("option:selected:first");

        const FilterId = selectedFilter.val();
        const MemberList = selectedFilter.data("members");
        const BetTypeList = selectedFilter.data("bet-types");
        const SportTypeList = selectedFilter.data("sport-types");
        const LeagueList = selectedFilter.data("leagues");
        const EvenStatus = selectedFilter.data("event-status");
        const MinStake = selectedFilter.data("stake");
        const FilterByMembers = selectedFilter.data("filter-by-member");
        const LimitRowCount = this.dropdownMostRecentTicket.find("option:selected:first").val();

        return {
            FilterId,
            TransId: lastTransId ? lastTransId : 0,
            MemberList,
            BetTypeList,
            SportTypeList,
            LeagueList,
            EvenStatus,
            MinStake,
            LimitRowCount,
            FilterByMembers,
            LatestTransId: latestTransId
        };
    }

    private abortRequest = () => {
        this.ajaxRequest && this.ajaxRequest.abort();
    };

    private successGetLastBets = (responseHtml: any) => {
        if ((responseHtml.ErrorCode && responseHtml.ErrorCode !== "0") ||
            (responseHtml.ErrCode && responseHtml.ErrCode !== "0")) {
            this.showError();
            return;
        }

        this.$msgError.addClass("hide");

        if (!this.isCalling) {
            return;
        }

        this.$betListTableContainer.html(responseHtml);
        this.$betListTableContainer.find('[data-toggle="tooltip"]').tooltip();

        const lastTransIdCacheKeyByFilter = this.$betListTableContainer.find("#new-last-trans-id-by-filter").val();
        const latestTransIdCacheKeyByFilter = this.$betListTableContainer.find("#new-latest-trans-id-by-filter").val();
        this.setTransIdCacheKeyByFilter(lastTransIdCacheKeyByFilter, latestTransIdCacheKeyByFilter)

        const lastTransId = this.$betListTableContainer.find("#last-trans-id").val();
        const latestTransId = this.$betListTableContainer.find("#latest-trans-id").val();
        this.setTransId(lastTransId, latestTransId);

        this.isCalling = false;
    };

    private showError = () => {
        this.$msgError.removeClass("hide");
        // protection level, avoid through too many exceptions on server
    };

    private completeGetLastBets = () => {
        this.autoRefreshComponent.isOn() && this.autoRefreshComponent.start();
    };

    private getSelectedFilterParams(autoStart = undefined) {
        return {
            filterName: $.trim(this.dropdownFilter.find("option:selected:first").text()),
            limitRowCount: this.dropdownMostRecentTicket.find("option:selected:first").val(),
            autoStart: autoStart || this.autoRefreshComponent.isOn()
        }
    }
}

$(function () {
    new MonitoringBetsView(
        new AutoRefreshComponent(),
        new MonitoringBetsCopyHtml(),
        new SaveFilterView()).init();
});