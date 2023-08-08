"use strict";
var SearchTicketView = /** @class */ (function () {
    function SearchTicketView($) {
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
            AJAXHelper.get(_this.getBetsUrl, { RefNo: _this.$refNo.val() }, _this.successGetLastBets);
        };
        this.successGetLastBets = function (responseHtml) {
            if ((responseHtml.ErrorCode && responseHtml.ErrorCode !== "0") ||
                (responseHtml.ErrCode && responseHtml.ErrCode !== "0")) {
                return;
            }
            _this.$betListTableContainer.children('tbody').html(responseHtml);
            _this.expandParlay();
        };
        this.onChangeRefNoTextBox = function () {
            _this.$refNo.on('change keyup', function () {
                _this.updateBtnStateOnTextChange();
            });
            _this.$refNo.on('paste', function () {
                setTimeout(_this.updateBtnStateOnTextChange, 100);
            });
        };
        this.$ = $;
    }
    SearchTicketView.prototype.expandParlay = function () {
        var parlayClickableId = "#r" + this.$refNo.val() + " a#hidden0";
        var parlayItem = $(parlayClickableId);
        parlayItem && parlayItem[0] && parlayItem[0].click();
    };
    SearchTicketView.prototype.registerEvents = function () {
        this.$submitButton.prop("disabled", true);
        this.$submitButton.on("click", this.getTickets);
        this.onChangeRefNoTextBox();
    };
    SearchTicketView.prototype.updateBtnStateOnTextChange = function () {
        var content = this.$("#ref-no").val().trim();
        if (content) {
            this.$("#ref-no").val(content);
            this.$("#submit-betList-form").prop("disabled", false);
        }
        else {
            this.$("#submit-betList-form").prop("disabled", true);
        }
    };
    return SearchTicketView;
}());
$(function () {
    new SearchTicketView($).init();
});
//# sourceMappingURL=search-tickets.js.map