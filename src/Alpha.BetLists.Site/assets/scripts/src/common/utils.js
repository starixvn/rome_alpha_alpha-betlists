var Common;
(function (Common) {
    var Utils = /** @class */ (function () {
        function Utils() {
        }
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