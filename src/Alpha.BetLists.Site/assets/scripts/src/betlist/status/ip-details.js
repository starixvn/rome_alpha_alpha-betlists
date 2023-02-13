var BetList;
(function (BetList) {
    var Status;
    (function (Status) {
        var IpDetails = /** @class */ (function () {
            function IpDetails() {
                this.ShowIpInfo = function (ip) {
                    Common.PopupDetails.Instance(window.top).Show(Common.AppPath.OldMainAppPath() + '_IPInfo/IpInfo.aspx?ip=' + ip, 'IP Details', 400, 400);
                };
            }
            Object.defineProperty(IpDetails, "Instance", {
                get: function () {
                    this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new IpDetails() : this.instance;
                    return this.instance;
                },
                enumerable: true,
                configurable: true
            });
            return IpDetails;
        }());
        Status.IpDetails = IpDetails;
    })(Status = BetList.Status || (BetList.Status = {}));
})(BetList || (BetList = {}));
var OpenIPInfo;
$(document).ready(function () {
    OpenIPInfo = BetList.Status.IpDetails.Instance.ShowIpInfo;
});
//# sourceMappingURL=ip-details.js.map