"use strict";
var BetList;
(function (BetList) {
    var LastBetsView = /** @class */ (function () {
        function LastBetsView() {
            var _this = this;
            this.betType = 0;
            this.getBetsUrl = "";
            this.isCalling = false;
            this.pageSize = 10;
            this.timeToGetLastBets = 2000;
            this.timeToRetryGetLastBets = 600000;
            this.getLastBetsTimeOut = 0;
            this.lastTransIdCacheKey = "";
            this.latestTransIdCacheKey = "";
            this.Init = function (shortcut, store) {
                _this.shortcut = shortcut;
                _this.store = store;
                _this.cacheElements();
                _this.registerEvents();
                _this.lastTransIdCacheKey = $("#last-trans-id-cache-key").val();
                _this.latestTransIdCacheKey = $("#latest-trans-id-cache-key").val();
                if (_this.convertAttributeToBool($("#reset-last-trans-id").val())) {
                    _this.localStorage.set(_this.lastTransIdCacheKey, 0);
                    _this.localStorage.set(_this.latestTransIdCacheKey, 0);
                }
                _this.loadLastBets(false);
            };
            this.cacheElements = function () {
                _this.$pageSize = $(".page-size");
                _this.$loadingIcon = $("#loading-icon");
                _this.$msgError = $("#msg-error");
                _this.$betListTableContainer = $("#bet-list-table-container");
                _this.$chbSwitch = $("#chb-switch");
                _this.$lblSwitch = $("#lbl-switch");
                _this.$isAutoRefresh = $("#is-auto-refresh");
                _this.betType = parseInt($("#bet-type").val(), 0);
                _this.getBetsUrl = $("#get-bets-url").val();
                _this.pageSize = parseInt($(".page-size.active").html());
            };
            this.registerEvents = function () {
                var self = _this;
                _this.$pageSize.click(function () {
                    self.changeLastBetRange($(this));
                });
                _this.shortcut.add("Shift+P", _this.setGetLastBetsTimeOutByShortcut);
                _this.$chbSwitch.off("click").on("click", _this.setGetLastBetsTimeOut);
                _this.$lblSwitch.off("click").on("click", function () {
                    self.$chbSwitch.click();
                });
            };
            this.changeLastBetRange = function (element) {
                var range = element.html();
                _this.pageSize = parseInt(range, 0);
                _this.$pageSize.removeClass("active");
                element.addClass("active");
                _this.replaceHrefParam("PageSize", _this.pageSize);
                if (_this.getLastBetsTimeOut !== 0) {
                    clearTimeout(_this.getLastBetsTimeOut);
                }
                _this.loadLastBets(false);
            };
            this.loadLastBets = function (isAutoLoad) {
                if (!isAutoLoad) {
                    _this.localStorage.set(_this.lastTransIdCacheKey, 0);
                    _this.localStorage.set(_this.latestTransIdCacheKey, 0);
                }
                if (_this.isCalling) {
                    _this.abortRequest();
                }
                _this.$loadingIcon.removeClass("hide");
                _this.isCalling = true;
                var lastTransId = _this.localStorage.get(_this.lastTransIdCacheKey);
                var latestTransId = _this.localStorage.get(_this.latestTransIdCacheKey);
                var param = {
                    BetType: _this.betType,
                    PageSize: _this.pageSize,
                    IsAutoLoad: isAutoLoad,
                    TransId: lastTransId ? lastTransId : "0",
                    LatestTransId: latestTransId
                };
                _this.ajaxRequest = $.ajax({
                    method: "GET",
                    data: param,
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
                        var cookielessSiteMainUrl = $("#CookielessSiteMainUrl");
                        var siteMainUrl = cookielessSiteMainUrl.length
                            ? cookielessSiteMainUrl.val()
                            : window.top["alpha"].urls.mainRootPath;
                        location.href = siteMainUrl + "partialum/index?" + $.param(jsonData);
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
                if (lastTransId > 0) {
                    _this.localStorage.set(_this.lastTransIdCacheKey, lastTransId);
                }
                var latestTransId = _this.$betListTableContainer.find("#latest-trans-id").val();
                _this.localStorage.set(_this.latestTransIdCacheKey, latestTransId);
                if (_this.convertAttributeToBool(_this.$isAutoRefresh.val())) {
                    var self_1 = _this;
                    _this.getLastBetsTimeOut = setTimeout(function () { self_1.loadLastBets(true); }, _this.timeToGetLastBets);
                }
                _this.isCalling = false;
            };
            this.showError = function () {
                _this.$msgError.removeClass("hide");
                // protection level, avoid through too many exceptions on server
                var self = _this;
                _this.getLastBetsTimeOut = setTimeout(function () { self.loadLastBets(true); }, _this.timeToRetryGetLastBets);
            };
            this.completeGetLastBets = function () {
                _this.$loadingIcon.addClass("hide");
            };
            this.setGetLastBetsTimeOutByShortcut = function () {
                _this.$lblSwitch.click();
            };
            this.setGetLastBetsTimeOut = function () {
                if (_this.getLastBetsTimeOut !== 0) {
                    clearTimeout(_this.getLastBetsTimeOut);
                    _this.getLastBetsTimeOut = 0;
                    _this.$isAutoRefresh.val("false");
                    _this.replaceHrefParam("IsAutoRefresh", "false");
                }
                else {
                    var self_2 = _this;
                    _this.getLastBetsTimeOut = setTimeout(function () { self_2.loadLastBets(true); }, _this.timeToGetLastBets);
                    _this.$isAutoRefresh.val("true");
                    _this.replaceHrefParam("IsAutoRefresh", "true");
                }
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
            this.convertAttributeToBool = function (attrValue) {
                return !!attrValue && attrValue.toString().toLowerCase() === "true";
            };
            this.replaceHrefParam = function (paramName, paramValue) {
                var self = _this;
                var $linkClass = $(".tab-report-type");
                $linkClass.each(function (index, element) {
                    var $link = $(element).find(">a");
                    var currentUrl = $link.attr("href");
                    currentUrl = self.replaceQueryStringValue(currentUrl, paramName, paramValue);
                    $link.attr("href", currentUrl);
                });
            };
            this.replaceQueryStringValue = function (queryString, paramName, paramValue) {
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
        Object.defineProperty(LastBetsView, "Instance", {
            get: function () {
                this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new LastBetsView() : this.instance;
                return this.instance;
            },
            enumerable: true,
            configurable: true
        });
        return LastBetsView;
    }());
    BetList.LastBetsView = LastBetsView;
})(BetList || (BetList = {}));
$(document).ready(function () {
    BetList.LastBetsView.Instance.Init(shortcut, store);
});
//# sourceMappingURL=last-bets.js.map