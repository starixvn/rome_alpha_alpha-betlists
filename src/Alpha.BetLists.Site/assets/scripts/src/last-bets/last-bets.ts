declare var shortcut: any;
declare var store: any;

"use strict";

namespace BetList {
    export class LastBetsView {
        private static instance: LastBetsView;

        public static get Instance(): LastBetsView {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new LastBetsView() : this.instance;
            return this.instance;
        }

        shortcut: any;
        store: any;

        $pageSize: JQuery;
        $loadingIcon: JQuery;
        $msgError: JQuery;
        $betListTableContainer: JQuery;

        betType: number = 0;
        getBetsUrl: string = "";
        isCalling = false;
        pageSize: number = 10;

        timeToGetLastBets: number = 0;
        timeToRetryGetLastBets: number = 600000;
        getLastBetsTimeOut: number = 0;
        ajaxRequest: JQueryXHR;

        lastTransIdCacheKey: string = "";
        latestTransIdCacheKey: string = "";

        btnAutoRefresh: JQuery;

        public Init = (shortcut: any, store: any) => {
            this.shortcut = shortcut;
            this.store = store;

            this.cacheElements();
            this.registerEvents();

            this.lastTransIdCacheKey = $("#last-trans-id-cache-key").val();
            this.latestTransIdCacheKey = $("#latest-trans-id-cache-key").val();

            if (this.convertAttributeToBool($("#reset-last-trans-id").val())) {
                this.localStorage.set(this.lastTransIdCacheKey, 0);
                this.localStorage.set(this.latestTransIdCacheKey, 0);
            }

            this.loadLastBets(false);
        };

        cacheElements = () => {
            this.$pageSize = $(".page-size");
            this.$loadingIcon = $("#loading-icon");
            this.$msgError = $("#msg-error");
            this.$betListTableContainer = $("#bet-list-table-container");

            this.betType = parseInt($("#bet-type").val(), 0);
            this.getBetsUrl = $("#get-bets-url").val();
            this.pageSize = parseInt($(".page-size.active").html());

            this.btnAutoRefresh = $("#ddlRefresh");
        };

        registerEvents = () => {
            let self = this;

            this.$pageSize.click(function () {
                self.changeLastBetRange($(this));
            });

            $("#refresh-value a")
                .off()
                .click(function () {
                    const seconds = +$(this).data("refresh-value");
                    if (isNaN(seconds) || seconds === 0) {
                        self.timeToGetLastBets = 0;
                        self.stopAutoRefresh(this.innerText)
                    } else {
                        self.timeToGetLastBets = seconds;
                        self.startAutoRefresh();
                    }
                });
        };

        changeLastBetRange = (element: JQuery) => {
            let range = element.html();
            this.pageSize = parseInt(range, 0);

            this.$pageSize.removeClass("active");
            element.addClass("active");

            this.replaceHrefParam("PageSize", this.pageSize);

            this.loadLastBets(false);
        };

        loadLastBets = (isAutoLoad: boolean) => {
            if (!isAutoLoad) {
                this.localStorage.set(this.lastTransIdCacheKey, 0);
                this.localStorage.set(this.latestTransIdCacheKey, 0);
            }

            if (this.isCalling) {
                this.abortRequest();
            }

            this.$loadingIcon.removeClass("hide");

            this.isCalling = true;
            let lastTransId = this.localStorage.get(this.lastTransIdCacheKey);
            let latestTransId = this.localStorage.get(this.latestTransIdCacheKey);

            let param = {
                BetType: this.betType,
                PageSize: this.pageSize,
                IsAutoLoad: isAutoLoad,
                TransId: lastTransId ? lastTransId : "0",
                LatestTransId: latestTransId
            };

            this.ajaxRequest = $.ajax({
                method: "GET",
                data: param,
                url: this.getBetsUrl,
                success: this.successGetLastBets,
                error: this.errorRequest,
                complete: this.completeGetLastBets
            });
        };

