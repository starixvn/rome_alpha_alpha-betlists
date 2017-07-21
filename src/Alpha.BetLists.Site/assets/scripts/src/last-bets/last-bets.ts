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
        $chbSwitch: JQuery;
        $lblSwitch: JQuery;
        $isAutoRefresh: JQuery;

        betType: number = 0;
        getBetsUrl: string = "";
        isCalling = false;
        pageSize: number = 10;

        timeToGetLastBets: number = 2000;
        timeToRetryGetLastBets: number = 600000;
        getLastBetsTimeOut: number = 0;
        ajaxRequest: JQueryXHR;

        lastTransIdCacheKey: string = "";

        public Init = (shortcut: any, store: any) => {
            this.shortcut = shortcut;
            this.store = store;

            this.cacheElements();
            this.registerEvents();

            this.lastTransIdCacheKey = $("#last-trans-id-cache-key").val();
            if (this.convertAttributeToBool($("#reset-last-trans-id").val())) {
                this.localStorage.set(this.lastTransIdCacheKey, 0);
            }

            this.loadLastBets(false);
        };

        cacheElements = () => {
            this.$pageSize = $(".page-size");
            this.$loadingIcon = $("#loading-icon");
            this.$msgError = $("#msg-error");
            this.$betListTableContainer = $("#bet-list-table-container");
            this.$chbSwitch = $("#chb-switch");
            this.$lblSwitch = $("#lbl-switch");

            this.$isAutoRefresh = $("#is-auto-refresh");

            this.betType = parseInt($("#bet-type").val(), 0);
            this.getBetsUrl = $("#get-bets-url").val();
        };

        registerEvents = () => {
            let self = this;

            this.$pageSize.click(function () {
                self.changeLastBetRange($(this));
            });

            this.shortcut.add("Shift+P", this.setGetLastBetsTimeOutByShortcut);

            this.$chbSwitch.off("click").on("click", this.setGetLastBetsTimeOut);
            this.$lblSwitch.off("click").on("click", () => {
                self.$chbSwitch.click();
            });
        };

        changeLastBetRange = (element: JQuery) => {
            let range = element.html();
            this.pageSize = parseInt(range, 0);

            this.$pageSize.removeClass("active");
            element.addClass("active");

            if (this.getLastBetsTimeOut !== 0) {
                clearTimeout(this.getLastBetsTimeOut);
            }

            this.loadLastBets(false);
        };

        loadLastBets = (isAutoLoad: boolean) => {
            this.$loadingIcon.removeClass("hide");

            if (this.isCalling) {
                this.abortRequest();
            }

            this.isCalling = true;

            let lastTransId = this.localStorage.get(this.lastTransIdCacheKey);

            let param = {
                BetType: this.betType,
                PageSize: this.pageSize,
                IsAutoLoad: isAutoLoad,
                TransId: lastTransId ? lastTransId - 1 : 0
            };

            this.ajaxRequest = $.ajax({
                method: "GET",
                data: param,
                url: this.getBetsUrl,
                success: this.successGetLastBets,
                error: this.showError,
                complete: this.completeGetLastBets
            });
        };

        abortRequest = () => {
            if (this.ajaxRequest) {
                this.ajaxRequest.abort();
            }
        };

        successGetLastBets = (responseHtml: any) => {
            if (responseHtml.ErrorCode && responseHtml.ErrorCode !== "0") {
                this.showError();
                return;
            }

            this.$msgError.addClass("hide");

            if (!this.isCalling) {
                return;
            }

            this.$betListTableContainer.html(responseHtml);

            let lastTransId = parseInt(this.$betListTableContainer.find("#last-trans-id").val(), 0);
            if (lastTransId > 0) {
                this.localStorage.set(this.lastTransIdCacheKey, lastTransId);
            }

            if (this.convertAttributeToBool(this.$isAutoRefresh.val())) {
                let self = this;
                this.getLastBetsTimeOut = setTimeout(() => { self.loadLastBets(true); }, this.timeToGetLastBets);
            }

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

        setGetLastBetsTimeOutByShortcut = () => {
            this.$lblSwitch.click();
        };

        setGetLastBetsTimeOut = () => {
            if (this.getLastBetsTimeOut !== 0) {
                clearTimeout(this.getLastBetsTimeOut);
                this.getLastBetsTimeOut = 0;
                this.$isAutoRefresh.val("false");
                this.replaceHrefParam("IsAutoRefresh", "false");
            } else {
                let self = this;
                this.getLastBetsTimeOut = setTimeout(() => { self.loadLastBets(true); }, this.timeToGetLastBets);
                this.$isAutoRefresh.val("true");
                this.replaceHrefParam("IsAutoRefresh", "true");
            }
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
    }
}

$(document).ready(() => {
    BetList.LastBetsView.Instance.Init(shortcut, store);
});