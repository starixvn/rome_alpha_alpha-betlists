var Common;
(function (Common) {
    var PopupDetails = /** @class */ (function () {
        function PopupDetails() {
            var _this = this;
            this.Show = function (url, title, width, height, options) {
                if (!_this.mainDomain) {
                    _this.topWindow.popupManager.openMultiple(url, title, width, height, undefined, undefined, undefined, undefined, undefined, undefined, options);
                }
                else {
                    _this.mainDomain = _this.mainDomain.indexOf('http') === -1 ?
                        location.origin.split(':')[0] + ':' + _this.mainDomain + '/site-main' : _this.mainDomain;
                    if (!title) {
                        var queryStrings = url.indexOf('?') !== -1 ?
                            url.split('&') : [''];
                        for (var i in queryStrings) {
                            if (queryStrings[i].toLowerCase().indexOf('parlay') !== -1
                                && url.toLowerCase().indexOf('betteam=3') !== -1) {
                                title = "System Parlay";
                                break;
                            }
                            if (queryStrings[i].toLowerCase().indexOf('parlay') !== -1
                                && url.toLowerCase().indexOf('betteam=1') !== -1) {
                                title = "Mix Parlay";
                                break;
                            }
                            if (queryStrings[i].toLowerCase().indexOf('cashout') !== -1) {
                                var transId = "";
                                for (var j in queryStrings) {
                                    if (queryStrings[j].toLowerCase().indexOf("transid") !== -1) {
                                        transId = queryStrings[j].split('=')[1];
                                        break;
                                    }
                                }
                                title = "Cash Out History - " + transId;
                                break;
                            }
                            if (queryStrings[i].toLowerCase().indexOf('sportname') !== -1) {
                                title = queryStrings[i].split('=')[1] + ' Details';
                                break;
                            }
                        }
                    }
                    _this.topWindow.postMessage({
                        event_id: "betlist-result",
                        title: title,
                        width: width,
                        height: height,
                        options: options,
                        url: (_this.IsCallingOtherSites(url) ? $("#MainDomain").val() : _this.betlistUrl) + url
                    }, _this.mainDomain);
                }
            };
            this.IsCallingOtherSites = function (url) {
                return url.indexOf('/site-results/') > -1 || url.indexOf('/ex-main/') > -1 || url.indexOf('/ex-totalbetsforecast/') > -1;
            };
        }
        PopupDetails.Instance = function (topWindow) {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new PopupDetails() : this.instance;
            this.instance.topWindow = topWindow;
            this.instance.mainDomain = $("#MainDomain").val();
            this.instance.betlistUrl = $("#BetListUrl").val();
            return this.instance;
        };
        return PopupDetails;
    }());
    Common.PopupDetails = PopupDetails;
})(Common || (Common = {}));
//# sourceMappingURL=popup-details.js.map