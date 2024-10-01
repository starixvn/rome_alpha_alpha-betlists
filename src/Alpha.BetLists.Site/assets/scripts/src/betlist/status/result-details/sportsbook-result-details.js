var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var BetList;
(function (BetList) {
    var Status;
    (function (Status) {
        var ResultDetails;
        (function (ResultDetails) {
            var SportsbookResultDetails = /** @class */ (function (_super) {
                __extends(SportsbookResultDetails, _super);
                function SportsbookResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId, transdesc) {
                        var params = '?matchid=' + matchId
                            + '&bettypeid=' + betTypeId
                            + '&race=' + race
                            + '&sportId=' + sportTypeId
                            + '&leagueId=' + league
                            + '&isOutright=' + isOutright
                            + '&refnum=' + refNo
                            + '&betid=' + betId
                            + '&transdesc=' + transdesc
                            + '&isViewResultFromBetList=true';
                        var baseballSport = 8;
                        var width = sportTypeId == baseballSport ? 1000 : 730;
                        Common.PopupDetails.Instance(window.top).Show(_this.ResultDetailsPath + params, "", width, 250);
                    };
                    return _this;
                }
                return SportsbookResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.SportsbookResultDetails = SportsbookResultDetails;
            var MixParlayResultDetails = /** @class */ (function (_super) {
                __extends(MixParlayResultDetails, _super);
                function MixParlayResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId) {
                        var params = '?refnum=' + refNoMixParlay
                            + '&wldate=' + winLostDate
                            + '&bettypeid=' + betTypeId
                            + '&matchid=1&isViewResultFromBetList=true';
                        Common.PopupDetails.Instance(window.top).Show(_this.ResultDetailsPath + params, "", 800, 500);
                    };
                    return _this;
                }
                return MixParlayResultDetails;
            }(SportsbookResultDetails));
            ResultDetails.MixParlayResultDetails = MixParlayResultDetails;
            var FastMarketsResultDetails = /** @class */ (function (_super) {
                __extends(FastMarketsResultDetails, _super);
                function FastMarketsResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId, hdp2) {
                        var params = '?matchid=' + matchId
                            + '&bettypeid=' + betTypeId
                            + '&race=' + race
                            + '&sportId=' + sportTypeId
                            + '&leagueId=' + league
                            + '&isOutright=' + isOutright
                            + '&refnum=' + refNo
                            + '&betid=' + betId
                            + '&isViewResultFromBetList=true'
                            + '&hdp2=' + hdp2;
                        Common.PopupDetails.Instance(window.top).Show(_this.ResultDetailsPath + params, "", 730, 250);
                    };
                    return _this;
                }
                return FastMarketsResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.FastMarketsResultDetails = FastMarketsResultDetails;
        })(ResultDetails = Status.ResultDetails || (Status.ResultDetails = {}));
    })(Status = BetList.Status || (BetList.Status = {}));
})(BetList || (BetList = {}));
//# sourceMappingURL=sportsbook-result-details.js.map