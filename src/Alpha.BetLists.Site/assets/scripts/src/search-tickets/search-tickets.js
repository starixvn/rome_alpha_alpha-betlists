"use strict";
var FindTicketView = /** @class */ (function () {
    function FindTicketView($) {
        var _this = this;
        this.getBetsUrl = "";
        this.init = function () {
            _this.cacheDOMElements();
            _this.registerEvents();
            setTimeout(function () {
                var mainDomain = _this.$("#MainDomain").val();
                if (mainDomain.length) {
                    mainDomain = mainDomain.indexOf("http") === -1
                        ? location.origin.split(":")[0] + ":" + mainDomain + "/site-main"
                        : mainDomain;
                    window.top.postMessage({
                        event_id: "winlossdetailnew-feedback",
                        feedback_id: 3
                    }, mainDomain);
                }
                else {
                    window.top.indexManager.initFeedbackFunction(3);
                }
            }, 1000);
        };
        this.cacheDOMElements = function () {
            _this.$refNo = _this.$("#ref-no");
            _this.$submitButton = _this.$("#submit-betList-form");
            _this.$betListTableContainer = _this.$("#tbl-betlist");
            _this.getBetsUrl = _this.$("#get-bets-url").val();
        };
        this.getTickets = function (e) {
            e.preventDefault();
            _this.$.blockUI();
            AJAXHelper.get(_this.getBetsUrl, { RefNo: _this.$("#RefNo").val() }, _this.successGetLastBets);
        };
        this.successGetLastBets = function (responseHtml) {
            if ((responseHtml.ErrorCode && responseHtml.ErrorCode !== "0") ||
                (responseHtml.ErrCode && responseHtml.ErrCode !== "0")) {
                return;
            }
            _this.$betListTableContainer.children('tbody').html(responseHtml);
        };
        this.onChangeRefNoTextBox = function () {
            _this.$refNo.on('change keyup paste', function () {
                var content = _this.$refNo.val().trim();
                if (content) {
                    _this.$("#RefNo").val(content);
                    _this.$submitButton.prop("disabled", false);
                }
                else {
                    _this.$submitButton.prop("disabled", true);
                }
            });
        };
        this.$ = $;
    }
    FindTicketView.prototype.registerEvents = function () {
        this.$submitButton.prop("disabled", true);
        this.$submitButton.on("click", this.getTickets);
        this.onChangeRefNoTextBox();
    };
    return FindTicketView;
}());
$(function () {
    var findTicketView = new FindTicketView($);
    findTicketView.init();
});
//# sourceMappingURL=search-tickets.js.map