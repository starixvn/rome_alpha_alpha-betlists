var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
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
                    enumerable: false,
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
var showDetail;
$(document).ready(function () {
    showDetail = BetList.Choice.TicketDetails.GoldDeluxeTicketDetails.Instance.ShowDetails;
});
//# sourceMappingURL=other-ticket-details.js.map