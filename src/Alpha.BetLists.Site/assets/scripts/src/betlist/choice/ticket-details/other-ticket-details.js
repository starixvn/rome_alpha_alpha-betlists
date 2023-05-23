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
    var Choice;
    (function (Choice) {
        var TicketDetails;
        (function (TicketDetails) {
            var NG2TicketDetails = /** @class */ (function (_super) {
                __extends(NG2TicketDetails, _super);
                function NG2TicketDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ShowNG2BetDetails = function (transId, refNo, custId) {
                        var params = "?refNo=" + refNo;
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.ResultsAppPath() + 'ng2/ParlayDetail' + params, "", 600, 350);
                    };
                    return _this;
                }
                Object.defineProperty(NG2TicketDetails, "Instance", {
                    get: function () {
                        this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new NG2TicketDetails() : this.instance;
                        return this.instance;
                    },
                    enumerable: true,
                    configurable: true
                });
                return NG2TicketDetails;
            }(TicketDetails.BaseTicketDetails));
            TicketDetails.NG2TicketDetails = NG2TicketDetails;
            var GoldDeluxeTicketDetails = /** @class */ (function (_super) {
                __extends(GoldDeluxeTicketDetails, _super);
                function GoldDeluxeTicketDetails() {
                    return _super !== null && _super.apply(this, arguments) || this;
                }
                Object.defineProperty(GoldDeluxeTicketDetails, "Instance", {
                    get: function () {
                        this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new GoldDeluxeTicketDetails() : this.instance;
                        return this.instance;
                    },
                    enumerable: true,
                    configurable: true
                });
                GoldDeluxeTicketDetails.prototype.ShowDetails = function (id) {
                    var element = document.getElementById(id);
                    if (element.style.display === "block") {
                        element.style.display = "none";
                    }
                    else {
                        element.style.display = "block";
                    }
                };
                return GoldDeluxeTicketDetails;
            }(TicketDetails.BaseTicketDetails));
            TicketDetails.GoldDeluxeTicketDetails = GoldDeluxeTicketDetails;
        })(TicketDetails = Choice.TicketDetails || (Choice.TicketDetails = {}));
    })(Choice = BetList.Choice || (BetList.Choice = {}));
})(BetList || (BetList = {}));
var showNG2Details;
var showDetail;
$(document).ready(function () {
    showNG2Details = BetList.Choice.TicketDetails.NG2TicketDetails.Instance.ShowNG2BetDetails;
    showDetail = BetList.Choice.TicketDetails.GoldDeluxeTicketDetails.Instance.ShowDetails;
});
//# sourceMappingURL=other-ticket-details.js.map