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
            var NG2ResultDetails = /** @class */ (function (_super) {
                __extends(NG2ResultDetails, _super);
                function NG2ResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId) {
                        var params = '?gambleId=' + matchId;
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ng2/TicketDetail' + params, "", 500, 400);
                    };
                    return _this;
                }
                return NG2ResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.NG2ResultDetails = NG2ResultDetails;
            var LeapGamingResultDetails = /** @class */ (function (_super) {
                __extends(LeapGamingResultDetails, _super);
                function LeapGamingResultDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewResultDetails = function (matchId, race, betTypeId, sportTypeId, refNo, userName, winLostDate, refNoMixParlay, league, isOutright, betId, betCheck, custId, transDesc) {
                        var params = '?custId=' + _this.CustId +
                            '&roleId=' + _this.ViewerLevelId +
                            '&sportId=' + sportTypeId +
                            '&transDesc=' + transDesc +
                            '&productId=49';
                        Common.PopupDetails.Instance(window.top).Show(_this.ResultDetailsPath + params, "", 835, 675);
                    };
                    return _this;
                }
                return LeapGamingResultDetails;
            }(ResultDetails.BaseResultDetails));
            ResultDetails.LeapGamingResultDetails = LeapGamingResultDetails;
        })(ResultDetails = Status.ResultDetails || (Status.ResultDetails = {}));
    })(Status = BetList.Status || (BetList.Status = {}));
})(BetList || (BetList = {}));
//# sourceMappingURL=other-result-details.js.map