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
                    _this.topWindow.postMessage({
                        event_id: "betlist-result",
                        title: title,
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