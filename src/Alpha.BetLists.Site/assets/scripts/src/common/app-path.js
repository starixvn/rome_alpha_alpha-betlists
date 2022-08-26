var Common;
(function (Common) {
    var AppPath = /** @class */ (function () {
        function AppPath() {
        }
        AppPath.OldMainAppPath = function () { return $('#old-main-path').val(); };
        AppPath.BetListAppPath = function () { return $('#betlist-path').val(); };
        AppPath.ResultsAppPath = function () { return $("#results-path").val(); };
        AppPath.TotalBetsForecastAppPath = function () { return $("#totalbets-forecast-path").val(); };
        return AppPath;
    }());
    Common.AppPath = AppPath;
})(Common || (Common = {}));
//# sourceMappingURL=app-path.js.map