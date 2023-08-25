var BetList;
(function (BetList) {
    var Choice;
    (function (Choice) {
        var ScoreMap = /** @class */ (function () {
            function ScoreMap() {
                this.ShowScoreMap = function (matchIds, betType, liveIndicator) {
                    var params = liveIndicator == 1 ?
                        "leagueId=-1&matchIds=" + matchIds + "&bettypes_live=" + betType + "&bettypes_dead=-1" :
                        "leagueId=-1&matchIds=" + matchIds + "&bettypes_live=-1&bettypes_dead=" + betType;
                    Common.PopupDetails.Instance(window.top).Show(Common.AppPath.TotalBetsForecastAppPath() + "ForecastScoreMap/Index?" + params, "", 1008);
                };
            }
            Object.defineProperty(ScoreMap, "Instance", {
                get: function () {
                    this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new ScoreMap() : this.instance;
                    return this.instance;
                },
                enumerable: false,
                configurable: true
            });
            return ScoreMap;
        }());
        Choice.ScoreMap = ScoreMap;
    })(Choice = BetList.Choice || (BetList.Choice = {}));
})(BetList || (BetList = {}));
var OpenScoreMap;
$(document).ready(function () {
    OpenScoreMap = BetList.Choice.ScoreMap.Instance.ShowScoreMap;
});
//# sourceMappingURL=score-map.js.map