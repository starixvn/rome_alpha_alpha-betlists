var BetList;
(function (BetList) {
    var Choice;
    (function (Choice) {
        var TicketDetails;
        (function (TicketDetails) {
            var BaseTicketDetails = /** @class */ (function () {
                function BaseTicketDetails() {
                    this.ViewerId = function () { return $('#viewer-id').val(); };
                    this.ViewerLevelId = function () { return $('#viewer-level-id').val(); };
                }
                return BaseTicketDetails;
            }());
            TicketDetails.BaseTicketDetails = BaseTicketDetails;
        })(TicketDetails = Choice.TicketDetails || (Choice.TicketDetails = {}));
    })(Choice = BetList.Choice || (BetList.Choice = {}));
})(BetList || (BetList = {}));
//# sourceMappingURL=base-ticket-details.js.map