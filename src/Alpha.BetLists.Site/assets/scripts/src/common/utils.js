var Common;
(function (Common) {
    var Utils = /** @class */ (function () {
        function Utils() {
        }
        Utils.ReloadPageAfterTimeout = function (url, timeout) {
            if (url === void 0) { url = undefined; }
            if (timeout === void 0) { timeout = 3000; }
            setTimeout(function () {
                if (url) {
                    window.location.href = url;
                }
                else {
                    window.location.reload();
                }
            }, timeout);
        };
        Utils.IsNullOrUndefinedOrEmpty = function (value) { return value === null || value === undefined || value === ''; };
        Utils.ParseBoolean = function (value) {
            if (!Utils.IsNullOrUndefinedOrEmpty(value)) {
                return value.toLowerCase() === "true";
            }
            return false;
        };
        return Utils;
    }());
    Common.Utils = Utils;
})(Common || (Common = {}));
//# sourceMappingURL=utils.js.map