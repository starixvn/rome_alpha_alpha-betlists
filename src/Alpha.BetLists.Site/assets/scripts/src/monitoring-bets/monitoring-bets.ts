"use strict";
declare var store: any;

class MonitoringBetsView {
    private dropdownFilter: JQuery;
    private dropdownMostRecentTicket: JQuery;
    private autoRefreshComponent: AutoRefreshComponent;
    private monitoringBetsCopyHtml: MonitoringBetsCopyHtml;

    localStorage: any;
    $pageSize: JQuery;
    $msgError: JQuery;
    $betListTableContainer: JQuery;

    getBetsUrl: string = "";
    isCalling = false;
    pageSize: number = 10;

    timeToGetLastBets: number = 2000;
    timeToRetryGetLastBets: number = 600000;
    getLastBetsTimeOut: number = 0;
    ajaxRequest: JQueryXHR;

    lastTransIdCacheKeyByFilter: string = "";
    latestTransIdCacheKeyByFilter: string = "";

    constructor(
        autoRefreshComponent: AutoRefreshComponent,
        monitoringBetsCopyHtml: MonitoringBetsCopyHtml) {
        this.autoRefreshComponent = autoRefreshComponent;
        this.monitoringBetsCopyHtml = monitoringBetsCopyHtml;
    }

    init() {
        this.localStorage = alphaReports.localStorage;
        
        this.cacheElements();
        this.registerEvents();
        this.setLastTransIdByFilter();
        
        this.autoRefreshComponent.start();
        this.monitoringBetsCopyHtml.init();
    }

    private cacheElements = () => {
        this.$pageSize = $(".page-size");
        this.$msgError = $("#msg-error");
        this.$betListTableContainer = $("#bet-list-table-container");

        this.getBetsUrl = $("#get-bets-url").val();
        this.pageSize = parseInt($(".page-size.active").html());

        this.dropdownFilter = $("#monitoring-bets-filter");

        this.dropdownMostRecentTicket = $("#most-recent-tickets");
    };

    private registerEvents = () => {
        const self = this;

        this.dropdownFilter.multiselect({
            multiple: false,
            maxHeight: 300,
            numberDisplayed: 1,
            includeSelectAllOption: false
        });

        this.dropdownFilter.change(() => {
            this.loadLastBets(true);
        });

        this.dropdownMostRecentTicket.multiselect({
            multiple: false,
            maxHeight: 300,
            numberDisplayed: 1,
            includeSelectAllOption: false
        });

        this.dropdownMostRecentTicket.change(() => {
            this.loadLastBets(true);
        });

        this.autoRefreshComponent.init(new AutoRefreshOption(
            $("#hidden-On").val(),
            $("#hidden-Off").val(),
            () => this.loadLastBets(false)));
    };

    private setLastTransIdByFilter() {
        this.lastTransIdCacheKeyByFilter = $("#last-trans-id-cache-key-by-filter").val();
        this.latestTransIdCacheKeyByFilter = $("#latest-trans-id-cache-key-by-filter").val();
    }

    private loadLastBets = (isAutoLoad: boolean) => {
        if (!isAutoLoad) {
            this.localStorage.set(this.lastTransIdCacheKeyByFilter, 0);
            this.localStorage.set(this.latestTransIdCacheKeyByFilter, 0);
        }

        if (this.isCalling) {
            this.abortRequest();
        }

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

        const param = {
            FilterId,
            TransId: lastTransId ? lastTransId : "0",
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
        return param;
    }


    private abortRequest = () => {
        if (this.ajaxRequest) {
            this.ajaxRequest.abort();
        }
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

        const lastTransId = this.$betListTableContainer.find("#last-trans-id").val();

        this.lastTransIdCacheKeyByFilter = this.$betListTableContainer.find("#new-last-trans-id-by-filter").val();

        if (lastTransId > 0) {
            this.localStorage.set(this.lastTransIdCacheKeyByFilter, lastTransId);
        }

        this.latestTransIdCacheKeyByFilter = this.$betListTableContainer.find("#new-latest-trans-id-by-filter").val();

        const latestTransId = this.$betListTableContainer.find("#latest-trans-id").val();

        this.localStorage.set(this.latestTransIdCacheKeyByFilter, latestTransId);

        this.isCalling = false;
    };

    private showError = () => {
        this.$msgError.removeClass("hide");
        // protection level, avoid through too many exceptions on server
    };

    private completeGetLastBets = () => {
        this.autoRefreshComponent.isOn() && this.autoRefreshComponent.start();
    };
}

$(function () {
    new MonitoringBetsView(
        new AutoRefreshComponent(),
        new MonitoringBetsCopyHtml(Clipboard)).init();
});