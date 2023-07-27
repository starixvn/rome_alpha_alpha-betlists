"use strict";
var FindTicketView = /** @class */ (function () {
    function FindTicketView($) {
        var _this = this;
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
                        feedback_id: 2
                    }, mainDomain);
                }
                else {
                    window.top.indexManager.initFeedbackFunction(2);
                }
            }, 1000);
        };
        this.cacheDOMElements = function () {
            _this.$refNo = _this.$("#ref-no");
            _this.$submitButton = _this.$("#submit-betList-form");
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
        this.onChangeRefNoTextBox();
    };
    return FindTicketView;
}());
$(function () {
    var findTicketView = new FindTicketView($);
    findTicketView.init();
});
//# sourceMappingURL=find-tickets.js.map