var Common;
(function (Common) {
    var PopupDetails = /** @class */ (function () {
        function PopupDetails() {
            var _this = this;
            this.Show = function (url, title, width, height, options) {
                _this.topWindow.popupManager.openMultiple(url, title, width, height, undefined, undefined, undefined, undefined, undefined, undefined, options);
            };
        }
        PopupDetails.Instance = function (topWindow) {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new PopupDetails() : this.instance;
            this.instance.topWindow = topWindow;
            return this.instance;
        };
        return PopupDetails;
    }());
    Common.PopupDetails = PopupDetails;
})(Common || (Common = {}));
//# sourceMappingURL=popup-details.js.map