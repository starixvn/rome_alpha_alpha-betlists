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
            var SportsbookTicketDetails = /** @class */ (function (_super) {
                __extends(SportsbookTicketDetails, _super);
                function SportsbookTicketDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ShowSingleMatchParlayDetail = function (transId, custId, status, transIndex) {
                        var params = "&TransactionId=" + transId
                            + "&CustomerId=" + custId
                            + "&Status=" + status;
                        var url = Common.AppPath.BetListAppPath() + 'BetBuilder/ViewDetail?' + params;
                        _this.showSingleMatchParlyTickets(transId, url, transIndex);
                    };
                    _this.showSingleMatchParlyTickets = function (transId, url, transIndex) {
                        var ticketsContent = $('#divEvent_' + transId + transIndex);
                        if (ticketsContent.html() === '') {
                            var getSingleMatchParlaySetting = {
                                method: 'GET',
                                url: url,
                                beforeSend: function () {
                                    var blockIconContent = '<span class="block-icon-small"></span>';
                                    ticketsContent.html(blockIconContent);
                                    ticketsContent.show();
                                },
                                success: function (responseHtml) {
                                    ticketsContent.html(responseHtml);
                                },
                            };
                            $.ajax(getSingleMatchParlaySetting);
                        }
                        else {
                            if (ticketsContent.is(':visible')) {
                                ticketsContent.hide();
                            }
                            else {
                                ticketsContent.show();
                            }
                        }
                    };
                    return _this;
                }
                Object.defineProperty(SportsbookTicketDetails, "Instance", {
                    get: function () {
                        this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new SportsbookTicketDetails() : this.instance;
                        return this.instance;
                    },
                    enumerable: true,
                    configurable: true
                });
                return SportsbookTicketDetails;
            }(TicketDetails.BaseTicketDetails));
            TicketDetails.SportsbookTicketDetails = SportsbookTicketDetails;
            var ParlayTicketDetails = /** @class */ (function (_super) {
                __extends(ParlayTicketDetails, _super);
                function ParlayTicketDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ShowComboParlayDetails = function (refNo, winlostDate, custId, betTeam) {
                        var params = '?refno=' + refNo
                            + '&winlostdate=' + winlostDate
                            + '&custid=' + custId
                            + '&betteam=' + betTeam;
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'Parlay/ComboParlayDetail' + params, "", 800, 438);
                    };
                    _this.ShowMixParlayTickets = function (transId, refNo, custId) {
                        var params = '?transid=' + transId + '&custId=' + custId;
                        var url = Common.AppPath.BetListAppPath() + 'Parlay/MixParlay' + params;
                        _this.showComboMixParlayTickets(transId, '', url);
                    };
                    _this.ShowComboParlayOldTickets = function (transId, transIndex, refNo, custId) {
                        var params = '?transid=' + transId + '&custId=' + custId;
                        var url = Common.AppPath.BetListAppPath() + 'Parlay/ComboParlay' + params;
                        _this.showComboMixParlayTickets(transId, transIndex, url);
                    };
                    _this.ShowComboParlayNewTickets = function (transId, transIndex, refNo, handicap1, handicap2, betCheck, betTeam, custId) {
                        var params = '?transid=' + transId
                            + '&handicap1=' + handicap1
                            + '&handicap2=' + handicap2
                            + '&betcheck=' + betCheck
                            + '&betteam=' + betTeam
                            + '&custId=' + custId;
                        var url = Common.AppPath.BetListAppPath() + 'Parlay/ComboParlay' + params;
                        _this.showComboMixParlayTickets(transId, transIndex, url);
                    };
                    _this.showComboMixParlayTickets = function (transId, transIndex, url) {
                        var ticketsContent = $('#divEvent_' + transId + transIndex);
                        var comboOfferName = $('#combinationOfferName_' + transId + transIndex);
                        var maxPayoutDiv = $('#maxPayout_' + transId + transIndex);
                        if (ticketsContent.html() === '') {
                            var getComboMixParlaySetting = {
                                method: 'GET',
                                url: url,
                                beforeSend: function () {
                                    var blockIconContent = '<span class="block-icon-small"></span>';
                                    ticketsContent.html(blockIconContent);
                                    ticketsContent.show();
                                },
                                success: function (responseHtml) {
                                    ticketsContent.html(responseHtml);
                                    if (ticketsContent.find(".offer-name-list").length) {
                                        comboOfferName.hide();
                                        if (maxPayoutDiv.length) {
                                            maxPayoutDiv.hide();
                                            ticketsContent.find(".max-payout").each(function () {
                                                $(this).show();
                                            });
                                        }
                                    }
                                },
                            };
                            $.ajax(getComboMixParlaySetting);
                        }
                        else {
                            if (ticketsContent.is(':visible')) {
                                ticketsContent.hide();
                                comboOfferName.show();
                                maxPayoutDiv.show();
                            }
                            else {
                                ticketsContent.show();
                                if (ticketsContent.find(".offer-name-list").length) {
                                    comboOfferName.hide();
                                    maxPayoutDiv.hide();
                                }
                            }
                        }
                    };
                    return _this;
                }
                Object.defineProperty(ParlayTicketDetails, "Instance", {
                    get: function () {
                        this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new ParlayTicketDetails() : this.instance;
                        return this.instance;
                    },
                    enumerable: true,
                    configurable: true
                });
                return ParlayTicketDetails;
            }(TicketDetails.BaseTicketDetails));
            TicketDetails.ParlayTicketDetails = ParlayTicketDetails;
            var CashOutHistoryDetails = /** @class */ (function (_super) {
                __extends(CashOutHistoryDetails, _super);
                function CashOutHistoryDetails() {
                    var _this = _super !== null && _super.apply(this, arguments) || this;
                    _this.ViewNewCashOutHistory = function (transId, custId, winlostDate) {
                        var params = '?transid=' + transId
                            + '&winlostdate=' + winlostDate
                            + '&custid=' + custId;
                        Common.PopupDetails.Instance(window.top).Show(Common.AppPath.BetListAppPath() + 'CashOutHistory/Index' + params, "", 600, 438);
                    };
                    return _this;
                }
                Object.defineProperty(CashOutHistoryDetails, "Instance", {
                    get: function () {
                        this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new CashOutHistoryDetails() : this.instance;
                        return this.instance;
                    },
                    enumerable: true,
                    configurable: true
                });
                return CashOutHistoryDetails;
            }(TicketDetails.BaseTicketDetails));
            TicketDetails.CashOutHistoryDetails = CashOutHistoryDetails;
        })(TicketDetails = Choice.TicketDetails || (Choice.TicketDetails = {}));
    })(Choice = BetList.Choice || (BetList.Choice = {}));
})(BetList || (BetList = {}));
var showMP;
var showCombMPDetail;
var showCombMP;
var showNewCombMP;
var showSingleMatchParlayDetail;
var ViewNewCashOutHistory;
$(document).ready(function () {
    showMP = BetList.Choice.TicketDetails.ParlayTicketDetails.Instance.ShowMixParlayTickets;
    showCombMPDetail = BetList.Choice.TicketDetails.ParlayTicketDetails.Instance.ShowComboParlayDetails;
    showCombMP = BetList.Choice.TicketDetails.ParlayTicketDetails.Instance.ShowComboParlayOldTickets;
    showNewCombMP = BetList.Choice.TicketDetails.ParlayTicketDetails.Instance.ShowComboParlayNewTickets;
    showSingleMatchParlayDetail = BetList.Choice.TicketDetails.SportsbookTicketDetails.Instance.ShowSingleMatchParlayDetail;
    ViewNewCashOutHistory = BetList.Choice.TicketDetails.CashOutHistoryDetails.Instance.ViewNewCashOutHistory;
});
//# sourceMappingURL=sportsbook-ticket-details.js.map