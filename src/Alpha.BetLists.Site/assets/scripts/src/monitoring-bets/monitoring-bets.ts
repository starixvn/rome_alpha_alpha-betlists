"use strict";
declare var store: any;

class MonitoringBetsView {
    private dropdownFilter: JQuery;
    private dropdownMostRecentTicket: JQuery;
    private autoRefreshComponent: AutoRefreshComponent;
    private monitoringBetsCopyHtml: MonitoringBetsCopyHtml;

    store: any;
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
        this.store = store;

        this.cacheElements();
        this.registerEvents();
        this.setLastTransIdByFilter();

        this.loadLastBets(false);

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
            nonSelectedText: 'Select...',
            numberDisplayed: 1,
            includeSelectAllOption: false
        });

        this.dropdownFilter.change(() => {
            this.loadLastBets(false);
        });

        this.dropdownMostRecentTicket.multiselect({
            multiple: false,
            maxHeight: 300,
            numberDisplayed: 1,
            includeSelectAllOption: false
        });

        this.dropdownMostRecentTicket.change(() => {
            this.loadLastBets(false);
        });

        this.autoRefreshComponent.init(new AutoRefreshOption(
            "ON",
            "OFF",
            () => {
                setTimeout(() => {
                    console.log('get data from server');
                    this.loadLastBets(false);

                }, 3000);
            }));
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
        this.ajaxRequest = $.ajax({
            method: "GET",
            data: this.getParam(),
            url: this.getBetsUrl,
            success: this.successGetLastBets,
            error: this.errorRequest,
            complete: this.completeGetLastBets
        });
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
        const FilterByMember = selectedFilter.data("filter-by-member");
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
            FilterByMember,
            LatestTransId: latestTransId
        };
        return param;
    }


    private abortRequest = () => {
        if (this.ajaxRequest) {
            this.ajaxRequest.abort();
        }
    };

    private errorRequest = (request: any) => {
        if (request.statusText === "abort") {
            return;
        }

        if (request.status === 500 && request.responseText) {
            const jsonData = JSON.parse(request.responseText);
            if (jsonData.UMFrom && jsonData.UMTo) {
                location.href = top["alpha"].urls.mainRootPath
                    + "PartialUM/Index?umfrom=" + jsonData.UMFrom
                    + "&umto=" + jsonData.UMTo;
                return;
            }
        }

        this.showError();
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
        if (this.autoRefreshComponent.labelStatusText.text() == this.autoRefreshComponent.autoRefreshOption.onText)
            this.autoRefreshComponent.start();
    };

    private localStorage = {
        get: (key: string) => {
            return this.store.enabled ? this.store.get(key) : $.cookie(key);
        },

        set: (key: string, value: any) => {
            this.store.enabled ? this.store.set(key, value) : $.cookie(key, value, this.localStorage.getCookieExpireOptions());
        },

        getCookieExpireOptions: () => {
            var cookieExpire: any = { expires: 365, path: window.location.pathname };

            if (this.isIe()) {
                cookieExpire = { expires: 365 };
            }

            return cookieExpire;
        }
    };

    private isIe = () => {
        var ua = window.navigator.userAgent;
        var msie = ua.indexOf("MSIE ");

        return msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./);
    };
}

$(function () {
    new MonitoringBetsView(
        new AutoRefreshComponent(),
        new MonitoringBetsCopyHtml(Clipboard)).init();
});