        abortRequest = () => {
            if (this.ajaxRequest) {
                this.ajaxRequest.abort();
            }
        };

        errorRequest = (request: any) => {
            if (request.statusText === "abort") {
                return;
            }

            if (request.status === 500 && request.responseText) {
                const jsonData = JSON.parse(request.responseText);
                if (jsonData.UMFrom && jsonData.UMTo) {
                    const cookielessSiteMainUrl = $("#CookielessSiteMainUrl");
                    const siteMainUrl = cookielessSiteMainUrl.length
                        ? cookielessSiteMainUrl.val()
                        : window.top["alpha"].urls.mainRootPath;
                    location.href = `${siteMainUrl}partialum/index?${$.param(jsonData)}`;
                    return;
                }
            }

            this.showError();
        };

        successGetLastBets = (responseHtml: any) => {
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

            let lastTransId = this.$betListTableContainer.find("#last-trans-id").val();

            if (lastTransId > 0) {
                this.localStorage.set(this.lastTransIdCacheKey, lastTransId);
            }

            let latestTransId = this.$betListTableContainer.find("#latest-trans-id").val();
            this.localStorage.set(this.latestTransIdCacheKey, latestTransId);

            this.timeToGetLastBets && this.startAutoRefresh();

            this.isCalling = false;
        };

        showError = () => {
            this.$msgError.removeClass("hide");

            // protection level, avoid through too many exceptions on server
            let self = this;
            this.getLastBetsTimeOut = setTimeout(() => { self.loadLastBets(true); }, this.timeToRetryGetLastBets);
        };
        completeGetLastBets = () => {
            this.$loadingIcon.addClass("hide");
        };

        localStorage = {
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
        isIe = () => {
            var ua = window.navigator.userAgent;
            var msie = ua.indexOf("MSIE ");

            return msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./);
        };

        convertAttributeToBool = (attrValue: string) => {
            return !!attrValue && attrValue.toString().toLowerCase() === "true";
        };
        replaceHrefParam = (paramName: string, paramValue: any) => {
            let self = this;
            let $linkClass = $(".tab-report-type");

            $linkClass.each((index: number, element: Element) => {
                let $link = $(element).find(">a");
                let currentUrl = $link.attr("href");

                currentUrl = self.replaceQueryStringValue(currentUrl, paramName, paramValue);

                $link.attr("href", currentUrl);
            });
        };
        replaceQueryStringValue = (queryString: string, paramName: string, paramValue: any) => {
            if (typeof queryString === "undefined"
                || queryString === null) {
                return "";
            }

            var regex = new RegExp("([?&])" + paramName + "=[^&]*", "i");

            var newQueryString = regex.test(queryString)
                ? queryString.replace(regex, "$1" + paramName + "=" + paramValue)
                : queryString + (queryString.indexOf("?") >= 0 ? "&" : "?") + paramName + "=" + paramValue;

            return newQueryString;
        };
        stopAutoRefresh = (offText: string) => {
            this.clearTimeoutAndSetAutoRefreshText(offText);
        };
        startAutoRefresh = () => {
            this.clearTimeoutAndSetAutoRefreshText(this.timeToGetLastBets);
            this.getLastBetsTimeOut = setTimeout(() => this.countdown(), 1000);
        };
        countdown = () => {
            let value = +this.btnAutoRefresh.text();
            if (value === 1) {
                this.clearTimeoutAndSetAutoRefreshText("...");
                this.loadLastBets(true);
            } else {
                this.btnAutoRefresh.text(--value);
                this.getLastBetsTimeOut = setTimeout(() => this.countdown(), 1000);
            }
        };
        clearTimeoutAndSetAutoRefreshText = (text) => {
            clearTimeout(this.getLastBetsTimeOut);
            this.btnAutoRefresh.text(text);
        };
    }
}

$(document).ready(() => {
    BetList.LastBetsView.Instance.Init(shortcut, store);
});